using Guid = System.Guid;
using System.Collections.Generic;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure.Command;
using Icon.Events;
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
    public sealed class ListComponentVersionsHandler
      : IQueryHandler<Queries.ListComponentVersions, ILookup<ValueObjects.TimestampedId, Result<Models.ComponentVersion, Errors>>>
    {
        private readonly IAggregateRepository _repository;

        public ListComponentVersionsHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<ILookup<ValueObjects.TimestampedId, Result<Models.ComponentVersion, Errors>>> Handle(
            Queries.ListComponentVersions query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                /* var timestampedComponentIdToVersionIds = (await Task.WhenAll( */
                /*     query.TimestampedComponentIds.Select(async timestampedComponentId => */
                /*     { */
                /*         var ids = */
                /*       await session.Query<Events.ComponentVersionCreated>() */
                /*       .Where(e => e.ComponentId == timestampedComponentId.Id) */
                /*       .Select(e => e.ComponentVersionId) */
                /*       .ToListAsync(cancellationToken); */
                /*         return (timestampedComponentId, ids); */
                /*     }))).ToDictionary(t => t.Item1, t => t.Item2); */
                /* return (await */
                /*   session.LoadAllThatExistedGrouped<Aggregates.ComponentVersionAggregate>( */
                /*       timestampedComponentIdToVersionIds, */
                /*       cancellationToken */
                /*       ) */
                /*   ).Select(result => */
                /*     result.Bind(a => a.ToModel()) */
                /*     ); */
                return null!;
            }
        }
    }
}