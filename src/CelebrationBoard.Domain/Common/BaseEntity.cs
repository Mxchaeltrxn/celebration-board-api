namespace CelebrationBoard.Domain.Common;

// https://enterprisecraftsmanship.com/posts/entity-base-class/
// TODO: Remove any references to Nhibernate
public abstract class BaseEntity
{
  public virtual long Id { get; protected set; }

  // Required by EF Core to create the entity
  protected BaseEntity()
  {
  }

  protected BaseEntity(long id)
  {
    Id = id;
  }

  public override bool Equals(object? obj)
  {
    if (obj is not BaseEntity other)
      return false;

    if (ReferenceEquals(this, other))
      return true;

    if (GetUnproxiedType(this) != GetUnproxiedType(other))
      return false;

    if (Id.Equals(default) || other.Id.Equals(default))
      return false;

    return Id.Equals(other.Id);
  }

  public static bool operator ==(BaseEntity a, BaseEntity b)
  {
    if (a is null && b is null)
      return true;

    if (a is null || b is null)
      return false;

    return a.Equals(b);
  }

  public static bool operator !=(BaseEntity a, BaseEntity b)
  {
    return !(a == b);
  }

  public override int GetHashCode()
  {
    return (GetUnproxiedType(this).ToString() + Id).GetHashCode();
  }

  internal static Type GetUnproxiedType(object obj)
  {
    const string EFCoreProxyPrefix = "Castle.Proxies.";

    var type = obj.GetType();
    var typeString = type.ToString();

    if (typeString.Contains(EFCoreProxyPrefix)) return type.BaseType;

    return type;
  }
}
