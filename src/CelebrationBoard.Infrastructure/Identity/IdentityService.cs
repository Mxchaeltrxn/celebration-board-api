namespace CelebrationBoard.Infrastructure.Identity;

using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CelebrationBoard.Domain.Authentication;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

public class IdentityService : IIdentityService
{
  private readonly UserManager<ApplicationUser> _userManager;

  public IdentityService(UserManager<ApplicationUser> userManager)
  {
    _userManager = userManager;
  }

  public async Task<Result<string>> CreateUserAsync(string username, string email, string password)
  {
    var user = new ApplicationUser(userName: username, email: email);

    var result = await _userManager.CreateAsync(user, password);
    if (result.Succeeded)
    {
      return Result.Success(user.Id);
    }

    return Result.Failure<string>(result.Errors.Select(e => e.Description).First()); // TODO: Change to take array.
  }

  public async Task<Result> DeleteUserAsync(string userId)
  {
    var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

    if (user is not null)
      return await DeleteUserAsync(user);

    return Result.Success();

    async Task<Result> DeleteUserAsync(ApplicationUser user)
    {
      var result = await _userManager.DeleteAsync(user);

      if (result.Succeeded)
      {
        return Result.Success();
      }

      return Result.Failure<string>(result.Errors.Select(e => e.Description).First()); // TODO: Change to take array.
    }
  }

  public async Task<bool> CheckPasswordAsync(string username, string password)
  {
    var user = await _userManager.FindByNameAsync(username);

    if (user is null)
      return false;

    return await _userManager.CheckPasswordAsync(user, password);
  }

  public async Task<string> GenerateTokenAsync(string userName, string jwtSecret, string jwtValidIssuer, string jwtValidAudience)
  {
    var user = await _userManager.FindByNameAsync(userName);

    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                };

    var token = GetToken(authClaims);

    return new JwtSecurityTokenHandler().WriteToken(token);

    JwtSecurityToken GetToken(List<Claim> authClaims)
    {
      var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

      var token = new JwtSecurityToken(
          issuer: jwtValidIssuer,
          audience: jwtValidAudience,
          expires: DateTime.Now.AddHours(3000), // TODO: What value should this be?
          claims: authClaims,
          signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
          );

      return token;
    }
  }

  public async Task<bool> IsDuplicateEmailAsync(string emailAddress)
  {
    return await _userManager.FindByEmailAsync(emailAddress) is not null;
  }

  public async Task<bool> IsDuplicateUsernameAsync(string username)
  {
    return await _userManager.FindByNameAsync(username) is not null;
  }
}