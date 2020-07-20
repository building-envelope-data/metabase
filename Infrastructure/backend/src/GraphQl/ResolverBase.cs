namespace Infrastructure.GraphQl
{
    public abstract class ResolverBase
    {
        protected Queries.IQueryBus QueryBus { get; }

        protected ResolverBase(Queries.IQueryBus queryBus)
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