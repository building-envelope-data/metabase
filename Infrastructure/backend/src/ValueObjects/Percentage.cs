using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Infrastructure.ValueObjects
{
    public sealed class Percentage
      : ValueObject, IComparable
    {
        public double Value { get; }

        private Percentage(double value)
        {
            Value = value;
        }

        public static Result<Percentage, Errors> From(
            double percentage,
            IReadOnlyList<object>? path = null
            )
        {
            // https://docs.microsoft.com/en-us/dotnet/api/system.double.nan?view=netcore-3.1
            if (double.IsNaN(percentage))
            {
                return Result.Failure<Percentage, Errors>(
                    Errors.One(
                    message: $"The percentage {percentage} is not a number",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );
            }
            if (percentage < 0 || percentage > 1)
            {
                return Result.Failure<Percentage, Errors>(
                    Errors.One(
                    message: $"The percentage {percentage} is not between 0 and 1 inclusive",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                    );
            }
            return Result.Success<Percentage, Errors>(
                new Percentage(percentage)
                );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public int CompareTo(object? obj)
        {
            // The behaviour in edge cases as implemented here is described on
            // https://docs.microsoft.com/en-us/dotnet/api/system.icomparable.compareto?redirectedfrom=MSDN&view=netcore-3.1#remarks
            if (obj is null)
            {
                return 1;
            }
            if (obj is Percentage percentage)
            {
                return Value.CompareTo(percentage);
            }
            throw new ArgumentException($"Object {obj} is not of type {typeof(Percentage)}");
        }

        public static explicit operator Percentage(double percentage)
        {
            return From(percentage).Value;
        }

        public static implicit operator double(Percentage percentage)
        {
            return percentage.Value;
        }
    }
}