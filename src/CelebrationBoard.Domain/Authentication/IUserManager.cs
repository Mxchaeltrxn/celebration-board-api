namespace CelebrationBoard.Domain.Authentication;

using CSharpFunctionalExtensions;
using System.Threading.Tasks;

public interface IUserManager
{
  Task<Result<string>> CreateUserAsync(string userName, string email, string password);

  Task<Result> DeleteUserAsync(string userId);
}
