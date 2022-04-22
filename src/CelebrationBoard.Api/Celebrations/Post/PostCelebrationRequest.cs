namespace CelebrationBoard.Api.Celebrations.Post;
public sealed record PostCelebrationRequest(long UserId, string Title, string Content, string PrivacyLevel);