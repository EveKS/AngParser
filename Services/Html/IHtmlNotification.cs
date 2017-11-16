using AngParser.Datas;
using AngParser.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AngParser.Services.Html
{
  public interface IHtmlNotification
  {
    int? EmailsCount(string id);
    Task<IEnumerable<ParsingEmailModel>> GetEmails(string id, string userId);
    Task PushEmail(string email, Uri uri, string id, string userId);
    Task<string> PushUri(string userId, int count);
    Task<bool> PushUri(Uri uri, string id, string userId);
  }
}
