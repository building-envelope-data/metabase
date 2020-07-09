using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using CancellationToken = System.Threading.CancellationToken;
using Errors = Infrastructure.Errors;

namespace Metabase.Handlers
{
    public sealed class GetDevelopersOfMethodsHandler
      : IQueryHandler<GetForwardManyToManyAssociatesOfModels<Models.Method, Models.MethodDeveloper, Models.Stakeholder>, IEnumerable<Result<IEnumerable<Result<Models.Stakeholder, Errors>>, Errors>>>
    {
        public static async Task<IEnumerable<Result<IEnumerable<Result<Models.Stakeholder, Errors>>, Errors>>> Do(
            IAggregateRepositoryReadOnlySession session,
            IEnumerable<TimestampedId> timestampedIds,
            CancellationToken cancellationToken
            )
        {
            // Note that the two handlers must not be executed simultaneously
            // because they both use the same session.
            var institutionsResults = await session.GetForwardManyToManyAssociatesOfModels<Models.Method, Models.MethodDeveloper, Models.Institution, Aggregates.MethodAggregate, Aggregates.InstitutionMethodDeveloperAggregate, Aggregates.InstitutionAggregate, Events.InstitutionMethodDeveloperAdded>(
                timestampedIds,
                cancellationToken
                )
              .ConfigureAwait(false);
            var personsResults = await session.GetForwardManyToManyAssociatesOfModels<Models.Method, Models.MethodDeveloper, Models.Person, Aggregates.MethodAggregate, Aggregates.PersonMethodDeveloperAggregate, Aggregates.PersonAggregate, Events.PersonMethodDeveloperAdded>(
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
            GetForwardManyToManyAssociatesOfModels<Models.Method, Models.MethodDeveloper, Models.Stakeholder> query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                return await Do(session, query.TimestampedIds, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}