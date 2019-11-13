namespace Icon.Infrastructure.Aggregate
{
    public interface IAggregateRepository
    {
        public IAggregateRepositorySession OpenSession();
        public IAggregateRepositoryReadOnlySession OpenReadOnlySession();
    }
}
