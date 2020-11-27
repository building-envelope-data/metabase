namespace Infrastructure.GraphQl
{
    public abstract class Query
    {
        protected readonly Queries.IQueryBus _queryBus;

        protected Query(Queries.IQueryBus queryBus)
        {
            _queryBus = queryBus;
        }
    }
}