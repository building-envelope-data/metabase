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
    public sealed class CreateModelHandler<TCommand, TAggregate>
      : ICommandHandler<TCommand, Result<TimestampedId, Errors>>
      where TCommand : ICommand<Result<TimestampedId, Errors>>
      where TAggregate : class, IAggregate, new()
    {
        private readonly IModelRepository _repository;
        private readonly Func<Guid, TCommand, ICreatedEvent> _newCreatedEvent;
        private readonly IEnumerable<Func<ModelRepositorySession, Id, TCommand, CancellationToken, Task<Result<Id, Errors>>>> _addAssociations;

        public CreateModelHandler(
            IModelRepository repository,
            Func<Guid, TCommand, ICreatedEvent> newCreatedEvent,
            IEnumerable<Func<ModelRepositorySession, Id, TCommand, CancellationToken, Task<Result<Id, Errors>>>> addAssociations
            )
        {
            _repository = repository;
            _newCreatedEvent = newCreatedEvent;
            _addAssociations = addAssociations;
        }

        public async Task<Result<TimestampedId, Errors>> Handle(
            TCommand command,
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
            TCommand command,
            CancellationToken cancellationToken
            )
        {
            return await (
                await session.Create<TAggregate>(
                  id => _newCreatedEvent(id, command),
                  cancellationToken
                  )
                .ConfigureAwait(false)
              )
              .Bind(async id =>
                  {
                      // We cannot aggregate the addition tasks and execute them in
                      // parallel with `Task.WhenAll` because all addition tasks use the
                      // same session.
                      var associationAdditionResults = new List<Result<Id, Errors>>();
                      foreach (var addAssociation in _addAssociations)
                      {
                          associationAdditionResults.Add(
                              await addAssociation(
                                session, id, command, cancellationToken
                                )
                              .ConfigureAwait(false)
                              );
                      }
                      return await
                      Result.Combine(associationAdditionResults)
                      .Bind(async _ => await
                          (await session.Save(cancellationToken).ConfigureAwait(false))
                          .Bind(async _ =>
                            await session.TimestampId<TAggregate>(id, cancellationToken).ConfigureAwait(false)
                            )
                          .ConfigureAwait(false)
                          )
                      .ConfigureAwait(false);
                  }
            )
              .ConfigureAwait(false);
        }
    }
}