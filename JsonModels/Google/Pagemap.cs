using Newtonsoft.Json;
using System.Collections.Generic;

namespace AngParser.JsonModels.Google
{
  public class Pagemap
  {
    [JsonProperty("cse_thumbnail")]
    public IList<CseThumbnail> CseThumbnail { get; set; }

    [JsonProperty("metatags")]
    public IList<Metatag> Metatags { get; set; }

    [JsonProperty("cse_image")]
    public IList<CseImage> CseImage { get; set; }

    [JsonProperty("website")]
    public IList<Website> Website { get; set; }

    [JsonProperty("searchaction")]
    public IList<Searchaction> Searchaction { get; set; }

    [JsonProperty("softwaresourcecode")]
    public IList<Softwaresourcecode> Softwaresourcecode { get; set; }

    [JsonProperty("listitem")]
    public IList<Listitem> Listitem { get; set; }

    [JsonProperty("musicalbum")]
    public IList<Musicalbum> Musicalbum { get; set; }

    [JsonProperty("musicrecording")]
    public IList<Musicrecording> Musicrecording { get; set; }

    [JsonProperty("musicgroup")]
    public IList<Musicgroup> Musicgroup { get; set; }

    [JsonProperty("imageobject")]
    public IList<Imageobject> Imageobject { get; set; }

    [JsonProperty("person")]
    public IList<Person> Person { get; set; }

    [JsonProperty("videoobject")]
    public IList<Videoobject> Videoobject { get; set; }
  }
}
