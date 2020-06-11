using System; // Func
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Command;
using Aggregates = Icon.Aggregates;
using CancellationToken = System.Threading.CancellationToken;
using Commands = Icon.Commands;
using DateTime = System.DateTime;
using Events = Icon.Events;

namespace Icon.Handlers
{
    public sealed class DeleteModelHandler<TModel, TAggregate>
      : ICommandHandler<Commands.Delete<TModel>, Result<ValueObjects.TimestampedId, Errors>>
      where TAggregate : class, IEventSourcedAggregate, new()
    {
        private readonly IAggregateRepository _repository;
        private readonly Func<Commands.Delete<TModel>, Events.IDeletedEvent> _newDeletedEvent;
        private readonly IEnumerable<Func<IAggregateRepositorySession, ValueObjects.TimestampedId, ValueObjects.Id, CancellationToken, Task<Result<bool, Errors>>>> _removeAssociations;

        public DeleteModelHandler(
            IAggregateRepository repository,
            Func<Commands.Delete<TModel>, Events.IDeletedEvent> newDeletedEvent,
            IEnumerable<Func<IAggregateRepositorySession, ValueObjects.TimestampedId, ValueObjects.Id, CancellationToken, Task<Result<bool, Errors>>>> removeAssociations
            )
        {
            _repository = repository;
            _newDeletedEvent = newDeletedEvent;
            _removeAssociations = removeAssociations;
        }

        public async Task<Result<ValueObjects.TimestampedId, Errors>> Handle(
            Commands.Delete<TModel> command,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenSession())
            {
                return await Handle(session, command, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task<Result<ValueObjects.TimestampedId, Errors>> Handle(
            IAggregateRepositorySession session,
            Commands.Delete<TModel> command,
            CancellationToken cancellationToken
            )
        {
            return await
              (await session.Load<TAggregate>(command.TimestampedId, cancellationToken))
              .Bind(async _ =>
                  {
                      // We cannot aggregate the removal tasks and execute them in
                      // parallel with `Task.WhenAll` because all removal tasks use the
                      // same session.
                      var associationRemovalResults = new List<Result<bool, Errors>>();
                      foreach (var removeAssociation in _removeAssociations)
                      {
                          associationRemovalResults.Add(
                              await removeAssociation(
                                session,
                                command.TimestampedId,
                                command.CreatorId,
                                cancellationToken
                                )
                              .ConfigureAwait(false)
                              );
                      }
                      return
                      await Result.Combine(associationRemovalResults)
                      .Bind(async _ =>
                          {
                              var @event = _newDeletedEvent(command);
                              return await (
                              await session.Delete<TAggregate>(
                                command.TimestampedId.Timestamp, @event, cancellationToken
                                )
                              .ConfigureAwait(false)
                              )
                          .Bind(async _ => await
                              (await session.Save(cancellationToken).ConfigureAwait(false))
                              .Bind(async _ => await
                                session.TimestampId<TAggregate>(command.TimestampedId.Id, cancellationToken).ConfigureAwait(false)
                                )
                              .ConfigureAwait(false)
                              )
                          .ConfigureAwait(false);
                          }
                          )
                      .ConfigureAwait(false);
                  }
            )
              .ConfigureAwait(false);
        }
    }
}