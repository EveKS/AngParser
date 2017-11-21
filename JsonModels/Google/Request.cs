using Newtonsoft.Json;

namespace AngParser.JsonModels.Google
{
  public class Request
  {
    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("totalResults")]
    public string TotalResults { get; set; }

    [JsonProperty("searchTerms")]
    public string SearchTerms { get; set; }

    [JsonProperty("count")]
    public int Count { get; set; }

    [JsonProperty("startIndex")]
    public int StartIndex { get; set; }

    [JsonProperty("inputEncoding")]
    public string InputEncoding { get; set; }

    [JsonProperty("outputEncoding")]
    public string OutputEncoding { get; set; }

    [JsonProperty("safe")]
    public string Safe { get; set; }

    [JsonProperty("cx")]
    public string Cx { get; set; }
  }
}
