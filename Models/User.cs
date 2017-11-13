using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngParser.Models
{
  public class User : IdentityUser
  {
    public IList<ScaningUriModel> ScaningUriModels { get; set; }

    public User()
    {
      this.ScaningUriModels = new List<ScaningUriModel>();
    }
  }
}
