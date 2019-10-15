using Guid = System.Guid;
using System.Collections.Generic;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Event;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Query;
using Icon.Domain;

namespace Icon.Domain.Component.List
{
    public class Query : IQuery<IEnumerable<ComponentAggregate>>
    {
    }

    public class QueryHandler :
        IQueryHandler<Query, IEnumerable<ComponentAggregate>>
    {
        private readonly IAggregateRepository _repository;

        public QueryHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ComponentAggregate>> Handle(Query query, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _repository.LoadAll<ComponentAggregate>(cancellationToken);
        }
    }
}