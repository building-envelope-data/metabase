namespace Infrastructure.Aggregates
{
    public interface IAggregateRepository
    {
        public IAggregateRepositorySession OpenSession();
        public IAggregateRepositoryReadOnlySession OpenReadOnlySession();
    }
}