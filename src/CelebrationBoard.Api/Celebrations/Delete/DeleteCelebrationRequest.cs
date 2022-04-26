namespace CelebrationBoard.Api.Celebrations.Delete;
public sealed record DeleteCelebrationRequest(long UserId, [FromQuery] long CelebrationId);