namespace CelebrationBoard.Api.Celebrations.Post;

public class PostCelebrationRequestValidator : AbstractValidator<PostCelebrationRequest>
{
  public PostCelebrationRequestValidator()
  {
    RuleFor(x => x.UserId).NotNull();
    RuleFor(x => x.Title)
      .MustBeValueObject(Title.Create);
    RuleFor(x => x.Content)
      .MustBeValueObject(Content.Create);
    RuleFor(x => x.PrivacyLevel)
      .MustBeValueObject(PrivacyLevel.Create);
  }
}
