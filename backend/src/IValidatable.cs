using System;

namespace Icon
{
    public interface IValidatable
    {
        public bool IsValid();

        public void EnsureValid()
        {
            if (!IsValid())
            {
                throw new InvalidOperationException($"The instance {this} is invalid.");
            }
        }
    }
}