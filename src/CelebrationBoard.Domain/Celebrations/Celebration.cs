namespace CelebrationBoard.Domain.Celebrations;

using System.Text.RegularExpressions;
using CelebrationBoard.Domain.Common;

public sealed class Celebration : BaseEntity
{
  public Title Title { get; private set; }
  public Content Content { get; private set; }
  // public bool IsPosted { get; private set; }
  public PrivacyLevel AccessLevel { get; private set; }
  public IReadOnlyCollection<CelebrationTag> Tags => (IReadOnlyCollection<CelebrationTag>)_tags;
  private readonly ICollection<CelebrationTag> _tags;

  // public IReadOnlyCollection<User> FavouritedUsers => (IReadOnlyCollection<User>)_favouritedUsers;
  // private readonly ICollection<User> _favouritedUsers;

  // public IReadOnlyCollection<User> DittoedUsers => (IReadOnlyCollection<User>)_dittoedUsers;
  // private readonly ICollection<User> _dittoedUsers;

  private Celebration()
  {
    this._tags = new List<CelebrationTag>();
    // this._dittoedUsers = new List<User>();
    // this._favouritedUsers = new List<User>();
    this.AccessLevel = PrivacyLevel.Private;
  }

  public Celebration(Title title, Content content, PrivacyLevel accessLevel) : this()
  {
    this.Title = title;
    this.Content = content;
    this.AccessLevel = accessLevel;
  }

  public void AddTags(ICollection<CelebrationTag> tags)
  {
    foreach (var tag in tags)
    {
      if (!this._tags.Contains(tag))
      {
        this._tags.Add(tag);
      }
    }
  }

  // public Celebration(CelebrationTitle title, CelebrationContent content, PrivacyControl privacyLevel) : this() // TODO: Investigate creating one class per constructor. A DraftCelebration vs a PostedCelebration
  // {
  //   this.Title = title;
  //   this.Content = content;
  //   this.IsPosted = true;
  //   this.PrivacyLevel = privacyLevel.ToString();
  // this._tags = (ICollection<CelebrationTag>)CalculateTags(content)
  //   .Select(tag => new CelebrationTag(tag));
  // }

  // public Celebration(CelebrationTitle title, CelebrationContent content) : this()
  // {
  //   this.Title = title;
  //   this.Content = content;
  //   this.IsPosted = false;
  //   this._tags = (ICollection<CelebrationTag>)CalculateTags(content)
  //     .Select(tag => new CelebrationTag(tag));
  // }


}


