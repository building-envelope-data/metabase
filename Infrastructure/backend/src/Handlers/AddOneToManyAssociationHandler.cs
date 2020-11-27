using System; // Func
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.Commands;
using Infrastructure.Events;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using CancellationToken = System.Threading.CancellationToken;

namespace Infrastructure.Handlers
{
    public sealed class AddOneToManyAssociationHandler<TInput, TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>
      : ICommandHandler<Commands.AddAssociationCommand<TInput>, Result<TimestampedId, Errors>>
      where TInput : ValueObjects.AddOneToManyAssociationInput
      where TModel : IModel
      where TAssociationModel : IOneToManyAssociation
      where TAssociateModel : IModel
      where TAggregate : class, IAggregate, IConvertible<TModel>, new()
      where TAssociationAggregate : class, IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
      where TAssociateAggregate : class, IAggregate, IConvertible<TAssociateModel>, new()
    {
        private readonly IModelRepository _repository;
        private readonly Func<Guid, Commands.AddAssociationCommand<TInput>, IAssociationAddedEvent> _newAssociationAddedEvent;

        public AddOneToManyAssociationHandler(
            IModelRepository repository,
            Func<Guid, Commands.AddAssociationCommand<TInput>, IAssociationAddedEvent> newAssociationAddedEvent
            )
        {
            _repository = repository;
            _newAssociationAddedEvent = newAssociationAddedEvent;
        }

        public async Task<Result<TimestampedId, Errors>> Handle(
            Commands.AddAssociationCommand<TInput> command,
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
            Commands.AddAssociationCommand<TInput> command,
            CancellationToken cancellationToken
            )
        {
            return await (
              await session.AddOneToManyAssociation<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>(
                id => _newAssociationAddedEvent(id, command),
                AddAssociationCheck.PARENT_AND_ASSOCIATE,
                cancellationToken
                )
                .ConfigureAwait(false)
              )
                .Bind(async id => await
                    (await session.Save(cancellationToken).ConfigureAwait(false))
                    .Bind(async _ =>
                      await session.TimestampId<TAssociationAggregate>(id, cancellationToken).ConfigureAwait(false)
                      )
                    .ConfigureAwait(false)
                    )
                .ConfigureAwait(false);
        }
    }
}