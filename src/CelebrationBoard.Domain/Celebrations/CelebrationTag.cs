namespace CelebrationBoard.Domain.Celebrations;

using System;
using System.Collections.Generic;
using CelebrationBoard.Domain.Common;

public class CelebrationTag : BaseEntity
{
  public TagName Name { get; private set; } = default!;
  public IReadOnlyCollection<Celebration> Celebrations => (IReadOnlyCollection<Celebration>)_celebrations;
  private readonly ICollection<Celebration> _celebrations;

  public CelebrationTag(TagName value)
  {
    this.Name = value;
    this._celebrations = new List<Celebration>();
  }

  public void AddCelebration(Celebration celebration)
  {
    if (!this._celebrations.Contains(celebration))
    {
      this._celebrations.Add(celebration);
    }
  }
}
