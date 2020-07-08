using System; // Func
using System.Linq; // Enumerable.Empty
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.Commands;
using Infrastructure.ValueObjects;
using CancellationToken = System.Threading.CancellationToken;
using ErrorCodes = Infrastructure.ErrorCodes;
using Errors = Infrastructure.Errors;

namespace Metabase.Handlers
{
    public sealed class AddMethodDeveloperHandler
      : ICommandHandler<Commands.AddAssociation<ValueObjects.AddMethodDeveloperInput>, Result<TimestampedId, Errors>>
    {
        private readonly IAggregateRepository _repository;
        private readonly CreateModelHandler<Commands.AddAssociation<ValueObjects.AddMethodDeveloperInput>, Aggregates.InstitutionMethodDeveloperAggregate> _addInstitutionMethodDeveloperHandler;
        private readonly CreateModelHandler<Commands.AddAssociation<ValueObjects.AddMethodDeveloperInput>, Aggregates.PersonMethodDeveloperAggregate> _addPersonMethodDeveloperHandler;

        public AddMethodDeveloperHandler(IAggregateRepository repository)
        {
            _repository = repository;
            _addInstitutionMethodDeveloperHandler =
              new CreateModelHandler<Commands.AddAssociation<ValueObjects.AddMethodDeveloperInput>, Aggregates.InstitutionMethodDeveloperAggregate>(
                  repository,
                  Events.InstitutionMethodDeveloperAdded.From,
                  Enumerable.Empty<Func<IAggregateRepositorySession, Id, Commands.AddAssociation<ValueObjects.AddMethodDeveloperInput>, CancellationToken, Task<Result<Id, Errors>>>>()
                  );
            _addPersonMethodDeveloperHandler =
              new CreateModelHandler<Commands.AddAssociation<ValueObjects.AddMethodDeveloperInput>, Aggregates.PersonMethodDeveloperAggregate>(
                  repository,
                  Events.PersonMethodDeveloperAdded.From,
                  Enumerable.Empty<Func<IAggregateRepositorySession, Id, Commands.AddAssociation<ValueObjects.AddMethodDeveloperInput>, CancellationToken, Task<Result<Id, Errors>>>>()
                  );
        }

        public async Task<Result<TimestampedId, Errors>> Handle(
            Commands.AddAssociation<ValueObjects.AddMethodDeveloperInput> command,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenSession())
            {
                return await
                  (await session.FetchAggregateType(
                      command.Input.StakeholderId,
                      cancellationToken
                      )
                    .ConfigureAwait(false)
                    )
                  .Bind(async aggregateType =>
                    {
                        if (aggregateType == typeof(Aggregates.InstitutionAggregate))
                            return await _addInstitutionMethodDeveloperHandler.Handle(session, command, cancellationToken).ConfigureAwait(false);
                        if (aggregateType == typeof(Aggregates.PersonAggregate))
                            return await _addPersonMethodDeveloperHandler.Handle(session, command, cancellationToken).ConfigureAwait(false);
                        return Result.Failure<TimestampedId, Errors>(
                        Errors.One(
                          message: $"The stakeholder with id {command.Input.StakeholderId} has the aggregate type {aggregateType} which is none of the expected types {typeof(Aggregates.InstitutionAggregate)} and {typeof(Aggregates.PersonAggregate)}",
                          code: ErrorCodes.InvalidType
                          )
                        );
                    }
                    )
                  .ConfigureAwait(false);
            }
        }
    }
}