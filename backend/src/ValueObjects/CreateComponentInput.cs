using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Array = System.Array;
using DateTime = System.DateTime;
using Guid = System.Guid;
using Models = Icon.Models;

namespace Icon.ValueObjects
{
    public sealed class CreateComponentInput
      : ValueObject
    {
        public ComponentInformation Information { get; }

        private CreateComponentInput(
            ComponentInformation information
            )
        {
            Information = information;
        }

        public static Result<CreateComponentInput, Errors> From(
            ComponentInformation information,
            IReadOnlyList<object>? path = null
            )
        {
            return Result.Ok<CreateComponentInput, Errors>(
                new CreateComponentInput(
                  information: information
                  )
                );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Information;
        }
    }
}