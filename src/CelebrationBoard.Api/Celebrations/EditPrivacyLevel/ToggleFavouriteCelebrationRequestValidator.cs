namespace CelebrationBoard.Api.Celebrations.EditPrivacyLevel;

public class EditCelebrationPrivacyLevelRequestValidator : AbstractValidator<EditCelebrationPrivacyLevelRequest>
{
  public EditCelebrationPrivacyLevelRequestValidator()
  {
    RuleFor(x => x.UserId).NotNull().GreaterThan(0);
    RuleFor(x => x.CelebrationId).NotNull().GreaterThan(0);
    RuleFor(x => x.PrivacyLevel)
      .MustBeValueObject(PrivacyLevel.Create);
  }
}
