using Newtonsoft.Json;

namespace AngParser.JsonModels.Google
{
  public class Softwaresourcecode
  {
    [JsonProperty("author")]
    public string Author { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; }

    [JsonProperty("about")]
    public string About { get; set; }

    [JsonProperty("keywords")]
    public string Keywords { get; set; }

    [JsonProperty("datemodified")]
    public string Datemodified { get; set; }
  }
}
