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
using AngParser.Services.Telegram;
using Microsoft.AspNetCore.Http;
using AngParser.Services.GoogleSearch;

namespace AngParser.Controllers
{
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [Route("api/[controller]")]
  public class UrlParserController : Controller
  {
    private ApplicationContext _context;

    private readonly IGoogleSearchService _googleSearchService;

    private readonly ITelegramService _telegramService;

    private readonly UserManager<User> _userManager;

    public UrlParserController(UserManager<User> userManager,
      ITelegramService telegramService,
      ApplicationContext context,
      IGoogleSearchService googleSearchService)
    {
      this._googleSearchService = googleSearchService;

      this._userManager = userManager;

      this._context = context;

      this._telegramService = telegramService;
    }

    // POST api/emailparser/start
    [HttpPost("get-urls")]
    public async Task<IActionResult> GetUrls([FromBody]StartParametrs startParametrs)
    {
      try
      {
        if (string.IsNullOrWhiteSpace(startParametrs.Message) || startParametrs.Count == 0) return BadRequest();

        var userName = HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userName))
        {
          return Unauthorized();
        }

        var user = await _userManager.FindByNameAsync(userName);

        var messages = await this._googleSearchService.CustomSearchAsync(startParametrs.Message, startParametrs.Count);

        return Ok(new { ok = "ok", messages = messages });
      }
      catch (Exception ex)
      {
        await this._telegramService.SendMessageExceptionAsync(ex);

        return StatusCode(500, ex);
      }
    }
  }
}
