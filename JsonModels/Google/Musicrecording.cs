using Newtonsoft.Json;

namespace AngParser.JsonModels.Google
{
  public class Musicrecording
  {
    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("duration")]
    public string Duration { get; set; }
  }
}
