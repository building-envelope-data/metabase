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
    public sealed class GetDevelopersOfMethodsIdentifiedByTimestampedIdsHandler
      : IQueryHandler<Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.Method, Models.MethodDeveloper, Models.Stakeholder>, IEnumerable<Result<IEnumerable<Result<Models.Stakeholder, Errors>>, Errors>>>
    {
        private readonly IAggregateRepository _repository;
        private readonly GetForwardAssociatesOfModelsIdentifiedByTimestampedIdsHandler<Models.Method, Models.MethodDeveloper, Models.Institution, Aggregates.InstitutionAggregate, Events.InstitutionMethodDeveloperAdded> _getInstitutionDevelopersHandler;
        private readonly GetForwardAssociatesOfModelsIdentifiedByTimestampedIdsHandler<Models.Method, Models.MethodDeveloper, Models.Person, Aggregates.PersonAggregate, Events.PersonMethodDeveloperAdded> _getPersonDevelopersHandler;

        public GetDevelopersOfMethodsIdentifiedByTimestampedIdsHandler(IAggregateRepository repository)
        {
            _repository = repository;
            _getInstitutionDevelopersHandler =
              new GetForwardAssociatesOfModelsIdentifiedByTimestampedIdsHandler<Models.Method, Models.MethodDeveloper, Models.Institution, Aggregates.InstitutionAggregate, Events.InstitutionMethodDeveloperAdded>(repository);
            _getPersonDevelopersHandler =
              new GetForwardAssociatesOfModelsIdentifiedByTimestampedIdsHandler<Models.Method, Models.MethodDeveloper, Models.Person, Aggregates.PersonAggregate, Events.PersonMethodDeveloperAdded>(repository);
        }

        public async Task<IEnumerable<Result<IEnumerable<Result<Models.Stakeholder, Errors>>, Errors>>> Handle(
            Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<Models.Method, Models.MethodDeveloper, Models.Stakeholder> query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                // TODO Await both results in parallel by using `WhenAll`?  There are sadly no n-tuple overloads of `WhenAll` as discussed in https://github.com/dotnet/runtime/issues/20166
                var institutionsResults = await _getInstitutionDevelopersHandler.Handle(
                    Queries.GetForwardAssociatesOfModelsIdentifiedByTimestampedIds<Models.Method, Models.MethodDeveloper, Models.Institution>
                      .From(query.TimestampedModelIds).Value, // Using `Value` is potentially unsafe but should be safe here!
                    session,
                    cancellationToken
                    );
                var personsResults = await _getPersonDevelopersHandler.Handle(
                    Queries.GetBackwardAssociatesOfModelsIdentifiedByTimestampedIds<Models.Method, Models.MethodDeveloper, Models.Person>
                      .From(query.TimestampedModelIds).Value, // Using `Value` is potentially unsafe but should be safe here!
                    session,
                    cancellationToken
                    );
                return institutionsResults.Zip(personsResults, (institutionsResult, personsResult) =>
                    Errors.Combine(institutionsResult, personsResult).Map(_ =>
                        institutionsResult.Value.Select(r => r.Map(i => (Models.Stakeholder)i))
                        .Concat(personsResult.Value.Select(r => r.Map(p => (Models.Stakeholder)p)))
                        )
                    );
            }
        }
    }
}