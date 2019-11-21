// Inspired by https://enterprisecraftsmanship.com/posts/value-object-better-implementation/
// It's an improvement over https://github.com/vkhorikov/CSharpFunctionalExtensions/blob/master/CSharpFunctionalExtensions/ValueObject/ValueObject.cs
// For the distinction between value objects and entities (what we call models) see https://enterprisecraftsmanship.com/posts/entity-vs-value-object-the-ultimate-list-of-differences/

using System.Collections.Generic;
using System.Linq;

namespace Icon.ValueObjects
{
  public abstract class ValueObject
  {
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
      if (obj is null)
        return false;

      if (GetType() != obj.GetType())
        return false;

      var valueObject = (ValueObject)obj;

      return
        GetEqualityComponents()
        .SequenceEqual(
            valueObject.GetEqualityComponents()
            );
    }

    public override int GetHashCode()
    {
      return GetEqualityComponents()
        .Aggregate(1, (current, obj) =>
            {
            unchecked
            {
            return current * 23 + (obj?.GetHashCode() ?? 0);
            }
            });
    }

    public static bool operator ==(ValueObject a, ValueObject b)
    {
      if (a is null && b is null)
        return true;

      if (a is null || b is null)
        return false;

      return a.Equals(b);
    }

    public static bool operator !=(ValueObject a, ValueObject b)
    {
      return !(a == b);
    }
  }
}
