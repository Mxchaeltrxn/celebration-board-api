namespace CelebrationBoard.Domain.Celebrations;

using CelebrationBoard.Domain.Common;

public sealed class PrivacyLevel : ValueObject
{
  public static PrivacyLevel PRIVATE = new PrivacyLevel(nameof(PRIVATE));
  public static PrivacyLevel PERSONAL = new PrivacyLevel(nameof(PERSONAL));
  private static readonly PrivacyLevel[] _all = { PRIVATE, PERSONAL };

  public string Value { get; }

  private PrivacyLevel(string value)
  {
    this.Value = value;
  }

  public static Result<PrivacyLevel, Error> Create(string input)
  {
    if (string.IsNullOrWhiteSpace(input))
      return Errors.General.ValueIsRequired();

    var privacyLevel = input.Trim().ToUpper();

    if (!_all.Any(x => x.Value == privacyLevel))
      return Errors.General.ValueIsInvalid();

    return new PrivacyLevel(privacyLevel);
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value;
  }
}