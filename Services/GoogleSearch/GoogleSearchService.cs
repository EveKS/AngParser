using AngParser.Service.JSON;
using AngParser.Services.Http;
using AngParser.Services.Telegram;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AngParser.Services.GoogleSearch
{
  public class GoogleSearchService : IGoogleSearchService
  {
    private readonly ITelegramService _telegramService;

    private readonly IHttpService _httpService;

    private readonly IJsonService _jsonService;

    private readonly IConfiguration _configuration;

    public GoogleSearchService(IConfiguration configuration, ITelegramService telegramService)
    {
      this._telegramService = telegramService;

      this._jsonService = new JsonService();

      this._httpService = new HttpService();

      this._configuration = configuration;
    }

    private string GetQueryString(string query, int startIndex)
    {
      string key = this._configuration["Google:key"];

      string cx = this._configuration["Google:cx"];

      string queryString = $"https://www.googleapis.com/customsearch/v1?key={key}&cx={cx}&q={HttpUtility.UrlEncode(query)}&start={startIndex}";

      return queryString;
    }

    async Task<IEnumerable<string>> IGoogleSearchService.CustomSearchAsync(string query, int count)
    {
      try
      {
        IEnumerable<string> result = new List<string>(count);

        for (int index = 0; index < count && index < 100;)
        {
          JsonModels.Google.GoogleSearch googleSearch = await CustomSearchAsync(query, index + 1);

          int? startIndex = googleSearch.Queries.NextPage.FirstOrDefault()?.StartIndex;

          if (index == startIndex.Value) break;

          index = startIndex.Value;

          var links = googleSearch.Items.Select(item => item.Link);

          result = result.Concat(links);
        }

        return result.Distinct();
      }
      catch (Exception ex)
      {
        await this._telegramService.SendMessageExceptionAsync(ex);
      }

      return null;
    }

    private async Task<JsonModels.Google.GoogleSearch> CustomSearchAsync(string query, int startIndex)
    {
      string queryString = GetQueryString(query, startIndex);

      string json = await this._httpService.GetAsync(new Uri(queryString));

      return this._jsonService.JsonConvertDeserializeObjectWithNull<JsonModels.Google.GoogleSearch>(json);
    }
  }
}
