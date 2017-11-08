using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AngParser.Services.Html;
using AngParser.Models;

namespace AngParser.Controllers
{
    [Route("api/[controller]")]
    public class EmailParserController : Controller
    {
        private IHtmlNotification _htmlNotification;

        private IHtmlService _htmlService;

        public EmailParserController()
        {
            this._htmlNotification = HtmlNotification.Instance;

            this._htmlService = HtmlService.Instance;
        }

        // POST api/emailparser/start
        [HttpPost("start")]
        public IActionResult Start([FromBody]string message)
        {
            Run(100, message.Split(new[] { ' ', '\n' }).Select(uri => new Uri(uri)));

            return Ok(new { ok = "ok" });
        }

        // GET api/emailparser/get-emails
        [HttpGet("get-emails")]
        public async Task <IActionResult> AngParser()
        {
            var message = this._htmlNotification.AngParser().Select(mes => mes.Email);

            while (message.Count() <= 0)
            {
                await Task.Delay(250);

                message = this._htmlNotification.AngParser().Select(mes => mes.Email);
            }

            return Ok(new { Continue = this._htmlNotification.FindCount < 100, emails = message });
        }

        private async void Run(int count, IEnumerable<Uri> urls)
        {
            foreach (var task in MainAsync(urls, count))
            {
                await task;
            }
        }

        private List<Task> MainAsync(IEnumerable<Uri> urls, int count)
        {
            var tasks = new List<Task>(urls.Count());

            foreach (var url in urls)
            {
                var task = Task.Run(async () =>
                {
                    IHtmlService htmlService = HtmlService.Instance;

                    await htmlService.DeepAdd(url, url, count);
                });

                tasks.Add(task);
            }

            return tasks;
        }

        public class Message
        {
            public List<string> Emails { get; set; }
            public bool Continue { get; set; }
        }
    }
}
