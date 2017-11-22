using Newtonsoft.Json;

namespace AngParser.JsonModels.Google
{
  public class Context
  {

    [JsonProperty("title")]
    public string Title { get; set; }
  }
}
