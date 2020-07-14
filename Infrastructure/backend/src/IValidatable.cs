using CSharpFunctionalExtensions;
using Errors = Infrastructure.Errors;
using IValidatable = Infrastructure.IValidatable;

namespace Infrastructure
{
    public interface IValidatable
    {
        public Result<bool, Errors> Validate();

        public bool IsValid();

        public void EnsureValid();
    }
}