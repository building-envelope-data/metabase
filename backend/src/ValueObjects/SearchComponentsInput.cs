using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Uri = System.Uri;
using ValueObjects = Icon.ValueObjects;

namespace Icon.ValueObjects
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
            return Result.Ok<SearchComponentsInput, Errors>(
                new SearchComponentsInput()
                );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return null;
        }
    }
}