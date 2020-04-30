using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Aggregate;
using Events = Icon.Events;
using Commands = Icon.Commands;
using Aggregates = Icon.Aggregates;
using CSharpFunctionalExtensions;

namespace Icon.Handlers
{
    public sealed class AddMethodDeveloperHandler
      : ICommandHandler<Commands.AddMethodDeveloper, Result<ValueObjects.TimestampedId, Errors>>
    {
        private readonly IAggregateRepository _repository;
        private readonly CreateModelHandler<Commands.AddMethodDeveloper, Aggregates.InstitutionMethodDeveloperAggregate> _addInstitutionMethodDeveloperHandler;
        private readonly CreateModelHandler<Commands.AddMethodDeveloper, Aggregates.PersonMethodDeveloperAggregate> _addPersonMethodDeveloperHandler;

        public AddMethodDeveloperHandler(IAggregateRepository repository)
        {
            _repository = repository;
            _addInstitutionMethodDeveloperHandler =
              new CreateModelHandler<Commands.AddMethodDeveloper, Aggregates.InstitutionMethodDeveloperAggregate>(
                  repository,
                  Events.InstitutionMethodDeveloperAdded.From
                  );
            _addPersonMethodDeveloperHandler =
              new CreateModelHandler<Commands.AddMethodDeveloper, Aggregates.PersonMethodDeveloperAggregate>(
                  repository,
                  Events.PersonMethodDeveloperAdded.From
                  );
        }

        public async Task<Result<ValueObjects.TimestampedId, Errors>> Handle(
            Commands.AddMethodDeveloper command,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenSession())
            {
                return await (
                    await session.FetchAggregateType(
                      command.Input.StakeholderId,
                      cancellationToken
                      )
                    ).Bind(async aggregateType =>
                    {
                        if (aggregateType == typeof(Aggregates.InstitutionAggregate))
                            return await _addInstitutionMethodDeveloperHandler.Handle(command, session, cancellationToken);
                        if (aggregateType == typeof(Aggregates.PersonAggregate))
                            return await _addPersonMethodDeveloperHandler.Handle(command, session, cancellationToken);
                        return Result.Failure<ValueObjects.TimestampedId, Errors>(
                        Errors.One(
                          message: $"The stakeholder with id {command.Input.StakeholderId} has the aggregate type {aggregateType} which is none of the expected types {typeof(Aggregates.InstitutionAggregate)} and {typeof(Aggregates.PersonAggregate)}",
                          code: ErrorCodes.InvalidType
                          )
                        );
                    }
                    );
            }
        }
    }
}