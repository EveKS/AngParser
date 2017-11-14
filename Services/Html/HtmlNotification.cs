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

    async Task<IEnumerable<ParsingEmailModel>> IHtmlNotification.GetEmails(ApplicationContext context, string id, string userId)
    {
      ScaningUriModel searchUrls = await CreateScaningUriModel(context, id, userId);

      var emails = await context.ParsingEmailModels
        .Where(e => e.ScaningUriModelId == searchUrls.ScaningUriModelId && !e.Sended)
        .ToListAsync();

      if (emails != null)
      {
        for (int i = 0; i < emails.Count; i++)
        {
          emails[i].Sended = true;
        }

        //this._context.UpdateRange(emails);
        await context.SaveChangesAsync();

        return emails;
      }

      return null;
    }

    async Task<string> IHtmlNotification.PushUri(ApplicationContext context, string userId)
    {
      ScaningUriModel searchUrls = await CreateScaningUriModel(context, string.Empty, userId);

      return searchUrls.ScaningUriModelId;
    }

    async Task<bool> IHtmlNotification.PushUri(ApplicationContext context, Uri uri, string id, string userId)
    {
      ScaningUriModel searchUrls = await CreateScaningUriModel(context, id, userId);

      if (searchUrls.SearchUri.Contains(uri)) return false;

      searchUrls.SearchUri.Push(uri);

      return true;
    }

    async Task IHtmlNotification.PushEmail(ApplicationContext context, string email, Uri uri, string id, string userId)
    {
      ScaningUriModel searchUrls = await CreateScaningUriModel(context, id, userId);

      if (!await this._context.ParsingEmailModels.Where(e => e.ScaningUriModelId == searchUrls.ScaningUriModelId)
        .AnyAsync(e => e.Email == email))
      {
        var parsingEmailModel = new ParsingEmailModel
        {
          Email = email,
          Sended = false,
          Uri = uri,
          ScaningUriModelId = id
        };

        await this._context.AddAsync(parsingEmailModel);
        await this._context.SaveChangesAsync();
      }
    }

    async Task<int?> IHtmlNotification.EmailsCount(string id) => (await this._context.ScaningUriModels
      .FirstOrDefaultAsync(u => u.ScaningUriModelId == id))
      ?.ParsingEmailModels?.Count;

    private async Task<ScaningUriModel> CreateScaningUriModel(ApplicationContext context, string id, string userId)
    {
      var searchUrls = this._uris.FirstOrDefault(u => u.ScaningUriModelId == id);

      if (searchUrls == null)
      {
        searchUrls = await context.ScaningUriModels
          .FirstOrDefaultAsync(u => u.ScaningUriModelId == id);

        if (searchUrls == null)
        {
          searchUrls = new ScaningUriModel()
          {
            UserId = userId
          };

          await context.AddAsync(searchUrls);
          await context.SaveChangesAsync();
        }

        this._uris.Push(searchUrls);
      }

      return searchUrls;
    }
  }
}
