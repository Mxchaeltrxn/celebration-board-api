namespace CelebrationBoard.Domain.Authentication;

public interface ICurrentUserService
{
  string? UserId { get; }
}
