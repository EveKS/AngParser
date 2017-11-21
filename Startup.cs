using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using AngParser.Services.Html;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AngParser.Datas;
using Microsoft.AspNetCore.Identity;
using AngParser.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using AngParser.Services.Telegram;
using AngParser.Service.JSON;
using AngParser.Services.GoogleSearch;

namespace AngParser
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      #region DB connection
      //string connection = Configuration["ConnectionStrings:DefaultConnection"];
      string connection = Configuration["ConnectionStrings:RegRu"];
      services.AddDbContext<ApplicationContext>(options =>
          options.UseSqlServer(connection));
      #endregion

      #region Identity
      services.AddIdentity<User, IdentityRole>(options =>
      {
        options.User = new UserOptions
        {
          RequireUniqueEmail = true,
          //AllowedUserNameCharacters = "допустимые символы"
        };

        options.Password = new PasswordOptions
        {
          RequireDigit = true,
          RequireNonAlphanumeric = false,
          RequireUppercase = false,
          RequireLowercase = true,
          RequiredLength = 5,
        };
      })
          .AddEntityFrameworkStores<ApplicationContext>();

      services.AddAuthentication()
          .AddFacebook(facebookOptions =>
          {
            facebookOptions.AppId = "AppId";
            facebookOptions.AppSecret = "AppSecret ";
          });

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
          .AddJwtBearer(cfg =>
          {
            cfg.RequireHttpsMetadata = false;
            cfg.SaveToken = true;

            cfg.TokenValidationParameters = new TokenValidationParameters()
            {
              ValidateIssuer = true,
              ValidIssuer = "234234",

              ValidateAudience = true,
              ValidAudience = "234234",

              ValidateLifetime = true,

              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("rte_tert_ert_re_tretretert!!"))
            };
          });
      #endregion

      #region Gzip Deflate
      services.Configure<GzipCompressionProviderOptions>(options =>
          options.Level = CompressionLevel.Optimal);

      services.AddResponseCompression(options =>
      {
        options.EnableForHttps = true;

        options.MimeTypes = new[]
        {
                    // General
                    "text/plain",
                    // Static files
                    "text/css",
                    "application/javascript",
                    // MVC
                    "text/html",
                    "application/xml",
                    "text/xml",
                    "application/json",
                    "text/json",
                    // Custom
                    "image/svg+xml"
                };

        options.Providers.Add<GzipCompressionProvider>();
      });
      #endregion

      services.AddMvc();

      #region services handlers.
      IServiceProvider provider = services.BuildServiceProvider();

      ApplicationContext applicationContext = provider.GetRequiredService<ApplicationContext>();

      IConfiguration configuration = provider.GetRequiredService<IConfiguration>();

      services.AddTransient<ITelegramService, TelegramService>(option =>
        new TelegramService(configuration));

      services.AddTransient<IGoogleSearchService, GoogleSearchService>(option =>
        new GoogleSearchService(configuration, new TelegramService(configuration)));

      services.AddScoped<IHtmlNotification, HtmlNotification>(option =>
        new HtmlNotification(applicationContext, new TelegramService(configuration)));

      services.AddTransient<IHtmlService, HtmlService>(option =>
        new HtmlService(new TelegramService(configuration)));
      
      services.AddTransient<IJsonService, JsonService>();
      #endregion
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.Use(async (context, next) =>
      {
        await next();
        if (context.Response.StatusCode == 404 &&
           !Path.HasExtension(context.Request.Path.Value) &&
           !context.Request.Path.Value.StartsWith("/api/"))
        {
          context.Request.Path = "/index.html";
          await next();
        }
      });

      app.UseResponseCompression();

      app.UseDefaultFiles();
      app.UseStaticFiles(new StaticFileOptions()
      {
        OnPrepareResponse = content =>
        {
          var time = 7 * 24 * 60 * 60;

          content.Context.Response.Headers[HeaderNames.CacheControl] = $"public,max-age={time}";
          content.Context.Response.Headers[HeaderNames.Expires] = DateTime.UtcNow.AddDays(7).ToString("R"); // Format RFC1123
        }
      });

      app.UseAuthentication();

      app.UseMvcWithDefaultRoute();
    }
  }
}
