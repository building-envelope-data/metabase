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
using CSharpFunctionalExtensions;
using Errors = Icon.Errors;

namespace Icon.Handlers
{
    public sealed class GetDevelopersOfMethodsHandler
      : IQueryHandler<Queries.GetManyToManyAssociatesOfModels<Models.Method, Models.MethodDeveloper, Models.Stakeholder>, IEnumerable<Result<IEnumerable<Result<Models.Stakeholder, Errors>>, Errors>>>
    {
        public static async Task<IEnumerable<Result<IEnumerable<Result<Models.Stakeholder, Errors>>, Errors>>> Do(
            IAggregateRepositoryReadOnlySession session,
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            CancellationToken cancellationToken
            )
        {
            // Note that the two handlers must not be executed simultaneously
            // because they both use the same session.
            var institutionsResults = await GetForwardManyToManyAssociatesOfModelsHandler<Models.Method, Models.MethodDeveloper, Models.Institution, Aggregates.MethodAggregate, Aggregates.InstitutionMethodDeveloperAggregate, Aggregates.InstitutionAggregate, Events.InstitutionMethodDeveloperAdded>.Do(
                session,
                timestampedIds,
                cancellationToken
                )
              .ConfigureAwait(false);
            var personsResults = await GetForwardManyToManyAssociatesOfModelsHandler<Models.Method, Models.MethodDeveloper, Models.Person, Aggregates.MethodAggregate, Aggregates.PersonMethodDeveloperAggregate, Aggregates.PersonAggregate, Events.PersonMethodDeveloperAdded>.Do(
                session,
                timestampedIds,
                cancellationToken
                )
              .ConfigureAwait(false);
            return institutionsResults.Zip(personsResults, (institutionsResult, personsResult) =>
                Errors.Combine(institutionsResult, personsResult).Map(_ =>
                  institutionsResult.Value.Select(r => r.Map(i => (Models.Stakeholder)i))
                  .Concat(personsResult.Value.Select(r => r.Map(p => (Models.Stakeholder)p)))
                  )
                );
        }

        private readonly IAggregateRepository _repository;

        public GetDevelopersOfMethodsHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Result<IEnumerable<Result<Models.Stakeholder, Errors>>, Errors>>> Handle(
            Queries.GetManyToManyAssociatesOfModels<Models.Method, Models.MethodDeveloper, Models.Stakeholder> query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                return await Do(session, query.TimestampedIds, cancellationToken);
            }
        }
    }
}