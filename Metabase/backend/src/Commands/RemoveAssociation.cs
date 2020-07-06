using CSharpFunctionalExtensions;
using Infrastructure.Commands;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Commands
{
    // The difference between the words `remove` and `delete` is explained on
    // https://english.stackexchange.com/questions/52508/difference-between-delete-and-remove
    public sealed class RemoveAssociation<TInput>
      : CommandBase<Result<TimestampedId, Errors>>
    {
        public TInput Input { get; }

        private RemoveAssociation(
            TInput input,
            Id creatorId
            )
          : base(creatorId)
        {
            Input = input;
        }

        public static Result<RemoveAssociation<TInput>, Errors> From(
            TInput input,
            Id creatorId
            )
        {
            return Result.Ok<RemoveAssociation<TInput>, Errors>(
                    new RemoveAssociation<TInput>(
                        input: input,
                        creatorId: creatorId
                        )
                    );
        }
    }
}