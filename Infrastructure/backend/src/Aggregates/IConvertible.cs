using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Errors = Infrastructure.Errors;

namespace Infrastructure.Aggregates
{
    // TODO Find a better name and place.
    // TODO Make `M` contravariant if possible https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/in-generic-modifier
    public interface IConvertible<M>
    {
        public Result<M, Errors> ToModel();
    }
}