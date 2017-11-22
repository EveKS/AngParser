using Newtonsoft.Json;
using System.Collections.Generic;

namespace AngParser.JsonModels.Google
{
  public class GoogleSearch
  {
    [JsonProperty("kind")]
    public string Kind { get; set; }

    [JsonProperty("url")]
    public Url Url { get; set; }

    [JsonProperty("queries")]
    public Queries Queries { get; set; }

    [JsonProperty("context")]
    public Context Context { get; set; }

    [JsonProperty("searchInformation")]
    public SearchInformation SearchInformation { get; set; }

    [JsonProperty("items")]
    public IList<Item> Items { get; set; }
  }
}
