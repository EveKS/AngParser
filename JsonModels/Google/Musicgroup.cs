using Newtonsoft.Json;

namespace AngParser.JsonModels.Google
{
  public class Musicgroup
  {
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }
  }
}
