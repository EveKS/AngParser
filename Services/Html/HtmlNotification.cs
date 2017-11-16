using AngParser.Datas;
using AngParser.Models;
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
    private ApplicationContext _context;

    private volatile ConcurrentStack<ScaningUriModel> _uris;

    public HtmlNotification(ApplicationContext context)
    {
      this._context = context;

      this._uris = new ConcurrentStack<ScaningUriModel>();
    }

    private static object lockEmails = new object();

    IEnumerable<ParsingEmailModel> IHtmlNotification.GetEmails(string id, string userId)
    {
      ScaningUriModel searchUrls = CreateScaningUriModel(id, userId, 0);

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

      return null;
    }

    string IHtmlNotification.PushUri(string userId, int count)
    {
      ScaningUriModel searchUrls = CreateScaningUriModel(string.Empty, userId, count);

      return searchUrls.ScaningUriModelId;
    }

    bool IHtmlNotification.PushUri(Uri uri, string id, string userId)
    {
      ScaningUriModel searchUrls = CreateScaningUriModel(id, userId, 0);

      if (searchUrls.SearchUri.Contains(uri)) return false;

      searchUrls.SearchUri.Push(uri);

      return true;
    }

    void IHtmlNotification.PushEmail(string email, Uri uri, string id, string userId)
    {
      ScaningUriModel searchUrls = CreateScaningUriModel(id, userId, 0);
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

    int? IHtmlNotification.EmailsCount(string id)
    {
      lock (lockEmails)
      {
        return (this._context.ScaningUriModels
        .FirstOrDefault(u => u.ScaningUriModelId == id))
        ?.ParsingEmailModels?.Count;
      }
    }

    private ScaningUriModel CreateScaningUriModel(string id, string userId, int count)
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
  }
}
