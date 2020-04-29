using Icon.Infrastructure.Query;
using DateTime = System.DateTime;

namespace Icon.GraphQl
{
    public abstract class ResolversBase
    {
        protected IQueryBus QueryBus { get; }

        public ResolversBase(IQueryBus queryBus)
        {
            QueryBus = queryBus;
        }

        protected ValueObjects.TimestampedId TimestampId(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp
            )
        {
            return ResultHelpers.HandleFailure(
                ValueObjects.TimestampedId.From(
                  id, timestamp
                  )
                );
        }
    }
}