namespace CelebrationBoard.Domain.Authentication;

using System.Threading.Tasks;
using CSharpFunctionalExtensions;

public interface IUserManager
{
  public Task<Result<string>> CreateUserAsync(string userName, string email, string password);
  public Task<Result> DeleteUserAsync(string userId);
}
