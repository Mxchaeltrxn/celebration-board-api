namespace CelebrationBoard.Api.Celebrations;
public sealed record EditCelebrationRequest(long Id, string? Title, string? Content);