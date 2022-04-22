namespace CelebrationBoard.Api.Celebrations;

public sealed record CelebrationDto(
  long Id,
  string Title,
  string Content,
  string PrivacyLevel
);