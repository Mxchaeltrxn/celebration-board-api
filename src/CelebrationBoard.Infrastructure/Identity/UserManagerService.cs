namespace CelebrationBoard.Infrastructure.Identity;

using CelebrationBoard.Domain.Authentication;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

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
  }


  public async Task<Result> DeleteUserAsync(ApplicationUser user)
  {
    var result = await _userManager.DeleteAsync(user);

    if (result.Succeeded)
    {
      return Result.Success();
    }

    return Result.Failure<string>(result.Errors.Select(e => e.Description).First()); // TODO: Change to take array.
  }
}