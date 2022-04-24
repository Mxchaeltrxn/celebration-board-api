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

public class UserManagerService : IUserManager
{
  private readonly UserManager<ApplicationUser> _userManager;

  public UserManagerService(UserManager<ApplicationUser> userManager)
  {
    _userManager = userManager;
  }

  public async Task<Result<string>> CreateUserAsync(string userName, string email, string password)
  {
    var user = new ApplicationUser(userName: userName, email: email);

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

  public async Task<bool> CheckPasswordAsync(string userName, string password)
  {
    var user = await FindByUserNameAsync(userName);
    if (user is null)
      return false;

    return await _userManager.CheckPasswordAsync(user, password);
  }

  public async Task<ApplicationUser> FindByUserNameAsync(string userName)
  {
    return await _userManager.FindByNameAsync(userName);
  }

  public async Task<string> GenerateToken(string userName, string jwtSecret, string jwtValidIssuer, string jwtValidAudience)
  {
    var user = await FindByUserNameAsync(userName);

    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("id", user.Id.ToString())
                };

    var token = GetToken(authClaims);

    return new JwtSecurityTokenHandler().WriteToken(token);

    JwtSecurityToken GetToken(List<Claim> authClaims)
    {
      var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

      var token = new JwtSecurityToken(
          issuer: jwtValidIssuer,
          audience: jwtValidAudience,
          expires: DateTime.Now.AddHours(3000),
          claims: authClaims,
          signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
          );

      return token;
    }
  }
}