using CSharpFunctionalExtensions;
using Icon.Infrastructure.Command;

namespace Icon.Commands
{
    public sealed class AddAssociation<TInput>
      : CommandBase<Result<ValueObjects.TimestampedId, Errors>>
    {
        public TInput Input { get; }

        private AddAssociation(
            TInput input,
            ValueObjects.Id creatorId
            )
          : base(creatorId)
        {
            Input = input;
        }

        public static Result<AddAssociation<TInput>, Errors> From(
            TInput input,
            ValueObjects.Id creatorId
            )
        {
            return Result.Ok<AddAssociation<TInput>, Errors>(
                    new AddAssociation<TInput>(
                        input: input,
                        creatorId: creatorId
                        )
                    );
        }
    }
}