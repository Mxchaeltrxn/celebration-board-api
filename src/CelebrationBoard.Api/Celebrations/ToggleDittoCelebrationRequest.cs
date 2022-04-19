namespace CelebrationBoard.Api.Celebrations;
public sealed record ToggleDittoCelebrationRequest(long PostId, long UserId, bool IsDitto);