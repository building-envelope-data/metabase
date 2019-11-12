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
using Marten.Linq.MatchesSql;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.Handlers
{
    public sealed class ListComponentVersionsHandler :
      IQueryHandler<Queries.ListComponentVersions, IEnumerable<Result<Models.ComponentVersion, IError>>>
    {
        private readonly IDocumentSession _session;
        private readonly IAggregateRepository _repository;

        public ListComponentVersionsHandler(IDocumentSession session, IAggregateRepository repository)
        {
            _session = session;
            _repository = repository;
        }

        public async Task<IEnumerable<Result<Models.ComponentVersion, IError>>> Handle(
            Queries.ListComponentVersions query,
            CancellationToken cancellationToken
            )
        {
            var ids =
              await _repository.Query<Events.ComponentVersionCreated>()
              .Where(e => e.ComponentId == query.ComponentId)
              .Select(e => e.ComponentVersionId)
              .ToListAsync();

            return
              (await
               _repository.LoadAllThatExisted<Aggregates.ComponentVersionAggregate>(
                 ids,
                 query.Timestamp,
                 cancellationToken
                 )
              ).Select(result =>
                result.Map(a => a.ToModel())
                );
        }
    }
}