using System;
using Array = System.Array;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using IError = HotChocolate.IError;
using ErrorCodes = Icon.ErrorCodes;

namespace Icon.ValueObjects
{
    public sealed class Id
      : ValueObject
    {
        public Guid Value { get; }

        private Id(Guid value)
        {
            Value = value;
        }

        public static Id New()
        {
            // Note that `Guid.NewGuid()` is guaranteed to not equal `Guid.Empty`, see
            // https://docs.microsoft.com/en-us/dotnet/api/system.guid.newguid?view=netframework-4.8#remarks
            // Therefore, using `Value` is safe here.
            return From(
                Guid.NewGuid()
                ).Value;
        }

        public static Result<Id, Errors> From(
            Guid id,
            IReadOnlyList<object>? path = null
            )
        {
            if (id == Guid.Empty)
                return Result.Failure<Id, Errors>(
                    Errors.One(
                    message: "Id is empty",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );

            return Result.Ok<Id, Errors>(new Id(id));
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public static explicit operator Id(Guid id)
        {
            return From(id).Value;
        }

        public static implicit operator Guid(Id id)
        {
            return id.Value;
        }
    }
}