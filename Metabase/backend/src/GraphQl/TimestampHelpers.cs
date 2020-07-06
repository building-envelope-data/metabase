using HotChocolate.Execution;
using HotChocolate.Resolvers;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
{
    public static class TimestampHelpers
    {
        public static void Store(
                        Timestamp timestamp,
                        IQueryRequestBuilder requestBuilder
                        )
        {
            requestBuilder.SetProperty("requestTimestamp", timestamp);
        }

        public static Timestamp Fetch(IResolverContext context)
        {
            return (Timestamp)context.ContextData["requestTimestamp"];
        }

        public static TimestampedId TimestampId(
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