using System; // Func
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.Commands;
using Infrastructure.Events;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using CancellationToken = System.Threading.CancellationToken;
using Errors = Infrastructure.Errors;

namespace Infrastructure.Handlers
{
    public sealed class DeleteModelHandler<TModel, TAggregate>
      : ICommandHandler<Infrastructure.Commands.Delete<TModel>, Result<TimestampedId, Errors>>
      where TModel : IModel
      where TAggregate : class, IAggregate, IConvertible<TModel>, new()
    {
        private readonly IModelRepository _repository;
        private readonly Func<Infrastructure.Commands.Delete<TModel>, IDeletedEvent> _newDeletedEvent;
        private readonly IEnumerable<Func<ModelRepositorySession, TimestampedId, Id, CancellationToken, Task<Result<bool, Errors>>>> _removeAssociations;

        public DeleteModelHandler(
            IModelRepository repository,
            Func<Infrastructure.Commands.Delete<TModel>, IDeletedEvent> newDeletedEvent,
            IEnumerable<Func<ModelRepositorySession, TimestampedId, Id, CancellationToken, Task<Result<bool, Errors>>>> removeAssociations
            )
        {
            _repository = repository;
            _newDeletedEvent = newDeletedEvent;
            _removeAssociations = removeAssociations;
        }

        public async Task<Result<TimestampedId, Errors>> Handle(
            Infrastructure.Commands.Delete<TModel> command,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenSession())
            {
                return await Handle(session, command, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task<Result<TimestampedId, Errors>> Handle(
            ModelRepositorySession session,
            Infrastructure.Commands.Delete<TModel> command,
            CancellationToken cancellationToken
            )
        {
            return await
              (await session.Load<TModel, TAggregate>(command.TimestampedId, cancellationToken).ConfigureAwait(false))
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