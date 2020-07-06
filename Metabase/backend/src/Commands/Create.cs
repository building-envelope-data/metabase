using CSharpFunctionalExtensions;
using Infrastructure.Commands;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Commands
{
    public sealed class Create<TInput>
      : CommandBase<Result<TimestampedId, Errors>>
    {
        public TInput Input { get; }

        private Create(
            TInput input,
            Id creatorId
            )
          : base(creatorId)
        {
            Input = input;
        }

        public static Result<Create<TInput>, Errors> From(
            TInput input,
            Id creatorId
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