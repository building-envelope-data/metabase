using System;

namespace Icon
{
    public abstract class Validatable : IValidatable
    {
      public abstract bool IsValid();

      public void EnsureValid()
      {
        if (!IsValid())
        {
          throw new InvalidOperationException($"The instance {this} is invalid.");
        }
      }
    }
}
