namespace CelebrationBoard.Domain.Authentication;

using System.Threading.Tasks;
using CSharpFunctionalExtensions;

public interface IIdentityService
{
  public Task<Result<string>> CreateUserAsync(string username, string email, string password);
  public Task<bool> IsDuplicateEmailAsync(string emailAddress);
  public Task<bool> IsDuplicateUsernameAsync(string username);
  public Task<Result> DeleteUserAsync(string userId);
  public Task<bool> CheckPasswordAsync(string username, string password);
  public Task<string> GenerateTokenAsync(string userName, string jwtSecret, string jwtValidIssuer, string jwtValidAudience);
}
