using CSharpFunctionalExtensions;
using Infrastructure.Commands;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Infrastructure.Commands
{
    public sealed class AddAssociationCommand<TInput>
      : CommandBase<Result<TimestampedId, Errors>>
    {
        public TInput Input { get; }

        private AddAssociationCommand(
            TInput input,
            Id creatorId
            )
          : base(creatorId)
        {
            Input = input;
        }

        public static Result<AddAssociationCommand<TInput>, Errors> From(
            TInput input,
            Id creatorId
            )
        {
            return Result.Success<AddAssociationCommand<TInput>, Errors>(
                    new AddAssociationCommand<TInput>(
                        input: input,
                        creatorId: creatorId
                        )
                    );
        }
    }
}