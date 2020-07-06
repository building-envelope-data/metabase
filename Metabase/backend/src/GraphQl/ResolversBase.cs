using Infrastructure.Queries;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
{
    public abstract class ResolversBase
    {
        protected IQueryBus QueryBus { get; }

        public ResolversBase(IQueryBus queryBus)
        {
            QueryBus = queryBus;
        }

        protected TimestampedId TimestampId(
            Id id,
            Timestamp timestamp
            )
        {
            return ResultHelpers.HandleFailure(
                TimestampedId.From(
                  id, timestamp
                  )
                );
        }
    }
}