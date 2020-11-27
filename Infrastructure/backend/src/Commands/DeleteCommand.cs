using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;

namespace Infrastructure.Commands
{
    // The difference between the words `remove` and `delete` is explained on
    // https://english.stackexchange.com/questions/52508/difference-between-delete-and-remove
    public sealed class DeleteCommand<TModel>
      : CommandBase<Result<TimestampedId, Errors>>
    {
        public TimestampedId TimestampedId { get; }

        private DeleteCommand(
            TimestampedId timestampedId,
            Id creatorId
            )
          : base(creatorId)
        {
            TimestampedId = timestampedId;
        }

        public static Result<DeleteCommand<TModel>, Errors> From(
            TimestampedId timestampedId,
            Id creatorId
            )
        {
            return Result.Success<DeleteCommand<TModel>, Errors>(
                    new DeleteCommand<TModel>(
                        timestampedId: timestampedId,
                        creatorId: creatorId
                        )
                    );
        }
    }
}