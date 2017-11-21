using Newtonsoft.Json;

namespace AngParser.JsonModels.Google
{
  public class Person
  {
    [JsonProperty("url")]
    public string Url { get; set; }
  }
}
