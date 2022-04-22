namespace CelebrationBoard.Domain.Celebrations;

using System.Collections.Generic;
using CelebrationBoard.Domain.Common;

public class Tag : BaseEntity
{
  public string Name { get; private set; } = default!;
  public IReadOnlyCollection<Celebration> Celebrations => (IReadOnlyCollection<Celebration>)_celebrations;
  private readonly ICollection<Celebration> _celebrations;

  protected Tag()
  { }

  public Tag(string value)
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
