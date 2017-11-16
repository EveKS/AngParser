using AngParser.Datas;
using AngParser.Models;
using AngParser.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AngParser.Controllers
{
  [Route("api/[controller]")]
  public class AccountsController : Controller
  {
    private RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AccountsController(UserManager<User> userManager,
        SignInManager<User> signInManager,
        RoleManager<IdentityRole> roleManager)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _roleManager = roleManager;
    }

    // POST api/accounts/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var userIdentity = new User
      {
        Email = model.Email,
        UserName = model.Email,
      };

      var result = await _userManager.CreateAsync(userIdentity, model.Password);

      if (!result.Succeeded)
        return BadRequest();

      return new OkResult();
    }

    // POST api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]LoginViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var user = await _userManager.FindByNameAsync(model.Email);
      if (user == null ||
          !await _userManager.CheckPasswordAsync(user, model.Password))
      {
        return Unauthorized();
      }

      var claims = await GetValidClaims(user);

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("rte_tert_ert_re_tretretert!!"));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var now = DateTime.UtcNow;
      var expires_in = 30;

      var token = new JwtSecurityToken("234234",
        "234234",
        claims,
        expires: now.Add(TimeSpan.FromDays(expires_in)),
        signingCredentials: creds);

      return Ok(new
      {
        id = user.Id,
        user_name = user.UserName,
        auth_token = new JwtSecurityTokenHandler().WriteToken(token),
        expires_in = expires_in
      });
    }

    private async Task<List<Claim>> GetValidClaims(User user)
    {
      IdentityOptions options = new IdentityOptions();
      var claims = new List<Claim>
        {
          new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
          new Claim(options.ClaimsIdentity.UserIdClaimType, user.Id.ToString()),
          new Claim(options.ClaimsIdentity.UserNameClaimType, user.UserName)
        };

      var userClaims = await _userManager.GetClaimsAsync(user);
      var userRoles = await _userManager.GetRolesAsync(user);
      claims.AddRange(userClaims);
      foreach (var userRole in userRoles)
      {
        claims.Add(new Claim(ClaimTypes.Role, userRole));
        var role = await _roleManager.FindByNameAsync(userRole);
        if (role != null)
        {
          var roleClaims = await _roleManager.GetClaimsAsync(role);
          foreach (Claim roleClaim in roleClaims)
          {
            claims.Add(roleClaim);
          }
        }
      }

      return claims;
    }
  }
}
