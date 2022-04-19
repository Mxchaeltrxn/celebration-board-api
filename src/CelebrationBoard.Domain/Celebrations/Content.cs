namespace CelebrationBoard.Domain.Celebrations;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

public sealed class Content : ValueObject
{
  public string Value { get; }

  private Content(string value)
  {
    this.Value = value;
  }

  public static Result<Content, Error> Create(string input)
  {
    if (string.IsNullOrEmpty(input))
      return Errors.General.ValueIsRequired();

    var content = input.Trim();

    if (content.Length > 10_000)
      return Errors.General.InvalidLength();

    return new Content(content);
  }

  public static explicit operator Content(string content)
  {
    return Create(content).Value;
  }

  public static implicit operator string(Content content)
  {
    return content.Value;
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value;
  }

  public TagName[] CalculateTags()
  {
    // '#tag' => 'tag'
    var singleWordHashTagMatches = new Regex(@"(?<=#)(\w*[A-Za-z_]+\w*)")
                              .Matches(this.Value);

    // '#[[text that is a tag]]' => 'text that is a tag'
    var multiWordHashTagMatches = new Regex(@"(?<=#\[\[)(\w*[A-Za-z_ ]+\w*)(?=\]\])")
                              .Matches(this.Value);

    return singleWordHashTagMatches.Concat(multiWordHashTagMatches)
      .Select(match => match.Value.Trim())
      .Where(t => !string.IsNullOrEmpty(t) && t.Length <= 100)
      .Select(t => TagName.Create(t).Value)
      .ToArray();
  }
}


