using Newtonsoft.Json;
using System.Collections.Generic;

namespace AngParser.JsonModels.Google
{
  public class Queries
  {
    [JsonProperty("request")]
    public IList<Request> Request { get; set; }

    [JsonProperty("nextPage")]
    public IList<NextPage> NextPage { get; set; }
  }
}
