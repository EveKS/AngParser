using Newtonsoft.Json;

namespace AngParser.JsonModels.Google
{
  public class Searchaction
  {
    [JsonProperty("target")]
    public string Target { get; set; }

    [JsonProperty("query-input")]
    public string QueryInput { get; set; }
  }
}
