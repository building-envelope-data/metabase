namespace Infrastructure.Models
{
    public sealed class ModelRepository : IModelRepository
    {
        private readonly Marten.IDocumentStore _store;
        private readonly Events.IEventBus _eventBus;

        public ModelRepository(
            Marten.IDocumentStore store,
            Events.IEventBus eventBus
            )
        {
            _store = store;
            _eventBus = eventBus;
        }

        // For the different kinds of Marten sessions see http://jasperfx.github.io/marten/documentation/troubleshoot/
        public ModelRepositorySession OpenSession()
        {
            return new ModelRepositorySession(_store.OpenSession(), _eventBus);
        }

        public ModelRepositoryReadOnlySession OpenReadOnlySession()
        {
            // TODO We sould like to use `QuerySession` here, which however does
            // not provide access to an `IEventStore`.
            return new ModelRepositoryReadOnlySession(_store.OpenSession());
        }
    }
}