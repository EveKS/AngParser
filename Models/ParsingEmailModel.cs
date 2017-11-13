using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AngParser.Models
{
  public class ParsingEmailModel
  {
    [Key]
    public string ParsingEmailModelId { get; set; }

    public bool Sended { get; set; }

    public string Email { get; set; }

    [NotMapped]
    public Uri Uri { get; set; }

    public string UriString { get => Convert.ToString(this.Uri); set => this.Uri = new Uri(value); }

    public string ScaningUriModelId { get; set; }

    public ScaningUriModel ScaningUriModel { get; set; }
  }
}
