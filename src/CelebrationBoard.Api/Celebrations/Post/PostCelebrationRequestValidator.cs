namespace CelebrationBoard.Api.Celebrations.Post;

public class ToggleFavouriteCelebrationRequestValidator : AbstractValidator<PostCelebrationRequest>
{
  public ToggleFavouriteCelebrationRequestValidator()
  {
    RuleFor(x => x.UserId).NotNull().GreaterThan(0);
    RuleFor(x => x.Title)
      .MustBeValueObject(Title.Create);
    RuleFor(x => x.Content)
      .MustBeValueObject(Content.Create);
    RuleFor(x => x.PrivacyLevel)
      .MustBeValueObject(PrivacyLevel.Create);
  }
}
