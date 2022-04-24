namespace CelebrationBoard.Domain.Celebrations;

using System.Collections.Generic;
using CelebrationBoard.Domain.Common;

public class User : BaseEntity
{
  public Guid UserId { get; set; }
  public IReadOnlyCollection<Celebration> FavouritedCelebrations => (IReadOnlyCollection<Celebration>)this._favouritedCelebrations;
  private readonly ICollection<Celebration> _favouritedCelebrations;

  // public IReadOnlyCollection<DittoUserCelebration> DittoedCelebrations => (IReadOnlyCollection<DittoUserCelebration>)this._dittoedCelebrations;
  // private readonly ICollection<DittoUserCelebration> _dittoedCelebrations;

  public IReadOnlyCollection<Celebration> Celebrations => (IReadOnlyCollection<Celebration>)this._celebrations;
  private readonly ICollection<Celebration> _celebrations;

  public User(Guid userId)
  {
    UserId = userId;
  }

  public void PostCelebration(Celebration celebration)
  {
    this._celebrations.Add(celebration);
  }

  public void DeleteCelebration(Celebration celebration)
  {
    this._celebrations.Remove(celebration);
  }

  public void ToggleCelebrationFavourite(Celebration celebration)
  {
    if (this._favouritedCelebrations.Contains(celebration))
    {
      celebration.RemoveFavouritedUser(this);
      this._celebrations.Remove(celebration);
    }
    else
    {
      celebration.AddFavouritedUser(this);
      this._celebrations.Add(celebration);
    }
  }

  // public void ToggleCelebrationDitto(Celebration celebration)
  // {
  //   if (this._dittoedCelebrations.Contains(celebration))
  //   {
  //     celebration.RemoveDittoedUser(this);
  //     this._celebrations.Remove(celebration);
  //   }
  //   else
  //   {
  //     celebration.AddDittoedUser(this);
  //     this._celebrations.Add(celebration);
  //   }
  // }
}

public class UserCelebration
{
  public long UserId { get; set; }
  public User? User { get; set; }
  public Celebration? Celebration { get; set; }
  public long CelebrationId { get; set; }
}

public sealed class FavouritedUserCelebration : UserCelebration
{
  public FavouritedUserCelebration(User user, Celebration celebration)
  {
    base.UserId = user.Id;
    base.User = user;
    base.Celebration = celebration;
    base.CelebrationId = celebration.Id;
  }
}

public sealed class DittoUserCelebration : UserCelebration
{
  // public DittoUserCelebration(User user, Celebration celebration) : base(user, celebration)
  // {
  // }
}