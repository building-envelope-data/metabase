using CSharpFunctionalExtensions;
using Infrastructure.Commands;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Commands
{
    // The difference between the words `remove` and `delete` is explained on
    // https://english.stackexchange.com/questions/52508/difference-between-delete-and-remove
    public sealed class Delete<TModel>
      : CommandBase<Result<TimestampedId, Errors>>
    {
        public TimestampedId TimestampedId { get; }

        private Delete(
            TimestampedId timestampedId,
            Id creatorId
            )
          : base(creatorId)
        {
            TimestampedId = timestampedId;
        }

        public static Result<Delete<TModel>, Errors> From(
            TimestampedId timestampedId,
            Id creatorId
            )
        {
            return Result.Ok<Delete<TModel>, Errors>(
                    new Delete<TModel>(
                        timestampedId: timestampedId,
                        creatorId: creatorId
                        )
                    );
        }
    }
}