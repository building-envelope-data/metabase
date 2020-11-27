using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using GreenDonut;
using HotChocolate;
using Infrastructure.Commands;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.GraphQl
{
    public abstract class Mutation
    {
        protected readonly ICommandBus _commandBus;
        protected readonly IQueryBus _queryBus;

        protected Mutation(ICommandBus commandBus, IQueryBus queryBus)
        {
            _commandBus = commandBus;
            _queryBus = queryBus;
        }

        protected async Task<TPayload> Create<TInput, TValidatedInput, TPayload>(
            TInput input,
            Func<TInput, IReadOnlyList<object>, Result<TValidatedInput, Errors>> validateInput,
            Func<TimestampedId, TPayload> newPayload
            )
        {
            var command = ResultHelpers.HandleFailure(
                  validateInput(input, Array.Empty<object>()) // TODO What is the proper path for variables?
                  .Bind(validatedInput =>
                    Commands.CreateCommand<TValidatedInput>.From(
                      input: validatedInput,
                      creatorId: Id.New() // TODO Use current user!
                      )
                    )
                  );
            var timestampedId = ResultHelpers.HandleFailure(
              await _commandBus
              .Send<
              Commands.CreateCommand<TValidatedInput>,
              Result<TimestampedId, Errors>
                >(command).ConfigureAwait(false)
                );
            return newPayload(timestampedId);
        }

        protected async Task<TPayload> Delete<TModel, TPayload>(
            Id id,
            Timestamp timestamp,
            Func<TimestampedId, TPayload> newPayload
            )
        {
            var command = ResultHelpers.HandleFailure(
                  TimestampedId.From(id, timestamp, Array.Empty<object>()) // TODO What is the proper path for variables?
                  .Bind(timestampedId =>
                    Commands.DeleteCommand<TModel>.From(
                      timestampedId: timestampedId,
                      creatorId: Id.New() // TODO Use current user!
                      )
                    )
                  );
            var timestampedId = ResultHelpers.HandleFailure(
              await _commandBus
              .Send<
              Commands.DeleteCommand<TModel>,
              Result<TimestampedId, Errors>
                >(command).ConfigureAwait(false)
                );
            return newPayload(timestampedId);
        }

        protected async Task<TPayload> AddAssociation<TInput, TValidatedInput, TAssociation, TPayload>(
            TInput input,
            Func<TInput, IReadOnlyList<object>, Result<TValidatedInput, Errors>> validateInput,
            Func<TAssociation, TPayload> newPayload,
            IDataLoader<TimestampedId, TAssociation> associationLoader
        )
        {
            var command = ResultHelpers.HandleFailure(
                  validateInput(input, Array.Empty<object>()) // TODO What is the proper path for variables?
                  .Bind(validatedInput =>
                    Commands.AddAssociationCommand<TValidatedInput>.From(
                      input: validatedInput,
                      creatorId: Id.New() // TODO Use current user!
                      )
                    )
                  );
            var timestampedId = ResultHelpers.HandleFailure(
                await _commandBus
                .Send<
                Commands.AddAssociationCommand<TValidatedInput>,
                Result<TimestampedId, Errors>
                >(command).ConfigureAwait(false)
                );
            return newPayload(
                  await associationLoader.LoadAsync(timestampedId).ConfigureAwait(false)
                );
        }

        protected async Task<TPayload> RemoveManyToManyAssociation<TAssociationModel, TInput, TPayload>(
            TInput input,
            Func<TInput, IReadOnlyList<object>, Result<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>, Errors>> validateInput,
            Func<Id, Id, Timestamp, TPayload> newPayload
        )
        {
            var command = ResultHelpers.HandleFailure(
                  validateInput(input, Array.Empty<object>()) // TODO What is the proper path for variables?
                  .Bind(validatedInput =>
                    Commands.RemoveAssociationCommand<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>.From(
                      input: validatedInput,
                      creatorId: Id.New() // TODO Use current user!
                      )
                    )
                  );
            var timestampedId = ResultHelpers.HandleFailure(
                await _commandBus
                .Send<
                Commands.RemoveAssociationCommand<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>,
                Result<TimestampedId, Errors>
                >(command).ConfigureAwait(false)
                );
            return newPayload(
                command.Input.ParentId,
                command.Input.AssociateId,
                timestampedId.Timestamp
                );
        }

        protected async Task<TPayload> RemoveOneToManyAssociation<TAssociationModel, TInput, TPayload>(
            TInput input,
            Func<TInput, IReadOnlyList<object>, Result<ValueObjects.RemoveOneToManyAssociationInput<TAssociationModel>, Errors>> validateInput,
            Func<Id, Timestamp, TPayload> newPayload
        )
        {
            var command = ResultHelpers.HandleFailure(
                  validateInput(input, Array.Empty<object>()) // TODO What is the proper path for variables?
                  .Bind(validatedInput =>
                    Commands.RemoveAssociationCommand<ValueObjects.RemoveOneToManyAssociationInput<TAssociationModel>>.From(
                      input: validatedInput,
                      creatorId: Id.New() // TODO Use current user!
                      )
                    )
                  );
            var timestampedId = ResultHelpers.HandleFailure(
                await _commandBus
                .Send<
                Commands.RemoveAssociationCommand<ValueObjects.RemoveOneToManyAssociationInput<TAssociationModel>>,
                Result<TimestampedId, Errors>
                >(command).ConfigureAwait(false)
                );
            return newPayload(
                command.Input.AssociateId,
                timestampedId.Timestamp
                );
        }
    }
}