using System;
using CSharpFunctionalExtensions;
using IValidatable = Infrastructure.IValidatable;
using Validatable = Infrastructure.Validatable;

namespace Infrastructure
{
    public abstract class Validatable : IValidatable
    {
        public static Result<bool, Errors> ValidateNull(
            object? @object,
            string variableName
            )
        {
            if (!(@object is null))
            {
                return Result.Failure<bool, Errors>(
                    Errors.One(
                      message: $"{variableName} is non-null but has the value {@object}",
                      code: ErrorCodes.InvalidValue
                      )
                    );
            }
            return Result.Success<bool, Errors>(true);
        }

        public static Result<bool, Errors> ValidateNonNull(
            object? @object,
            string variableName
            )
        {
            if (@object is null)
            {
                return Result.Failure<bool, Errors>(
                    Errors.One(
                      message: $"{variableName} is null",
                      code: ErrorCodes.InvalidValue
                      )
                    );
            }
            return Result.Success<bool, Errors>(true);
        }

        public static Result<bool, Errors> ValidateEmpty(
            Guid id,
            string variableName
            )
        {
            if (id != Guid.Empty)
            {
                return Result.Failure<bool, Errors>(
                    Errors.One(
                      message: $"{variableName} is non-empty but but has the value {id}",
                      code: ErrorCodes.InvalidValue
                      )
                    );
            }
            return Result.Success<bool, Errors>(true);
        }

        public static Result<bool, Errors> ValidateNonEmpty(
            Guid id,
            string variableName
            )
        {
            if (id == Guid.Empty)
            {
                return Result.Failure<bool, Errors>(
                    Errors.One(
                      message: $"{variableName} is empty",
                      code: ErrorCodes.InvalidValue
                      )
                    );
            }
            return Result.Success<bool, Errors>(true);
        }

        public static Result<bool, Errors> ValidateNullOrNonEmpty(
            Guid? id,
            string variableName
            )
        {
            if (id is null)
            {
                return Result.Success<bool, Errors>(true);
            }
            return ValidateNonEmpty(id.Value, variableName);
        }

        public static Result<bool, Errors> ValidateMinValue(
            DateTime dateTime,
            string variableName
            )
        {
            if (dateTime != DateTime.MinValue)
            {
                return Result.Failure<bool, Errors>(
                    Errors.One(
                      message: $"{variableName} does not have the default value but the value {dateTime}",
                      code: ErrorCodes.InvalidValue
                      )
                    );
            }
            return Result.Success<bool, Errors>(true);
        }

        public static Result<bool, Errors> ValidateNotMinValue(
            DateTime dateTime,
            string variableName
            )
        {
            if (dateTime == DateTime.MinValue)
            {
                return Result.Failure<bool, Errors>(
                    Errors.One(
                      message: $"{variableName} has the default value {DateTime.MinValue}",
                      code: ErrorCodes.InvalidValue
                      )
                    );
            }
            return Result.Success<bool, Errors>(true);
        }

        public static Result<bool, Errors> ValidateZero(
            int number,
            string variableName
            )
        {
            if (number != 0)
            {
                return Result.Failure<bool, Errors>(
                    Errors.One(
                      message: $"{variableName} is not 0 but the number {number}",
                      code: ErrorCodes.InvalidValue
                      )
                    );
            }
            return Result.Success<bool, Errors>(true);
        }

        public static Result<bool, Errors> ValidateNonZero(
            int number,
            string variableName
            )
        {
            if (number == 0)
            {
                return Result.Failure<bool, Errors>(
                    Errors.One(
                      message: $"{variableName} is 0",
                      code: ErrorCodes.InvalidValue
                      )
                    );
            }
            return Result.Success<bool, Errors>(true);
        }

        public static Result<bool, Errors> ValidateEquality<T>(
            T @object,
            T expected,
            string variableName
            )
        {
            // What `==` does is explained under
            // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/equality-comparisons
            // and
            // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/how-to-define-value-equality-for-a-type
            if (!object.Equals(@object, expected))
            {
                return Result.Failure<bool, Errors>(
                    Errors.One(
                      message: $"{variableName} does not have the expected value {expected} but the value {@object}",
                      code: ErrorCodes.InvalidValue
                      )
                    );
            }
            return Result.Success<bool, Errors>(true);
        }

        public abstract Result<bool, Errors> Validate();

        public bool IsValid()
        {
            return Validate().IsFailure;
        }

        public void EnsureValid()
        {
            var result = Validate();
            if (result.IsFailure)
            {
                throw new InvalidOperationException(
                    $"The instance {this} is invalid and the errors are {result.Error}"
                    );
            }
        }
    }
}