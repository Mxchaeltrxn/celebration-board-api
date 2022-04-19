namespace CelebrationBoard.Domain.Celebrations;

public sealed class Title : ValueObject
{
  public string Value { get; }

  private Title(string value)
  {
    this.Value = value;
  }

  public static Result<Title, Error> Create(string input)
  {
    if (input.Length > 200)
      return Errors.General.InvalidLength();

    var title = input.Trim();

    return new Title(title);
  }

  public static explicit operator Title(string title)
  {
    return Create(title).Value;
  }

  public static implicit operator string(Title title)
  {
    return title.Value;
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value;
  }
}


