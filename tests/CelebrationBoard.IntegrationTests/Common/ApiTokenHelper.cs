namespace CelebrationBoard.IntegrationTests.Common;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;


public class ApiTokenHelper
{
  public static string GetNormalUserToken()
  {
    string userName = "demouser@app.com";

    return CreateToken(userName);
  }

  private static string CreateToken(string userName)
  {
    var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName),
    new Claim(ClaimTypes.Email, userName)};

    var key = Encoding.ASCII.GetBytes("JWTAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr");
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(claims.ToArray()),
      Expires = DateTime.UtcNow.AddHours(1),
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };
    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }
}
