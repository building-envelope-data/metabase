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
using Events = Icon.Events;
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
              var ids =
                await _repository.Query<Events.ComponentCreated>()
                .Select(e => e.ComponentId)
                .ToListAsync();

                return
                  (await _repository
                   .LoadAll<Aggregates.ComponentAggregate>(
                     ids,
                     query.Timestamp,
                     cancellationToken
                     )
                   ).Select(a => a.ToModel());
        }
    }
}