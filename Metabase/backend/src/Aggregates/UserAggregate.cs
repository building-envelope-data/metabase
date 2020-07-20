using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Aggregates
{
    public sealed class UserAggregate
      : Aggregate, IConvertible<Models.User>
    {
#nullable disable
        public UserAggregate() { }
#nullable enable

        public void Apply(Marten.Events.Event<Events.UserCreated> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.AggregateId;
        }

        public void Apply(Marten.Events.Event<Events.UserDeleted> @event)
        {
            ApplyDeleted(@event);
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
            {
                return Result.Combine(
                    base.Validate()
                    );
            }
            return Result.Combine(
                base.Validate()
                );
        }

        public Result<Models.User, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.User, Errors>(virginResult.Error);

            var idResult = Infrastructure.ValueObjects.Id.From(Id);
            var timestampResult = Infrastructure.ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.Combine(
                  idResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.User.From(
                    id: idResult.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}