namespace CelebrationBoard.Api.Celebrations.EditPrivacyLevel;
public sealed record EditCelebrationPrivacyLevelRequest(long UserId, [FromQuery] long CelebrationId, string PrivacyLevel);