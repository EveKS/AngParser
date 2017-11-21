using Newtonsoft.Json;

namespace AngParser.JsonModels.Google
{
  public class Website
  {
    [JsonProperty("url")]
    public string Url { get; set; }
  }
}
