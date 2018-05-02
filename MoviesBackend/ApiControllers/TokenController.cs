using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MoviesBackend.Exceptions;

namespace MoviesBackend.ApiControllers
{
  [Produces("application/json")]
  [Route("api/Token")]
  public class TokenController : Controller
  {
    private readonly UserManager<IdentityUser> _userManager;
    public const string JWT_SECRET_ENV_VAR = "MOVIES_JWT_SECRET_KEY";

    public TokenController(UserManager<IdentityUser> userManager)
    {
      _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> Create(string email, string password)
    {
      if (email == null || password == null)
      {
        return Unauthorized();
      }
      IdentityUser user = await _userManager.FindByEmailAsync(email);
      if (user == null)
      {
        return Unauthorized();
      }

      // TODO: check if user is priveledged to have a tokens

      return new ObjectResult(GenerateToken(user));
    }

    private string GenerateToken(IdentityUser user)
    {
      Claim[] claims = new Claim[]
      {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds()}"),
        new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}")
      };

      string jwtSecret = Environment.GetEnvironmentVariable(JWT_SECRET_ENV_VAR);
      if (jwtSecret == null)
      {
        throw new JwtException($"Couldn't find JWT secret key environment variable: ${JWT_SECRET_ENV_VAR}");
      }
      SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

      JwtSecurityToken token = new JwtSecurityToken(
        new JwtHeader(new SigningCredentials(
          secretKey,
          SecurityAlgorithms.HmacSha256
        )),
        new JwtPayload(claims)
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}