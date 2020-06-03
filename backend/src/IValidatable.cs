using CSharpFunctionalExtensions;
using Errors = Icon.Errors;
using IError = HotChocolate.IError;

namespace Icon
{
    public interface IValidatable
    {
        public Result<bool, Errors> Validate();

        public bool IsValid();

        public void EnsureValid();
    }
}