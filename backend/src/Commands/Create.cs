using CSharpFunctionalExtensions;
using Icon.Infrastructure.Commands;

namespace Icon.Commands
{
    public sealed class Create<TInput>
      : CommandBase<Result<ValueObjects.TimestampedId, Errors>>
    {
        public TInput Input { get; }

        private Create(
            TInput input,
            ValueObjects.Id creatorId
            )
          : base(creatorId)
        {
            Input = input;
        }

        public static Result<Create<TInput>, Errors> From(
            TInput input,
            ValueObjects.Id creatorId
            )
        {
            return Result.Ok<Create<TInput>, Errors>(
                    new Create<TInput>(
                        input: input,
                        creatorId: creatorId
                        )
                    );
        }
    }
}