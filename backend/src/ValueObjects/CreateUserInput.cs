using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Array = System.Array;
using DateTime = System.DateTime;
using Guid = System.Guid;
using Models = Icon.Models;

namespace Icon.ValueObjects
{
    public sealed class CreateUserInput
      : ValueObject
    {
        private CreateUserInput()
        {
        }

        public static Result<CreateUserInput, Errors> From(
            IReadOnlyList<object>? path = null
            )
        {
            return Result.Ok<CreateUserInput, Errors>(
                new CreateUserInput()
                );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return null;
        }
    }
}