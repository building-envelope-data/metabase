using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.Commands;
using Infrastructure.Events;
using Infrastructure.Models;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using CancellationToken = System.Threading.CancellationToken;

namespace Infrastructure.Configuration
{
    public abstract class QueryCommandAndEventBusses
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup));

            /* services.AddScoped<MediatR.IMediator, MediatR.Mediator>(); */
            /* services.AddTransient<MediatR.ServiceFactory>(sp => t => sp.GetService(t)); */

            services.AddScoped<IQueryBus, QueryBus>();
            services.AddScoped<ICommandBus, CommandBus>();
            services.AddScoped<IEventBus, EventBus>();
        }

        protected static void AddModelHandlers<TModel, TAggregate, TCreateInput, TCreatedEvent>(
                IServiceCollection services,
                Func<Guid, Commands.CreateCommand<TCreateInput>, ICreatedEvent> newCreatedEvent,
                IEnumerable<Func<Models.ModelRepositorySession, Id, Commands.CreateCommand<TCreateInput>, CancellationToken, Task<Result<Id, Errors>>>> addAssociations,
                Func<Commands.DeleteCommand<TModel>, IDeletedEvent> newDeletedEvent,
                IEnumerable<Func<Models.ModelRepositorySession, TimestampedId, Id, CancellationToken, Task<Result<bool, Errors>>>> removeAssociations
                    )
                where TModel : IModel
                where TAggregate : class, IAggregate, IConvertible<TModel>, new()
                where TCreatedEvent : ICreatedEvent
        {
            AddCreateModelHandler<TCreateInput, TAggregate>(
                    services, newCreatedEvent, addAssociations
                    );
            AddDeleteModelHandler<TModel, TAggregate>(
                services, newDeletedEvent, removeAssociations
                );
            AddGetModelsAtTimestampsHandler<TModel, TAggregate, TCreatedEvent>(services);
            AddGetModelsForTimestampedIdsHandler<TModel, TAggregate>(services);
        }

        protected static void AddCreateModelHandler<TInput, TAggregate>(
                IServiceCollection services,
                Func<Guid, Commands.CreateCommand<TInput>, ICreatedEvent> newEvent,
                IEnumerable<Func<Models.ModelRepositorySession, Id, Commands.CreateCommand<TInput>, CancellationToken, Task<Result<Id, Errors>>>> addAssociations
        )
          where TAggregate : class, IAggregate, new()
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.CreateCommand<TInput>,
              Result<TimestampedId, Errors>
                >,
              Handlers.CreateModelHandler<Commands.CreateCommand<TInput>, TAggregate>>(serviceProvider =>
                  new Handlers.CreateModelHandler<Commands.CreateCommand<TInput>, TAggregate>(
                    serviceProvider.GetRequiredService<Models.IModelRepository>(),
                    newEvent,
                    addAssociations
                    )
                  );
        }

        protected static void AddDeleteModelHandler<TModel, TAggregate>(
                IServiceCollection services,
                Func<Commands.DeleteCommand<TModel>, IDeletedEvent> newEvent,
                IEnumerable<Func<Models.ModelRepositorySession, TimestampedId, Id, CancellationToken, Task<Result<bool, Errors>>>> removeAssociations
        )
          where TModel : IModel
          where TAggregate : class, IAggregate, IConvertible<TModel>, new()
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.DeleteCommand<TModel>,
              Result<TimestampedId, Errors>
                >,
              Handlers.DeleteModelHandler<TModel, TAggregate>>(serviceProvider =>
                  new Handlers.DeleteModelHandler<TModel, TAggregate>(
                    serviceProvider.GetRequiredService<Models.IModelRepository>(),
                    newEvent,
                    removeAssociations
                    )
                  );
        }

        protected static void AddGetModelsAtTimestampsHandler<TModel, TAggregate, TCreatedEvent>(
                IServiceCollection services
                )
          where TModel : IModel
          where TAggregate : class, IAggregate, IConvertible<TModel>, new()
          where TCreatedEvent : ICreatedEvent
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsAtTimestampsQuery<TModel>,
              IEnumerable<Result<IEnumerable<Result<TModel, Errors>>, Errors>>
                >,
              Handlers.GetModelsAtTimestampsHandler<TModel, TAggregate, TCreatedEvent>
                >();
        }

        protected static void AddGetModelsForTimestampedIdsHandler<TModel, TAggregate>(
                IServiceCollection services
                )
                where TModel : IModel
                where TAggregate : class, IAggregate, IConvertible<TModel>, new()
        {
            services.AddScoped<
                MediatR.IRequestHandler<
                Queries.GetModelsForTimestampedIdsQuery<TModel>,
                IEnumerable<Result<TModel, Errors>>
                    >,
                Handlers.GetModelsForTimestampedIdsHandler<TModel, TAggregate>
                    >();
        }

        protected static void AddReflexiveManyToManyAssociationHandlers<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent, TAddInput, TAssociationRemovedEvent>(
                        IServiceCollection services,
                        Func<Guid, Commands.AddAssociationCommand<TAddInput>, IAssociationAddedEvent> newAssociationAddedEvent,
                        Func<Guid, Commands.RemoveAssociationCommand<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>, IAssociationRemovedEvent> newAssociationRemovedEvent
                        )
            where TModel : IModel
            where TAssociationModel : IManyToManyAssociation
            where TAggregate : class, IAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociationAddedEvent : IAssociationAddedEvent
            where TAddInput : ValueObjects.AddManyToManyAssociationInput
            where TAssociationRemovedEvent : IAssociationRemovedEvent
        {
            AddManyToManyAssociationHandlers<TModel, TAssociationModel, TModel, TAggregate, TAssociationAggregate, TAggregate, TAssociationAddedEvent, TAddInput, TAssociationRemovedEvent>(
                    services,
                    newAssociationAddedEvent,
                    newAssociationRemovedEvent
                    );
        }

        protected static void AddManyToManyAssociationHandlersWithAssociationGetters<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent, TAddInput, TAssociationRemovedEvent>(
                        IServiceCollection services,
                        Func<Guid, Commands.AddAssociationCommand<TAddInput>, IAssociationAddedEvent> newAssociationAddedEvent,
                        Func<Guid, Commands.RemoveAssociationCommand<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>, IAssociationRemovedEvent> newAssociationRemovedEvent
                        )
                    where TModel : IModel
                    where TAssociationModel : IManyToManyAssociation
                    where TAssociateModel : IModel
                    where TAggregate : class, IAggregate, IConvertible<TModel>, new()
                    where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
                    where TAssociateAggregate : class, IAggregate, IConvertible<TAssociateModel>, new()
                    where TAssociationAddedEvent : IAssociationAddedEvent
                    where TAddInput : ValueObjects.AddManyToManyAssociationInput
                    where TAssociationRemovedEvent : IAssociationRemovedEvent
        {
            AddManyToManyAssociationHandlers<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent, TAddInput, TAssociationRemovedEvent>(
                    services,
                    newAssociationAddedEvent,
                    newAssociationRemovedEvent
                    );
            AddGetManyToManyAssociationsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(services);
        }

        protected static void AddManyToManyAssociationHandlers<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent, TAddInput, TAssociationRemovedEvent>(
                        IServiceCollection services,
                        Func<Guid, Commands.AddAssociationCommand<TAddInput>, IAssociationAddedEvent> newAssociationAddedEvent,
                        Func<Guid, Commands.RemoveAssociationCommand<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>, IAssociationRemovedEvent> newAssociationRemovedEvent
                        )
                    where TModel : IModel
                    where TAssociationModel : IManyToManyAssociation
                    where TAssociateModel : IModel
                    where TAggregate : class, IAggregate, IConvertible<TModel>, new()
                    where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
                    where TAssociateAggregate : class, IAggregate, IConvertible<TAssociateModel>, new()
                    where TAssociationAddedEvent : IAssociationAddedEvent
                    where TAddInput : ValueObjects.AddManyToManyAssociationInput
                    where TAssociationRemovedEvent : IAssociationRemovedEvent
        {
            AddAddManyToManyAssociationHandler<TAddInput, TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>(services, newAssociationAddedEvent);
            AddRemoveManyToManyAssociationHandler<TAssociationModel, TAssociationAggregate>(services, newAssociationRemovedEvent);
            AddGetHandler<TAssociationModel, TAssociationAggregate>(services);
            AddGetManyToManyAssociatesHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(services);
        }

        protected static void AddAddManyToManyAssociationHandler<TInput, TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>(
                IServiceCollection services,
                Func<Guid, Commands.AddAssociationCommand<TInput>, IAssociationAddedEvent> newEvent
                )
            where TInput : ValueObjects.AddManyToManyAssociationInput
            where TModel : IModel
            where TAssociationModel : IManyToManyAssociation
            where TAssociateModel : IModel
            where TAggregate : class, IAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociateAggregate : class, IAggregate, IConvertible<TAssociateModel>, new()
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.AddAssociationCommand<TInput>,
              Result<TimestampedId, Errors>
                >,
              Handlers.AddManyToManyAssociationHandler<TInput, TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>>(serviceProvider =>
                  new Handlers.AddManyToManyAssociationHandler<TInput, TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>(
                    serviceProvider.GetRequiredService<Models.IModelRepository>(),
                    newEvent
                    )
                  );
        }

        protected static void AddRemoveManyToManyAssociationHandler<TAssociationModel, TAssociationAggregate>(
                IServiceCollection services,
                Func<Guid, Commands.RemoveAssociationCommand<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>, IAssociationRemovedEvent> newEvent
                )
            where TAssociationModel : IManyToManyAssociation
            where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.RemoveAssociationCommand<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>,
              Result<TimestampedId, Errors>
                >,
              Handlers.RemoveManyToManyAssociationHandler<TAssociationModel, TAssociationAggregate>>(serviceProvider =>
                  new Handlers.RemoveManyToManyAssociationHandler<TAssociationModel, TAssociationAggregate>(
                    serviceProvider.GetRequiredService<Models.IModelRepository>(),
                    newEvent
                    )
                  );
        }

        protected static void AddOneToManyAssociationHandlersWithAssociationGetters<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent, TAddInput, TAssociationRemovedEvent>(
                        IServiceCollection services,
                        Func<Guid, Commands.AddAssociationCommand<TAddInput>, IAssociationAddedEvent> newAssociationAddedEvent,
                        Func<Guid, Commands.RemoveAssociationCommand<ValueObjects.RemoveOneToManyAssociationInput<TAssociationModel>>, IAssociationRemovedEvent> newAssociationRemovedEvent
                        )
                    where TModel : IModel
                    where TAssociationModel : IOneToManyAssociation
                    where TAssociateModel : IModel
                    where TAggregate : class, IAggregate, IConvertible<TModel>, new()
                    where TAssociationAggregate : class, IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
                    where TAssociateAggregate : class, IAggregate, IConvertible<TAssociateModel>, new()
                    where TAssociationAddedEvent : IAssociationAddedEvent
                    where TAddInput : ValueObjects.AddOneToManyAssociationInput
                    where TAssociationRemovedEvent : IAssociationRemovedEvent
        {
            AddOneToManyAssociationHandlers<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent, TAddInput, TAssociationRemovedEvent>(
                    services,
                    newAssociationAddedEvent,
                    newAssociationRemovedEvent
                    );
            AddGetOneToManyAssociationsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(services);
        }

        protected static void AddOneToManyAssociationHandlers<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent, TAddInput, TAssociationRemovedEvent>(
                        IServiceCollection services,
                        Func<Guid, Commands.AddAssociationCommand<TAddInput>, IAssociationAddedEvent> newAssociationAddedEvent,
                        Func<Guid, Commands.RemoveAssociationCommand<ValueObjects.RemoveOneToManyAssociationInput<TAssociationModel>>, IAssociationRemovedEvent> newAssociationRemovedEvent
                        )
                    where TModel : IModel
                    where TAssociationModel : IOneToManyAssociation
                    where TAssociateModel : IModel
                    where TAggregate : class, IAggregate, IConvertible<TModel>, new()
                    where TAssociationAggregate : class, IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
                    where TAssociateAggregate : class, IAggregate, IConvertible<TAssociateModel>, new()
                    where TAssociationAddedEvent : IAssociationAddedEvent
                    where TAddInput : ValueObjects.AddOneToManyAssociationInput
                    where TAssociationRemovedEvent : IAssociationRemovedEvent
        {
            AddAddOneToManyAssociationHandler<TAddInput, TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>(services, newAssociationAddedEvent);
            AddRemoveOneToManyAssociationHandler<TAssociationModel, TAssociationAggregate>(services, newAssociationRemovedEvent);
            AddGetHandler<TAssociationModel, TAssociationAggregate>(services);
            AddGetOneToManyAssociatesHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(services);
        }

        protected static void AddAddOneToManyAssociationHandler<TInput, TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>(
                IServiceCollection services,
                Func<Guid, Commands.AddAssociationCommand<TInput>, IAssociationAddedEvent> newEvent
                )
            where TInput : ValueObjects.AddOneToManyAssociationInput
            where TModel : IModel
            where TAssociationModel : IOneToManyAssociation
            where TAssociateModel : IModel
            where TAggregate : class, IAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociateAggregate : class, IAggregate, IConvertible<TAssociateModel>, new()
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.AddAssociationCommand<TInput>,
              Result<TimestampedId, Errors>
                >,
              Handlers.AddOneToManyAssociationHandler<TInput, TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>>(serviceProvider =>
                  new Handlers.AddOneToManyAssociationHandler<TInput, TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>(
                    serviceProvider.GetRequiredService<Models.IModelRepository>(),
                    newEvent
                    )
                  );
        }

        protected static void AddRemoveOneToManyAssociationHandler<TAssociationModel, TAssociationAggregate>(
                IServiceCollection services,
                Func<Guid, Commands.RemoveAssociationCommand<ValueObjects.RemoveOneToManyAssociationInput<TAssociationModel>>, IAssociationRemovedEvent> newEvent
                )
            where TAssociationModel : IOneToManyAssociation
            where TAssociationAggregate : class, IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.RemoveAssociationCommand<ValueObjects.RemoveOneToManyAssociationInput<TAssociationModel>>,
              Result<TimestampedId, Errors>
                >,
              Handlers.RemoveOneToManyAssociationHandler<TAssociationModel, TAssociationAggregate>>(serviceProvider =>
                  new Handlers.RemoveOneToManyAssociationHandler<TAssociationModel, TAssociationAggregate>(
                    serviceProvider.GetRequiredService<Models.IModelRepository>(),
                    newEvent
                    )
                  );
        }

        protected static void AddGetHandler<TModel, TAggregate>(IServiceCollection services)
            where TModel : IModel
            where TAggregate : class, IAggregate, IConvertible<TModel>, new()
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIdsQuery<TModel>,
              IEnumerable<Result<TModel, Errors>>
                >,
              Handlers.GetModelsForTimestampedIdsHandler<TModel, TAggregate>
                >();
        }

        protected static void AddGetManyToManyAssociatesHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(IServiceCollection services)
            where TModel : IModel
            where TAssociationModel : IManyToManyAssociation
            where TAssociateModel : IModel
            where TAggregate : class, IAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociateAggregate : class, IAggregate, IConvertible<TAssociateModel>, new()
            where TAssociationAddedEvent : IAssociationAddedEvent
        {
            AddGetForwardManyToManyAssociatesHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(services);
            AddGetBackwardManyToManyAssociatesHandler<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate, TAssociationAddedEvent>(services);
        }

        protected static void AddGetForwardManyToManyAssociatesHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(IServiceCollection services)
            where TModel : IModel
            where TAssociationModel : IManyToManyAssociation
            where TAssociateModel : IModel
            where TAggregate : class, IAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociateAggregate : class, IAggregate, IConvertible<TAssociateModel>, new()
            where TAssociationAddedEvent : IAssociationAddedEvent
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetForwardManyToManyAssociatesOfModelsQuery<TModel, TAssociationModel, TAssociateModel>,
              IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>
                >,
              Handlers.GetForwardManyToManyAssociatesOfModelsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>
                >();
        }

        protected static void AddGetBackwardManyToManyAssociatesHandler<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate, TAssociationAddedEvent>(IServiceCollection services)
            where TAssociateModel : IModel
            where TAssociationModel : IManyToManyAssociation
            where TModel : IModel
            where TAssociateAggregate : class, IAggregate, IConvertible<TAssociateModel>, new()
            where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAggregate : class, IAggregate, IConvertible<TModel>, new()
            where TAssociationAddedEvent : IAssociationAddedEvent
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetBackwardManyToManyAssociatesOfModelsQuery<TAssociateModel, TAssociationModel, TModel>,
              IEnumerable<Result<IEnumerable<Result<TModel, Errors>>, Errors>>
                >,
              Handlers.GetBackwardManyToManyAssociatesOfModelsHandler<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate, TAssociationAddedEvent>
                >();
        }

        protected static void AddGetManyToManyAssociationsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(IServiceCollection services)
    where TModel : IModel
    where TAssociationModel : IManyToManyAssociation
    where TAssociateModel : IModel
    where TAggregate : class, IAggregate, IConvertible<TModel>, new()
    where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
    where TAssociateAggregate : class, IAggregate, IConvertible<TAssociateModel>, new()
    where TAssociationAddedEvent : IAssociationAddedEvent
        {
            AddGetForwardManyToManyAssociationsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(services);
            AddGetBackwardManyToManyAssociationsHandler<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(services);
        }

        protected static void AddGetForwardManyToManyAssociationsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(IServiceCollection services)
    where TModel : IModel
    where TAssociationModel : IManyToManyAssociation
    where TAggregate : class, IAggregate, IConvertible<TModel>, new()
    where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
    where TAssociationAddedEvent : IAssociationAddedEvent
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetForwardManyToManyAssociationsOfModelsQuery<TModel, TAssociationModel>,
              IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>
                >,
              Handlers.GetForwardManyToManyAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>
                >();
        }

        protected static void AddGetBackwardManyToManyAssociationsHandler<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(IServiceCollection services)
    where TAssociateModel : IModel
    where TAssociationModel : IManyToManyAssociation
    where TAssociateAggregate : class, IAggregate, IConvertible<TAssociateModel>, new()
    where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
    where TAssociationAddedEvent : IAssociationAddedEvent
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetBackwardManyToManyAssociationsOfModelsQuery<TAssociateModel, TAssociationModel>,
              IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>
                >,
              Handlers.GetBackwardManyToManyAssociationsOfModelsHandler<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>
                >();
        }

        protected static void AddGetOneToManyAssociatesHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(IServiceCollection services)
            where TModel : IModel
            where TAssociationModel : IOneToManyAssociation
            where TAssociateModel : IModel
            where TAggregate : class, IAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociateAggregate : class, IAggregate, IConvertible<TAssociateModel>, new()
            where TAssociationAddedEvent : IAssociationAddedEvent
        {
            AddGetForwardOneToManyAssociatesHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(services);
            AddGetBackwardOneToManyAssociateHandler<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate, TAssociationAddedEvent>(services);
        }

        protected static void AddGetForwardOneToManyAssociatesHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(IServiceCollection services)
            where TModel : IModel
            where TAssociationModel : IOneToManyAssociation
            where TAssociateModel : IModel
            where TAggregate : class, IAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociateAggregate : class, IAggregate, IConvertible<TAssociateModel>, new()
            where TAssociationAddedEvent : IAssociationAddedEvent
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetForwardOneToManyAssociatesOfModelsQuery<TModel, TAssociationModel, TAssociateModel>,
              IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>
                >,
              Handlers.GetForwardOneToManyAssociatesOfModelsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>
                >();
        }

        protected static void AddGetBackwardOneToManyAssociateHandler<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate, TAssociationAddedEvent>(IServiceCollection services)
            where TAssociateModel : IModel
            where TAssociationModel : IOneToManyAssociation
            where TModel : IModel
            where TAssociateAggregate : class, IAggregate, IConvertible<TAssociateModel>, new()
            where TAssociationAggregate : class, IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAggregate : class, IAggregate, IConvertible<TModel>, new()
            where TAssociationAddedEvent : IAssociationAddedEvent
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetBackwardOneToManyAssociateOfModelsQuery<TAssociateModel, TAssociationModel, TModel>,
              IEnumerable<Result<TModel, Errors>>
                >,
              Handlers.GetBackwardOneToManyAssociateOfModelsHandler<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate, TAssociationAddedEvent>
                >();
        }

        protected static void AddGetOneToManyAssociationsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(IServiceCollection services)
    where TModel : IModel
    where TAssociationModel : IOneToManyAssociation
    where TAssociateModel : IModel
    where TAggregate : class, IAggregate, IConvertible<TModel>, new()
    where TAssociationAggregate : class, IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
    where TAssociateAggregate : class, IAggregate, IConvertible<TAssociateModel>, new()
    where TAssociationAddedEvent : IAssociationAddedEvent
        {
            AddGetForwardOneToManyAssociationsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(services);
            AddGetBackwardOneToManyAssociationHandler<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(services);
        }

        protected static void AddGetForwardOneToManyAssociationsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(IServiceCollection services)
    where TModel : IModel
    where TAssociationModel : IOneToManyAssociation
    where TAggregate : class, IAggregate, IConvertible<TModel>, new()
    where TAssociationAggregate : class, IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
    where TAssociationAddedEvent : IAssociationAddedEvent
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetForwardOneToManyAssociationsOfModelsQuery<TModel, TAssociationModel>,
              IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>
                >,
              Handlers.GetForwardOneToManyAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>
                >();
        }

        protected static void AddGetBackwardOneToManyAssociationHandler<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(IServiceCollection services)
    where TAssociateModel : IModel
    where TAssociationModel : IOneToManyAssociation
    where TAssociateAggregate : class, IAggregate, IConvertible<TAssociateModel>, new()
    where TAssociationAggregate : class, IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
    where TAssociationAddedEvent : IAssociationAddedEvent
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetBackwardOneToManyAssociationOfModelsQuery<TAssociateModel, TAssociationModel>,
              IEnumerable<Result<TAssociationModel, Errors>>
                >,
              Handlers.GetBackwardOneToManyAssociationOfModelsHandler<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>
                >();
        }
    }
}