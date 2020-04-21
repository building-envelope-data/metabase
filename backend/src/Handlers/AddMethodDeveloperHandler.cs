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
        private readonly AddInstitutionMethodDeveloperHandler _addInstitutionMethodDeveloperHandler;
        private readonly AddPersonMethodDeveloperHandler _addPersonMethodDeveloperHandler;

        public AddMethodDeveloperHandler(IAggregateRepository repository)
        {
            _repository = repository;
            _addInstitutionMethodDeveloperHandler = new AddInstitutionMethodDeveloperHandler(repository);
            _addPersonMethodDeveloperHandler = new AddPersonMethodDeveloperHandler(repository);
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

        public sealed class AddInstitutionMethodDeveloperHandler
          : CreateModelHandler<Commands.AddMethodDeveloper, Aggregates.InstitutionMethodDeveloperAggregate>
        {
            public AddInstitutionMethodDeveloperHandler(IAggregateRepository repository)
              : base(repository)
            {
            }

            protected override Events.IEvent NewCreatedEvent(ValueObjects.Id id, Commands.AddMethodDeveloper command)
            {
                return Events.InstitutionMethodDeveloperAdded.From(id, command);
            }
        }

        public sealed class AddPersonMethodDeveloperHandler
          : CreateModelHandler<Commands.AddMethodDeveloper, Aggregates.PersonMethodDeveloperAggregate>
        {
            public AddPersonMethodDeveloperHandler(IAggregateRepository repository)
              : base(repository)
            {
            }

            protected override Events.IEvent NewCreatedEvent(ValueObjects.Id id, Commands.AddMethodDeveloper command)
            {
                return Events.PersonMethodDeveloperAdded.From(id, command);
            }
        }
    }
}