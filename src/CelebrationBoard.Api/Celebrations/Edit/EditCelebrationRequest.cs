namespace CelebrationBoard.Api.Celebrations.Edit;
public sealed record EditCelebrationRequest(long Id, string? Title, string Content);