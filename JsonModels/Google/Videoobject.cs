using Newtonsoft.Json;

namespace AngParser.JsonModels.Google
{
  public class Videoobject
  {
    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("paid")]
    public string Paid { get; set; }

    [JsonProperty("channelid")]
    public string Channelid { get; set; }

    [JsonProperty("videoid")]
    public string Videoid { get; set; }

    [JsonProperty("duration")]
    public string Duration { get; set; }

    [JsonProperty("unlisted")]
    public string Unlisted { get; set; }

    [JsonProperty("thumbnailurl")]
    public string Thumbnailurl { get; set; }

    [JsonProperty("embedurl")]
    public string Embedurl { get; set; }

    [JsonProperty("playertype")]
    public string Playertype { get; set; }

    [JsonProperty("width")]
    public string Width { get; set; }

    [JsonProperty("height")]
    public string Height { get; set; }

    [JsonProperty("isfamilyfriendly")]
    public string Isfamilyfriendly { get; set; }

    [JsonProperty("regionsallowed")]
    public string Regionsallowed { get; set; }

    [JsonProperty("interactioncount")]
    public string Interactioncount { get; set; }

    [JsonProperty("datepublished")]
    public string Datepublished { get; set; }

    [JsonProperty("genre")]
    public string Genre { get; set; }
  }
}
