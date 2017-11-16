using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using System.Threading;

namespace AngParser.Services.Telegram
{
  class TelegramService : ITelegramService
  {
    const string TOKEN = "344652520:AAE6zsadftMdDgalmz2H3vEMq52eAR5bjag";

    async Task ITelegramService.SendMessageExceptionAsync(Exception ex)
    {
      var appendInfo = $"{DateTime.Now:dd.MM.yy hh:mm:ss}\texception";

      string[] exDetail =
      {
        $"Member name:\t{ex.TargetSite}",
        $"Class defining member:\t{ ex.TargetSite.DeclaringType}",
        $"Member Type:\t{ex.TargetSite.MemberType}",
        $"Message:\t{ex.Message}",
        $"Source:\t{ex.Source}",
        $"Help Link:\t{ex.HelpLink}",
        $"Stack:\t{ex.StackTrace}",
      };

      await SendMessagePrivate($"{appendInfo}\n{string.Join(Environment.NewLine, exDetail)}");
    }

    async Task ITelegramService.SendMessage(string message)
    {
      await SendMessagePrivate(message);
    }

    private async Task SendMessagePrivate(string text)
    {
      using (var httpClient = new HttpClient())
      {
        var url = "https://api.telegram.org/bot" + TOKEN +
            "/sendMessage?";

        using (var content = new FormUrlEncodedContent(new[]
        {
                    new KeyValuePair<string, string>("chat_id", "273841531"),
                    new KeyValuePair<string, string>("text", text)
                }))
        {
          await httpClient.PostAsync(url, content);
        }
      }
    }
  }
}
