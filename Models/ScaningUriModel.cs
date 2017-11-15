using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AngParser.Models
{
  public class ScaningUriModel
  {
    [Key]
    public string ScaningUriModelId { get; set; }

    public string QueryString { get; set; }

    public int Count { get; set; }

    [NotMapped]
    public ConcurrentStack<Uri> SearchUri { get; set; }

    public string UserId { get; set; }

    public User User { get; set; }

    public IList<ParsingEmailModel> ParsingEmailModels { get; set; }

    public ScaningUriModel()
    {
      this.ParsingEmailModels = new List<ParsingEmailModel>();

      this.SearchUri = new ConcurrentStack<Uri>();
    }
  }
}
