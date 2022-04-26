namespace CelebrationBoard.Api.Celebrations.ToggleFavourite;

public class ToggleFavouriteCelebrationRequestValidator : AbstractValidator<ToggleFavouriteCelebrationRequest>
{
  public ToggleFavouriteCelebrationRequestValidator()
  {
    RuleFor(x => x.UserId).NotNull().GreaterThan(0);
    RuleFor(x => x.CelebrationId).NotNull().GreaterThan(0);
  }
}
