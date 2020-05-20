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
      : IQueryHandler<Queries.GetAssociatesOfModels<Models.Method, Models.MethodDeveloper, Models.Stakeholder>, IEnumerable<Result<IEnumerable<Result<Models.Stakeholder, Errors>>, Errors>>>
    {
        private readonly IAggregateRepository _repository;
        private readonly GetForwardManyToManyAssociatesOfModelsHandler<Models.Method, Models.MethodDeveloper, Models.Institution, Aggregates.MethodAggregate, Aggregates.InstitutionMethodDeveloperAggregate, Aggregates.InstitutionAggregate, Events.InstitutionMethodDeveloperAdded> _getInstitutionDevelopersHandler;
        private readonly GetForwardManyToManyAssociatesOfModelsHandler<Models.Method, Models.MethodDeveloper, Models.Person, Aggregates.MethodAggregate, Aggregates.PersonMethodDeveloperAggregate, Aggregates.PersonAggregate, Events.PersonMethodDeveloperAdded> _getPersonDevelopersHandler;

        public GetDevelopersOfMethodsHandler(IAggregateRepository repository)
        {
            _repository = repository;
            _getInstitutionDevelopersHandler =
              new GetForwardManyToManyAssociatesOfModelsHandler<Models.Method, Models.MethodDeveloper, Models.Institution, Aggregates.MethodAggregate, Aggregates.InstitutionMethodDeveloperAggregate, Aggregates.InstitutionAggregate, Events.InstitutionMethodDeveloperAdded>(repository);
            _getPersonDevelopersHandler =
              new GetForwardManyToManyAssociatesOfModelsHandler<Models.Method, Models.MethodDeveloper, Models.Person, Aggregates.MethodAggregate, Aggregates.PersonMethodDeveloperAggregate, Aggregates.PersonAggregate, Events.PersonMethodDeveloperAdded>(repository);
        }

        public async Task<IEnumerable<Result<IEnumerable<Result<Models.Stakeholder, Errors>>, Errors>>> Handle(
            Queries.GetAssociatesOfModels<Models.Method, Models.MethodDeveloper, Models.Stakeholder> query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                // There are sadly no n-tuple overloads of `WhenAll` as discussed in
                // https://github.com/dotnet/runtime/issues/20166
                // However, there is a NuGet package that does that
                // https://www.nuget.org/packages/TaskTupleAwaiter/
                // https://github.com/buvinghausen/TaskTupleAwaiter
                // Because that package seems to be out-dated, I use good old
                // `WhenAll` to await the tasks and extract the result
                // afterwards using `Task#Result` as was mentioned in
                // https://github.com/dotnet/runtime/issues/20166#issuecomment-428028466
                var institutionsResultsTask = _getInstitutionDevelopersHandler.Handle(
                    query.TimestampedModelIds,
                    session,
                    cancellationToken
                    );
                var personsResultsTask = _getPersonDevelopersHandler.Handle(
                    query.TimestampedModelIds,
                    session,
                    cancellationToken
                    );
                await Task.WhenAll(
                    (Task)institutionsResultsTask,
                    (Task)personsResultsTask
                    )
                  .ConfigureAwait(false);
                return institutionsResultsTask.Result.Zip( // Using `Result` here and below is potentially unsafe but should be safe here!
                    personsResultsTask.Result,
                    (institutionsResult, personsResult) =>
                    Errors.Combine(institutionsResult, personsResult).Map(_ =>
                        institutionsResult.Value.Select(r => r.Map(i => (Models.Stakeholder)i))
                        .Concat(personsResult.Value.Select(r => r.Map(p => (Models.Stakeholder)p)))
                        )
                    );
            }
        }
    }
}