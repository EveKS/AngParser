using AngParser.Datas;
using AngParser.Models;
using AngParser.Services.Email;
using AngParser.Services.Http;
using AngParser.Services.Telegram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AngParser.Services.Html
{
  public class HtmlService : IHtmlService
  {
    private volatile IHtmlNotification _htmlNotification;

    private readonly ITelegramService _telegramService;

    private readonly IEmailValidation _emailValidation;

    private readonly IHtmlParser _htmlParser;

    private readonly IHttpService _httpService;

    public HtmlService()
    {
      this._telegramService = new TelegramService();

      this._emailValidation = new EmailValidation();

      this._httpService = new HttpService();

      this._htmlParser = new HtmlParser();
    }

    void IHtmlService.CreateHtmlNotification(IHtmlNotification htmlNotification)
    {
      this._htmlNotification = htmlNotification;
    }

    async Task IHtmlService.DeepAdd(string userId, string id, Uri uri,
      Uri mainUri, int count, CancellationToken token)
    {
      await this.DeepAdd(userId, id, uri, mainUri, count, token);
    }

    private async Task DeepAdd(string userId, string id, Uri uri,
      Uri mainUri, int count, CancellationToken token)
    {
      try
      {
        if (token.IsCancellationRequested) return;

        var eCount = this._htmlNotification.EmailsCount(id);

        if (mainUri == null || this.UriHaveCircle(uri)
            || (eCount != null ? eCount : 0) > count) return;

        var baseDomain = mainUri.Host.Replace("www.", string.Empty);

        var thisDomain = uri.Host.Replace("www.", string.Empty);

        var isbaseDomain = (!string.IsNullOrWhiteSpace(baseDomain) || !string.IsNullOrWhiteSpace(thisDomain))
            && (baseDomain.Contains(thisDomain) || thisDomain.Contains(baseDomain));

        if (isbaseDomain && uri.ToString() != "#"
            && await this._htmlNotification.PushUri(uri, id, userId))
        {
          var html = await _httpService.GetAsync(uri);

          var emails = this._htmlParser.AngParser(html);

          foreach (var email in emails)
          {
            if (!token.IsCancellationRequested && this._emailValidation.IsValidEmail(email))
            {
              await this._htmlNotification.PushEmail(email, uri, id, userId);
            }
          }

          var urls = _htmlParser.GetLinks(html, uri).ToList();

          foreach (var url in urls)
          {
            if (Uri.TryCreate(url, UriKind.Absolute, out Uri u))
            {
              if (u.Scheme == Uri.UriSchemeHttp || u.Scheme == Uri.UriSchemeHttps)
              {
                await this.DeepAdd(userId, id, u, mainUri, count, token);
              }
              else if (u.Scheme == Uri.UriSchemeMailto)
              {
                var email = this._htmlParser.AngParser(url).FirstOrDefault();

                if (!token.IsCancellationRequested && !string.IsNullOrWhiteSpace(email) && this._emailValidation.IsValidEmail(email))
                {
                  await _htmlNotification.PushEmail(email, uri, id, userId);
                }
              }
            }
          }
        }
      }
      catch(Exception ex)
      {
        await this._telegramService.SendMessageExceptionAsync(ex);
      }
    }

    private bool UriHaveCircle(Uri uri)
    {
      var haveCircle = uri.Query.Split('+')
          .GroupBy(query => query)
          .Any(group => group.Count() > 1);

      haveCircle |= uri.Query.Split('%')
          .GroupBy(query => query)
          .Any(group => group.Count() > 1);

      return haveCircle;
    }
  }
}
