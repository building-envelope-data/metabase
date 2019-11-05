using Guid = System.Guid;
using System.Collections.Generic;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Event;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Query;
using Models = Icon.Models;
using Queries = Icon.Queries;
using Aggregates = Icon.Aggregates;
using System.Linq;
using Marten;

namespace Icon.Handlers
{
    public class ListComponentsHandler :
      IQueryHandler<Queries.ListComponents, IEnumerable<Models.Component>>
    {
        private readonly IAggregateRepository _repository;

        public ListComponentsHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Models.Component>> Handle(Queries.ListComponents query, CancellationToken cancellationToken)
        {
            if (query.Timestamp == null)
            {
              return (await _repository.LoadAll<Aggregates.ComponentAggregate>(cancellationToken: cancellationToken))
                .Select(a => a.ToModel());
            }
            else
            {
                return (await _repository.LoadAll<Aggregates.ComponentAggregate>(query.Timestamp, cancellationToken))
                  .Select(a => a.ToModel());
            }
        }
    }
}