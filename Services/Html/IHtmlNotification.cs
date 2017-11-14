using AngParser.Datas;
using AngParser.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AngParser.Services.Html
{
  public interface IHtmlNotification
  {
    Task<int?> EmailsCount(string id);
    Task<IEnumerable<ParsingEmailModel>> GetEmails(ApplicationContext context, string id, string userId);
    Task PushEmail(ApplicationContext context, string email, Uri uri, string id, string userId);
    Task<string> PushUri(ApplicationContext context, string userId);
    Task<bool> PushUri(ApplicationContext context, Uri uri, string id, string userId);
  }
}
