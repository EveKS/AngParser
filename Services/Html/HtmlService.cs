using AngParser.Datas;
using AngParser.Models;
using AngParser.Services.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AngParser.Services.Html
{
  public class HtmlService : IHtmlService
  {
    private volatile static IHtmlNotification _htmlNotification;

    private IHtmlParser _htmlParser;

    private IHttpService _httpService;

    public HtmlService()
    {
      this._httpService = new HttpService();

      this._htmlParser = new HtmlParser();
    }

    void IHtmlService.CreateHtmlNotification(IHtmlNotification htmlNotification)
    {
      if (HtmlService._htmlNotification == null)
      {
        HtmlService._htmlNotification = htmlNotification;
      }
    }

    async Task IHtmlService.DeepAdd(string userId, string id, Uri uri, Uri mainUri, int count)
    {
      await this.DeepAdd(userId, id, uri, mainUri, count);
    }

    private async Task DeepAdd(string userId, string id, Uri uri, Uri mainUri, int count)
    {
      var eCount = await HtmlService._htmlNotification.EmailsCount(id);

      if (mainUri == null || this.UriHaveCircle(uri)
          || (eCount != null ? eCount : 0) > count) return;

      var baseDomain = mainUri.Host.Replace("www.", string.Empty);

      var thisDomain = uri.Host.Replace("www.", string.Empty);

      var isbaseDomain = (!string.IsNullOrWhiteSpace(baseDomain) || !string.IsNullOrWhiteSpace(thisDomain))
          && (baseDomain.Contains(thisDomain) || thisDomain.Contains(baseDomain));

      if (isbaseDomain && uri.ToString() != "#"
          && await _htmlNotification.PushUri(uri, id, userId))
      {
        var html = await _httpService.GetAsync(uri);

        var emails = _htmlParser.AngParser(html);

        foreach (var email in emails)
        {
          await _htmlNotification.PushEmail(email, uri, id, userId);
        }

        var urls = _htmlParser.GetLinks(html, uri).ToList();

        foreach (var url in urls)
        {
          if (Uri.TryCreate(url, UriKind.Absolute, out Uri u))
          {
            if (u.Scheme == Uri.UriSchemeHttp || u.Scheme == Uri.UriSchemeHttps)
            {
              await this.DeepAdd(userId, id, u, mainUri, count);
            }
            else if (u.Scheme == Uri.UriSchemeMailto)
            {
              var email = _htmlParser.AngParser(url).FirstOrDefault();

              if (!string.IsNullOrWhiteSpace(email))
              {
                await _htmlNotification.PushEmail(email, uri, id, userId);
              }
            }
          }
        }
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