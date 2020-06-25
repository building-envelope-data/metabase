using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.ValueObjects
{
    public sealed class Section
      : ValueObject
    {
        public string Value { get; }

        private Section(string value)
        {
            Value = value;
        }

        public static Result<Section, Errors> From(
            string section,
            IReadOnlyList<object>? path = null
            )
        {
            section = section.Trim();

            if (section.Length == 0)
                return Result.Failure<Section, Errors>(
                    Errors.One(
                    message: "Section is empty",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );

            if (section.Length > 128)
                return Result.Failure<Section, Errors>(
                    Errors.One(
                    message: "Section is too long",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );

            return Result.Ok<Section, Errors>(new Section(section));
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public static explicit operator Section(string section)
        {
            return From(section).Value;
        }

        public static implicit operator string(Section section)
        {
            return section.Value;
        }
    }
}