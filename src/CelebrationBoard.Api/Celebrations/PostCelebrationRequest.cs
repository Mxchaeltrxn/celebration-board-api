using FluentValidation;

namespace CelebrationBoard.Api.Celebrations;
public sealed record PostCelebrationRequest(string Title, string Content, string PrivacyLevel);

public class PostCelebrationRequestValidator : AbstractValidator<PostCelebrationRequest>
{
  public PostCelebrationRequestValidator()
  {
    RuleFor(x => x.Title).MustBeValueObject(Title.Create);
    RuleFor(x => x.Content).MustBeValueObject(Content.Create);
    RuleFor(x => x.PrivacyLevel).MustBeValueObject(PrivacyLevel.Create);
  }
}
