// namespace CelebrationBoard.Domain.Celebrations;

// using System.Collections.Generic;
// using CelebrationBoard.Domain.Common;

// public class User : BaseEntity
// {
//   public string Username { get; set; }
//   public string EmailAddress { get; set; }
//   public string Password { get; set; }
//   public IReadOnlyCollection<Celebration> FavouritedCelebrations => (IReadOnlyCollection<Celebration>)this._favouritedCelebrations;
//   private readonly ICollection<Celebration> _favouritedCelebrations;

//   public IReadOnlyCollection<Celebration> DittoedCelebrations => (IReadOnlyCollection<Celebration>)this._dittoedCelebrations;
//   private readonly ICollection<Celebration> _dittoedCelebrations;

//   public IReadOnlyCollection<Celebration> Celebrations => (IReadOnlyCollection<Celebration>)this._celebrations;
//   private readonly ICollection<Celebration> _celebrations;

//   public void MakeCelebration(Celebration celebration)
//   {
//     this._celebrations.Add(celebration);
//   }

//   public void EditCelebration(Celebration celebration)
//   {
//     // this._celebrations.Add(celebration);
//   }

//   public void EditCelebrationPrivacyLevel(Celebration celebration)
//   {
//     // Should only be able to take published ones
//   }

//   public void ToggleCelebrationFavourite()
//   {

//   }

//   public void ToggleCelebrationDitto()
//   {

//   }
// }


