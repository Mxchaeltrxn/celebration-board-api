namespace CelebrationBoard.UnitTests;

using System.Text;
using CelebrationBoard.Domain.Celebrations;
using CelebrationBoard.Domain.Common;
using FluentAssertions;
using Xunit;

public class ContentTests
{
  [Fact]
  public void Empty_content_returns_error()
  {
    var content = Content.Create("");
    content.Error.Should().Be(Errors.General.ValueIsRequired());
  }

  [Fact]
  public void Content_longer_than_10k_characters_returns_error()
  {
    var content = Content.Create(GenerateRandomString(wordLength: 10_001));
    content.Error.Should().Be(Errors.General.InvalidLength());
  }

  [Fact]
  public void Content_with_no_tags()
  {
    var content = Content.Create(GenerateParagraph(new[] { "#" }) + Environment.NewLine + GenerateParagraph(new[] { "#" })).Value;

    var tags = content.CalculateTags();
    tags.Should().BeEquivalentTo(Array.Empty<string>());
  }

  [Fact]
  public void Content_with_a_single_word_tag_in_middle()
  {
    var tagValue = GenerateTagValue();
    var content = Content.Create(GenerateParagraph(new[] { "#" }) + $" #{tagValue} " + GenerateParagraph(new[] { "#" })).Value;

    var tags = content.CalculateTags();
    tags.Should().BeEquivalentTo(new string[] { tagValue });
  }

  [Fact]
  public void Content_with_a_single_word_tag_at_start()
  {
    var tagValue = GenerateTagValue();
    var content = Content.Create($"#{tagValue} " + GenerateParagraph(new[] { "#" })).Value;

    var tags = content.CalculateTags();
    tags.Should().BeEquivalentTo(new string[] { tagValue });
  }

  [Fact]
  public void Content_with_a_single_word_tag_at_end()
  {
    var tagValue = GenerateTagValue();
    var content = Content.Create(GenerateParagraph(new[] { "#" }) + $" #{tagValue}").Value;

    var tags = content.CalculateTags();
    tags.Should().BeEquivalentTo(new string[] { tagValue });
  }

  [Fact]
  public void Content_with_two_single_words()
  {
    var tagValue1 = GenerateTagValue();
    var tagValue2 = GenerateTagValue();
    var content = Content.Create($"#{tagValue1} #{tagValue2}").Value;

    var tags = content.CalculateTags();
    tags.Should().BeEquivalentTo(new string[] { tagValue1, tagValue2 });
  }

  [Fact]
  public void Content_with_duplicate_single_word_tags_is_deduplicated()
  {
    var tagValue = GenerateTagValue();
    var content = Content.Create($"#{tagValue} #{tagValue}").Value;

    var tags = content.CalculateTags();
    tags.Should().BeEquivalentTo(new string[] { tagValue });
  }

  [Fact]
  public void Content_with_one_multi_word_tag_at_start_returns_tag()
  {
    var tagValue = GenerateTagValue(true);
    var content = Content.Create($" #[[{tagValue}]]" + GenerateParagraph(new[] { "#" })).Value;

    var tags = content.CalculateTags();
    tags.Should().BeEquivalentTo(new string[] { tagValue });
  }

  [Fact]
  public void Content_with_one_multi_word_tag_at_the_middle_returns_tag()
  {
    var tagValue = GenerateTagValue(true);
    var content = Content.Create(GenerateParagraph(new[] { "#" }) + $" #[[{tagValue}]] random text").Value;

    var tags = content.CalculateTags();
    tags.Should().BeEquivalentTo(new string[] { tagValue });
  }

  [Fact]
  public void Content_with_one_multi_word_tag_at_end_returns_tag()
  {
    var tagValue = GenerateTagValue(true);
    var content = Content.Create(GenerateParagraph(new[] { "#" }) + $" #[[{tagValue}]]").Value;

    var tags = content.CalculateTags();
    tags.Should().BeEquivalentTo(new string[] { tagValue });
  }

  [Fact]
  public void Content_with_two_multi_word_tags_in_middle_returns_two_tags()
  {
    var tagValue1 = GenerateTagValue(true);
    var tagValue2 = GenerateTagValue(true);
    var content = Content.Create($"#[[{tagValue1}]] #[[{tagValue2}]]").Value;

    var tags = content.CalculateTags();
    tags.Should().BeEquivalentTo(new string[] { tagValue1, tagValue2 });
  }

  [Fact]
  public void Content_with_duplicate_multi_word_tags_deduplicates_tags()
  {
    var tagValue = GenerateTagValue(true);
    var content = Content.Create($"#[[{tagValue}]] #[[{tagValue}]]").Value;

    var tags = content.CalculateTags();
    tags.Should().BeEquivalentTo(new string[] { tagValue });
  }

  [Fact]
  public void Content_with_single_and_multi_word_tags_returns_two_tags()
  {
    var singleWordTag = GenerateTagValue(false);
    var multiWordTag = GenerateTagValue(true);
    var content = Content.Create($"#{singleWordTag} #[[{multiWordTag}]]").Value;

    var tags = content.CalculateTags();
    tags.Should().BeEquivalentTo(new string[] { singleWordTag, multiWordTag });
  }

  [Fact]
  public void Content_with_two_tags_on_different_lines_returns_two_lines()
  {
    var singleWordTag = GenerateTagValue(false);
    var multiWordTag = GenerateTagValue(true);
    var content = Content.Create($"#{singleWordTag}{Environment.NewLine}#[[{multiWordTag}]]").Value;

    var tags = content.CalculateTags();
    tags.Should().BeEquivalentTo(new string[] { singleWordTag, multiWordTag });
  }

  [Fact]
  public void Content_with_string_of_hashes_returns_no_tags()
  {
    var content = Content.Create($"####").Value;

    var tags = content.CalculateTags();
    tags.Should().BeEquivalentTo(Array.Empty<string>());
  }

  [Fact]
  public void Content_with_hash_followed_by_numers_returns_no_tags()
  {
    var content = Content.Create($"#12314").Value;

    var tags = content.CalculateTags();
    tags.Should().BeEquivalentTo(Array.Empty<string>());
  }

  private static string GenerateRandomString(string[]? ignoredChars = null, int wordLength = 8)
  {
    var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+-=[]{}\\|;:'\"<,>./?";
    if (ignoredChars is not null)
    {
      for (int i = 0; i < ignoredChars.Length; i++)
      {
        chars = chars.Replace(ignoredChars[i], "");
      }
    }
    var stringChars = new char[wordLength];
    var random = new Random();

    for (int i = 0; i < stringChars.Length; i++)
    {
      stringChars[i] = chars[random.Next(chars.Length)];
    }

    return new string(stringChars);
  }

  private static string GenerateParagraph(string[]? ignoredChars = null, int wordCount = 15)
  {
    var sb = new StringBuilder();
    for (int i = 0; i < wordCount; i++)
    {
      sb.Append(GenerateRandomString(ignoredChars));
      sb.Append(' ');
    }

    return sb.ToString();
  }

  private static string GenerateTagValue(bool withSpace = false)
  {
    var _endHashTagValues = new[] {
  "!","@","#","$","%","^","&","*","(",")","_","+","-","=","[","]","{","}","\\","|",";",":","'", "\"","<",",",">",".","/","?"
};
    // Hash tag must start with an alphabetic character
    var tagValue = $"a{GenerateRandomString(_endHashTagValues)}";

    return withSpace ? $"{tagValue} {GenerateRandomString(_endHashTagValues)}" : tagValue;
  }
}
