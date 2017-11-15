using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AngParser.Services.Html;
using AngParser.Models;
using AngParser.Datas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace AngParser.Controllers
{
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [Route("api/[controller]")]
  public class EmailParserController : Controller
  {
    private CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();

    private ApplicationContext _context;

    private readonly UserManager<User> _userManager;

    private IHtmlNotification _htmlNotification;

    private IHtmlService _htmlService;

    public EmailParserController(IHtmlService htmlService,
      UserManager<User> userManager,
      IHtmlNotification htmlNotification,
      ApplicationContext context)
    {
      this._userManager = userManager;

      this._htmlService = htmlService;

      htmlService.CreateHtmlNotification(htmlNotification);

      this._context = context;

      this._htmlNotification = htmlNotification;
    }

    // POST api/emailparser/start
    [HttpPost("start")]
    public async Task<IActionResult> Start([FromBody]Start start)
    {
      if (string.IsNullOrWhiteSpace(start.Message) || start.Count == 0) return BadRequest();

      var userName = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

      if (string.IsNullOrWhiteSpace(userName))
      {
        return Unauthorized();
      }

      var user = await _userManager.FindByNameAsync(userName);

      var id = this.Run(user.Id, start.Count, start.Message.Split(new[] { ' ', '\n' }).Select(uri => new Uri(uri)));

      return Ok(new { ok = "ok", id = id });
    }

    // Post api/emailparser/get-emails
    [HttpPost("get-emails")]
    public async Task<IActionResult> AngParser([FromBody]string id)
    {
      var userName = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

      if (string.IsNullOrWhiteSpace(userName))
      {
        return Unauthorized();
      }

      var scaningUriModels = await this._context.ScaningUriModels
        .FirstOrDefaultAsync(s => s.ScaningUriModelId == id);

      if (scaningUriModels == null) return BadRequest();

      var user = await _userManager.FindByNameAsync(userName);

      var findCount = this._htmlNotification.EmailsCount(id);

      var message = this._htmlNotification.GetEmails( id, user.Id);

      while (message == null || message.Count() <= 0)
      {
        await Task.Delay(500);

        message = this._htmlNotification.GetEmails(id, user.Id);
      }

      var rnd = new Random();

      if (scaningUriModels.Count > findCount)
      {
        this.Stop();
      }

      return Ok(new { Continue = scaningUriModels.Count > findCount, emails = message.Select(mes => mes.Email) });
    }

    private string Run(string userId, int count, IEnumerable<Uri> urls)
    {
      var id = _htmlNotification.PushUri(userId, count);

      this.RunTasks(userId, id, count, urls);

      return id;
    }

    private async void RunTasks(string userId, string id, int count, IEnumerable<Uri> urls)
    {
      var tasks = MainAsync(userId, id, count, urls);

      for (int i = 0; i < tasks.Count; i++)
      {
        await tasks[i];
      }
    }

    private List<Task> MainAsync(string userId, string id, int count, IEnumerable<Uri> urls)
    {
      var tasks = new List<Task>(urls.Count());

      foreach (var url in urls)
      {
        var task = Task.Run(async () =>
        {
          await this._htmlService.DeepAdd(userId, id, url, url, count, this._cancelTokenSource.Token);
        }, _cancelTokenSource.Token);

        tasks.Add(task);
      }

      return tasks;
    }

    private void Stop()
    {
      this._cancelTokenSource.Cancel();
    }

    public class Message
    {
      public List<string> Emails { get; set; }
      public bool Continue { get; set; }
    }
  }

  public class Start
  {
    public string Message { get; set; }

    public int Count { get; set; }
  }
}
