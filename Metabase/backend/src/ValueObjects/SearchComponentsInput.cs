using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Errors = Infrastructure.Errors;

namespace Metabase.ValueObjects
{
    public sealed class SearchComponentsInput
      : ValueObject
    {
        private SearchComponentsInput()
        {
        }

        public static Result<SearchComponentsInput, Errors> From(
            IReadOnlyList<object>? path = null
            )
        {
            return Result.Success<SearchComponentsInput, Errors>(
                new SearchComponentsInput()
                );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return null;
        }
    }
}