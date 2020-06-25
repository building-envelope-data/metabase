using CSharpFunctionalExtensions;

namespace Icon
{
    public interface IValidatable
    {
        public Result<bool, Errors> Validate();

        public bool IsValid();

        public void EnsureValid();
    }
}