namespace CelebrationBoard.Api.Celebrations;
public sealed record ToggleFavouriteCelebrationRequest(long PostId, long UserId, bool IsFavourite);