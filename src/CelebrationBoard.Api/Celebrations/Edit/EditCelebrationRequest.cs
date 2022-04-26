namespace CelebrationBoard.Api.Celebrations.Edit;
public sealed record EditCelebrationRequest(long UserId, [FromQuery] long CelebrationId, string? Title, string Content);