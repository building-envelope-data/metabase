using CSharpFunctionalExtensions;
using Infrastructure.Commands;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Infrastructure.Commands
{
    // The difference between the words `remove` and `delete` is explained on
    // https://english.stackexchange.com/questions/52508/difference-between-delete-and-remove
    public sealed class RemoveAssociationCommand<TInput>
      : CommandBase<Result<TimestampedId, Errors>>
    {
        public TInput Input { get; }

        private RemoveAssociationCommand(
            TInput input,
            Id creatorId
            )
          : base(creatorId)
        {
            Input = input;
        }

        public static Result<RemoveAssociationCommand<TInput>, Errors> From(
            TInput input,
            Id creatorId
            )
        {
            return Result.Success<RemoveAssociationCommand<TInput>, Errors>(
                    new RemoveAssociationCommand<TInput>(
                        input: input,
                        creatorId: creatorId
                        )
                    );
        }
    }
}