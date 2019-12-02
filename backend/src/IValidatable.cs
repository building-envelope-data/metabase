using CSharpFunctionalExtensions;
using IError = HotChocolate.IError;
using Errors = Icon.Errors;

namespace Icon
{
    public interface IValidatable
    {
        public Result<bool, Errors> Validate();

        public bool IsValid();

        public void EnsureValid();
    }
}