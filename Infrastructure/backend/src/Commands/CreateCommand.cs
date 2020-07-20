using CSharpFunctionalExtensions;
using Infrastructure.Commands;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Infrastructure.Commands
{
    public sealed class CreateCommand<TInput>
      : CommandBase<Result<TimestampedId, Errors>>
    {
        public TInput Input { get; }

        private CreateCommand(
            TInput input,
            Id creatorId
            )
          : base(creatorId)
        {
            Input = input;
        }

        public static Result<CreateCommand<TInput>, Errors> From(
            TInput input,
            Id creatorId
            )
        {
            return Result.Success<CreateCommand<TInput>, Errors>(
                    new CreateCommand<TInput>(
                        input: input,
                        creatorId: creatorId
                        )
                    );
        }
    }
}