namespace CelebrationBoard.Domain.Authentication;

using System.Threading.Tasks;
using CSharpFunctionalExtensions;

public interface IUserManager
{
  Task<Result<string>> CreateUserAsync(string userName, string email, string password);

  Task<Result> DeleteUserAsync(string userId);
}
