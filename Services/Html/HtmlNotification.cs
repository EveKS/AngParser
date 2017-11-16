using AngParser.Datas;
using AngParser.Models;
using AngParser.Services.Telegram;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AngParser.Services.Html
{
  public class HtmlNotification : IHtmlNotification
  {
    private readonly ITelegramService _telegramService;

    private readonly ApplicationContext _context;

    private volatile ConcurrentStack<ScaningUriModel> _uris;

    public HtmlNotification(ApplicationContext context)
    {
      this._context = context;

      this._telegramService = new TelegramService();

      this._uris = new ConcurrentStack<ScaningUriModel>();
    }

    private static object lockEmails = new object();

    async Task<IEnumerable<ParsingEmailModel>> IHtmlNotification.GetEmails(string id, string userId)
    {
      try
      {
        ScaningUriModel searchUrls = await CreateScaningUriModel(id, userId, 0);

        if (searchUrls == null) return null;

        lock (lockEmails)
        {
          var emails = this._context.ParsingEmailModels
            .Where(e => e.ScaningUriModelId == id && !e.Sended)
            .ToList();

          if (emails != null)
          {
            for (int i = 0; i < emails.Count; i++)
            {
              emails[i].Sended = true;
            }

            this._context.SaveChanges();

            return emails;
          }
        }
      }
      catch (Exception ex)
      {
        await this._telegramService.SendMessageExceptionAsync(ex);
      }

      return null;
    }

    async Task<string> IHtmlNotification.PushUri(string userId, int count)
    {
      ScaningUriModel searchUrls = await CreateScaningUriModel(string.Empty, userId, count);

      if (searchUrls == null) return null;

      return searchUrls.ScaningUriModelId;
    }

    async Task<bool> IHtmlNotification.PushUri(Uri uri, string id, string userId)
    {
      ScaningUriModel searchUrls = await CreateScaningUriModel(id, userId, 0);

      if (searchUrls == null) return false;

      if (searchUrls.SearchUri.Contains(uri)) return false;

      searchUrls.SearchUri.Push(uri);

      return true;
    }

    async Task IHtmlNotification.PushEmail(string email, Uri uri, string id, string userId)
    {
      try
      {
        ScaningUriModel searchUrls = await CreateScaningUriModel(id, userId, 0);

        if (searchUrls == null) return;

        lock (lockEmails)
        {
          if (!this._context.ParsingEmailModels.Where(e => e.ScaningUriModelId == id)
          .Any(e => e.Email == email))
          {
            var parsingEmailModel = new ParsingEmailModel
            {
              Email = email,
              Sended = false,
              Uri = uri,
              ScaningUriModelId = id
            };

            this._context.Add(parsingEmailModel);
            this._context.SaveChanges();
          }
        }
      }
      catch (Exception ex)
      {
        await this._telegramService.SendMessageExceptionAsync(ex);
      }
    }

    int? IHtmlNotification.EmailsCount(string id)
    {
      lock (lockEmails)
      {
        return (this._context.ScaningUriModels
          .FirstOrDefault(u => u.ScaningUriModelId == id))
          ?.ParsingEmailModels?.Count;
      }
    }

    private async Task<ScaningUriModel> CreateScaningUriModel(string id, string userId, int count)
    {
      try
      {
        var searchUrls = this._uris.FirstOrDefault(u => u.ScaningUriModelId == id);

        if (searchUrls == null)
        {
          lock (lockEmails)
          {
            searchUrls = this._context.ScaningUriModels
            .FirstOrDefault(u => u.ScaningUriModelId == id);

            if (searchUrls == null)
            {
              {
                searchUrls = new ScaningUriModel()
                {
                  UserId = userId,
                  Count = count
                };

                this._context.Add(searchUrls);
                this._context.SaveChanges();
              }

              this._uris.Push(searchUrls);
            }
          }
        }

        return searchUrls;
      }
      catch (Exception ex)
      {
        await this._telegramService.SendMessageExceptionAsync(ex);
      }

      return null;
    }
  }
}
