using Newtonsoft.Json;

namespace AngParser.JsonModels.Google
{
  public class Musicalbum
  {
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("byartist")]
    public string Byartist { get; set; }

    [JsonProperty("image")]
    public string Image { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("datepublished")]
    public string Datepublished { get; set; }
  }
}
