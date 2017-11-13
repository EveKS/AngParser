using AngParser.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngParser.Datas
{
  public class ApplicationContext : IdentityDbContext<User>
  {
    public DbSet<ParsingEmailModel> ParsingEmailModels { get; set; }
    public DbSet<ScaningUriModel> ScaningUriModels { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }
  }
}
