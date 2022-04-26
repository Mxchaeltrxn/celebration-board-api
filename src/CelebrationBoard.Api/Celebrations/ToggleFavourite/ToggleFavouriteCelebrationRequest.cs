namespace CelebrationBoard.Api.Celebrations.ToggleFavourite;
public sealed record ToggleFavouriteCelebrationRequest([FromQuery] long CelebrationId, long UserId);