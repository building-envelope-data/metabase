using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Infrastructure.ValueObjects
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
            {
                return Result.Failure<Id, Errors>(
                    Errors.One(
                    message: "Id is empty",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );
            }
            return Result.Success<Id, Errors>(new Id(id));
        }

        public static Result<Id, Errors>? MaybeFrom(
            Guid? id,
            IReadOnlyList<object>? path = null
            )
        {
            if (id is null)
                return null;

            // Why can't we use the null-forgiving operator `!` as follows?
            // return From(id: id!, path: path);
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-forgiving
            return From(id: id ?? throw new ArgumentNullException(nameof(id)), path: path);
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

        public override string ToString()
        {
            return $"{GetType()}({Value})";
        }
    }
}