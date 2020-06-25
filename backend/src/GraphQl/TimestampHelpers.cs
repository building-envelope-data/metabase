using HotChocolate.Execution;
using HotChocolate.Resolvers;

namespace Icon.GraphQl
{
    public static class TimestampHelpers
    {
        public static void Store(
                        ValueObjects.Timestamp timestamp,
                        IQueryRequestBuilder requestBuilder
                        )
        {
            requestBuilder.SetProperty("requestTimestamp", timestamp);
        }

        public static ValueObjects.Timestamp Fetch(IResolverContext context)
        {
            return (ValueObjects.Timestamp)context.ContextData["requestTimestamp"];
        }

        public static ValueObjects.TimestampedId TimestampId(
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