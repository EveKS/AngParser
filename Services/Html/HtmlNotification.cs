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

    async Task<IEnumerable<ParsingEmailModel>> IHtmlNotification.GetEmails(string id, string userId)
    {
      ScaningUriModel searchUrls = await CreateScaningUriModel(id, userId);

      var emails = this._context.ParsingEmailModels
        .Where(e => e.ScaningUriModelId == searchUrls.ScaningUriModelId && !e.Sended);

      if (emails != null)
      {
        var emailList = await emails.ToListAsync();

        await Task.Run(async () =>
        {
          for (int i = 0; i < emailList.Count; i++)
          {
            emailList[i].Sended = true;
          }

          this._context.UpdateRange(emailList);
          await this._context.SaveChangesAsync();

          return emailList;
        });
      }

      return null;
    }

    async Task<string> IHtmlNotification.PushUri(string userId)
    {
      ScaningUriModel searchUrls = await CreateScaningUriModel(string.Empty, userId);

      return searchUrls.ScaningUriModelId;
    }

    async Task<bool> IHtmlNotification.PushUri(Uri uri, string id, string userId)
    {
      ScaningUriModel searchUrls = await CreateScaningUriModel(id, userId);

      if (searchUrls.SearchUri.Contains(uri)) return false;

      searchUrls.SearchUri.Push(uri);

      return true;
    }

    async Task IHtmlNotification.PushEmail(string email, Uri uri, string id, string userId)
    {
      ScaningUriModel searchUrls = await CreateScaningUriModel(id, userId);

      if (!await this._context.ParsingEmailModels.Where(e => e.ScaningUriModelId == searchUrls.ScaningUriModelId)
        .AnyAsync(e => e.Email == email))
      {
        searchUrls.ParsingEmailModels.Add(new ParsingEmailModel
        {
          Email = email,
          Sended = false,
          Uri = uri,
          ScaningUriModelId = id
        });

        await Task.Run(async () =>
        {
          this._context.Update(searchUrls);
          await this._context.SaveChangesAsync();
        });
      }
    }

    async Task<int?> IHtmlNotification.EmailsCount(string id) => (await this._context.ScaningUriModels
      .FirstOrDefaultAsync(u => u.ScaningUriModelId == id))
      ?.ParsingEmailModels.Count;

    private async Task<ScaningUriModel> CreateScaningUriModel(string id, string userId)
    {
      var searchUrls = this._uris.FirstOrDefault(u => u.ScaningUriModelId == id);

      if (searchUrls == null)
      {
        searchUrls = await this._context.ScaningUriModels
          .FirstOrDefaultAsync(u => u.ScaningUriModelId == id);

        if (searchUrls == null)
        {
          searchUrls = new ScaningUriModel()
          {
            UserId = userId
          };

          await this._context.AddAsync(searchUrls);
          await this._context.SaveChangesAsync();
        }

        this._uris.Push(searchUrls);
      }

      return searchUrls;
    }
  }
}
