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
    IEnumerable<ParsingEmailModel> GetEmails(string id, string userId);
    void PushEmail(string email, Uri uri, string id, string userId);
    string PushUri(string userId, int count);
    bool PushUri(Uri uri, string id, string userId);
  }
}
