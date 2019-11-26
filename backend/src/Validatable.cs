using System;
using Errors = Icon.Errors;
using ErrorCodes = Icon.ErrorCodes;
using CSharpFunctionalExtensions;
using IError = HotChocolate.IError;

namespace Icon
{
    public abstract class Validatable : IValidatable
    {
        public static Result<bool, Errors> ValidateNull(
            object @object,
            string variableName
            )
        {
            if (!(@object is null))
              return Result.Failure<bool, Errors>(
                  Errors.One(
                    message: $"{variableName} is non-null",
                    code: ErrorCodes.InvalidValue
                    )
                  );

            return Result.Ok<bool, Errors>(@object);
        }

        public static Result<bool, Errors> ValidateNonNull(
            object @object,
            string variableName
            )
        {
            if (@object is null)
              return Result.Failure<bool, Errors>(
                  Errors.One(
                    message: $"{variableName} is null",
                    code: ErrorCodes.InvalidValue
                    )
                  );

            return Result.Ok<bool, Errors>(@object);
        }

        public static Result<bool, Errors> ValidateEmpty(
            Guid id,
            string variableName
            )
        {
            if (id != Guid.Empty)
              return Result.Failure<bool, Errors>(
                  Errors.One(
                    message: $"{variableName} is non-empty",
                    code: ErrorCodes.InvalidValue
                    )
                  );

            return Result.Ok<bool, Errors>(id);
        }

        public static Result<bool, Errors> ValidateNonEmpty(
            Guid id,
            string variableName
            )
        {
            if (id == Guid.Empty)
              return Result.Failure<bool, Errors>(
                  Errors.One(
                    message: $"{variableName} is empty",
                    code: ErrorCodes.InvalidValue
                    )
                  );

            return Result.Ok<bool, Errors>(id);
        }

        public static Result<bool, Errors> ValidateNotMinValue(
            DateTime dateTime,
            string variableName
            )
        {
            if (dateTime == DateTime.MinValue)
              return Result.Failure<bool, Errors>(
                  Errors.One(
                    message: $"{variableName} has the default value",
                    code: ErrorCodes.InvalidValue
                    )
                  );

            return Result.Ok<bool, Errors>(dateTime);
        }

        public static Result<bool, Errors> ValidateNonZero(
            int number,
            string variableName
            )
        {
            if (number == 0)
              return Result.Failure<bool, Errors>(
                  Errors.One(
                    message: $"{variableName} is 0",
                    code: ErrorCodes.InvalidValue
                    )
                  );

            return Result.Ok<bool, Errors>(number);
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