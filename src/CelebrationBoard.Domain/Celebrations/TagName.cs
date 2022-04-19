namespace CelebrationBoard.Domain.Celebrations;

public sealed class TagName : ValueObject
{
  public string Value { get; }

  private TagName(string value)
  {
    this.Value = value;
  }

  public static Result<TagName, Error> Create(string input)
  {
    if (string.IsNullOrEmpty(input))
      return Errors.General.ValueIsRequired();

    var name = input.Trim();

    if (name.Length > 100)
      return Errors.General.InvalidLength();

    return new TagName(name);
  }

  public static explicit operator TagName(string name)
  {
    return Create(name).Value;
  }

  public static implicit operator string(TagName name)
  {
    return name.Value;
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value;
  }
}


