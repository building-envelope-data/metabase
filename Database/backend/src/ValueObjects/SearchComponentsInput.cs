using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Database.ValueObjects
{
    public sealed class SearchComponentsInput
      : ValueObject
    {
        public Proposition<SearchComponentsVariable> Where { get; }

        private SearchComponentsInput(
            Proposition<SearchComponentsVariable> where
            )
        {
            Where = where;
        }

        public static Result<SearchComponentsInput, Errors> From(
            Proposition<SearchComponentsVariable> where,
            IReadOnlyList<object>? path = null
            )
        {
            return Result.Success<SearchComponentsInput, Errors>(
                new SearchComponentsInput(where)
                );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Where;
        }
    }
}