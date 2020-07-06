using CSharpFunctionalExtensions;
using Infrastructure.Commands;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Commands
{
    public sealed class AddAssociation<TInput>
      : CommandBase<Result<TimestampedId, Errors>>
    {
        public TInput Input { get; }

        private AddAssociation(
            TInput input,
            Id creatorId
            )
          : base(creatorId)
        {
            Input = input;
        }

        public static Result<AddAssociation<TInput>, Errors> From(
            TInput input,
            Id creatorId
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