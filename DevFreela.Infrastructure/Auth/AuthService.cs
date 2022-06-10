using DevFreela.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace DevFreela.Infrastructure.Auth
{
  public class AuthService : IAuthService
  {
    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration)
    {
      _configuration = configuration;
    }
    public string GenerateJwtToken(string email, string role)
    {
      var issuer = _configuration["Jwt:Issuer"];
      var audience = _configuration["Jwt:Audience"];
      var key = _configuration["Jwt:Key"];
      var securetyKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
      var credentials = new SigningCredentials(securetyKey, SecurityAlgorithms.HmacSha256);

      var claims = new List<Claim>
      {
          new Claim("userName", email),
          new Claim(ClaimTypes.Role, role)
       };
      var token = new JwtSecurityToken(
       issuer: issuer,
       audience: audience,
       expires: DateTime.Now.AddHours(8),
       signingCredentials: credentials,
       claims: claims);

      var tokenHandler = new JwtSecurityTokenHandler();

      var stringToken = tokenHandler.WriteToken(token);
      return stringToken;
    }
  }
}