using NSwag.Generation.Processors.Security;
using IAggregateRepository = Icon.Infrastructure.Aggregate.IAggregateRepository;
using IEventSourcedAggregate = Icon.Infrastructure.Aggregate.IEventSourcedAggregate;
using IdentityServer4.AccessTokenValidation;
using Icon.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSwag.AspNetCore;
using Microsoft.OpenApi;
using NSwag;
using IdentityServer4.AspNetIdentity;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Command = Icon.Infrastructure.Command;
using Query = Icon.Infrastructure.Query;
using Events = Icon.Events;
using Models = Icon.Models;
using System.Threading.Tasks;
using System;
using WebPWrecover.Services;
using IdentityServer4.Validation;
using Queries = Icon.Queries;
using Commands = Icon.Commands;
using Handlers = Icon.Handlers;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;
using MediatR;
using System.Linq.Expressions;
using Marten; // IsOneOf

namespace Icon.Configuration
{
    public sealed class QueryCommandAndEventBusses
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup));

            /* services.AddScoped<MediatR.IMediator, MediatR.Mediator>(); */
            /* services.AddTransient<MediatR.ServiceFactory>(sp => t => sp.GetService(t)); */

            services.AddScoped<Query.IQueryBus, Query.QueryBus>();
            services.AddScoped<Command.ICommandBus, Command.CommandBus>();
            services.AddScoped<Events.IEventBus, Events.EventBus>();

            AddModelOfUnknownTypeHandlers(services);

            AddComponentHandlers(services);
            AddDatabaseHandlers(services);
            AddInstitutionHandlers(services);
            AddMethodHandlers(services);
            AddPersonHandlers(services);
            AddStakeholderHandlers(services);
            AddStandardHandlers(services);

            AddComponentConcretizationHandlers(services);
            AddComponentPartHandlers(services);
            AddComponentVariantHandlers(services);
            AddComponentVersionHandlers(services);

            AddComponentManufacturerHandlers(services);
            AddInstitutionRepresentativeHandlers(services);
            AddMethodDeveloperHandlers(services);
            AddPersonAffiliationHandlers(services);

            // TODO Shall we broadcast events?
            /* services.AddScoped<INotificationHandler<ClientCreated>, ClientsEventHandler>(); */
            /* services.AddScoped<IRequestHandler<CreateClient, Unit>, ClientsCommandHandler>(); */
            /* services.AddScoped<IRequestHandler<GetClientView, ClientView>, ClientsQueryHandler>(); */
        }

        private static void AddModelOfUnknownTypeHandlers(IServiceCollection services)
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<Models.IModel>,
              IEnumerable<Result<Models.IModel, Errors>>
                >,
              Handlers.GetModelsOfUnknownTypeForTimestampedIdsHandler
                >();
        }

        private static void AddComponentHandlers(IServiceCollection services)
        {
            AddModelHandlers<Models.Component, Aggregates.ComponentAggregate, ValueObjects.CreateComponentInput, Events.ComponentCreated>(
                    services,
                    Events.ComponentCreated.From,
                    Events.ComponentDeleted.From
                    );
        }

        private static void AddDatabaseHandlers(IServiceCollection services)
        {
            AddModelHandlers<Models.Database, Aggregates.DatabaseAggregate, ValueObjects.CreateDatabaseInput, Events.DatabaseCreated>(
                    services,
                    Events.DatabaseCreated.From,
                    Events.DatabaseDeleted.From
                    );
        }

        private static void AddInstitutionHandlers(IServiceCollection services)
        {
            AddModelHandlers<Models.Institution, Aggregates.InstitutionAggregate, ValueObjects.CreateInstitutionInput, Events.InstitutionCreated>(
                    services,
                    Events.InstitutionCreated.From,
                    Events.InstitutionDeleted.From
                    );
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetOneToManyAssociatesOfModels<Models.Institution, Models.Database>,
              IEnumerable<Result<IEnumerable<Result<Models.Database, Errors>>, Errors>>
                                >,
              Handlers.GetDatabasesOperatedByInstitutionsHandler
                                >();
            /* AddGetOneToManyAssociatesHandler<Models.Institution, Models.Database, Aggregates.DatabaseAggregate, Events.DatabaseCreated>( */
            /*         services, */
            /*         modelGuids => (createdEvent => createdEvent.InstitutionId.IsOneOf(modelGuids)), */
            /*         createdEvent => new Handlers.GetOneToManyAssociatesOfModelsHandler<Models.Institution, Models.Database, Aggregates.DatabaseAggregate, Events.DatabaseCreated>.Select { ModelId = createdEvent.InstitutionId, AssociateId = createdEvent.AggregateId } */
            /*         ); */
        }

        private static void AddMethodHandlers(IServiceCollection services)
        {
            AddModelHandlers<Models.Method, Aggregates.MethodAggregate, ValueObjects.CreateMethodInput, Events.MethodCreated>(
                    services,
                    Events.MethodCreated.From,
                    Events.MethodDeleted.From
                    );
        }

        private static void AddPersonHandlers(IServiceCollection services)
        {
            AddModelHandlers<Models.Person, Aggregates.PersonAggregate, ValueObjects.CreatePersonInput, Events.PersonCreated>(
                    services,
                    Events.PersonCreated.From,
                    Events.PersonDeleted.From
                    );
        }

        private static void AddStakeholderHandlers(IServiceCollection services)
        {
            // Get
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<Models.Stakeholder>,
              IEnumerable<Result<Models.Stakeholder, Errors>>
                >,
              Handlers.GetStakeholdersHandler
                >();
        }

        private static void AddStandardHandlers(IServiceCollection services)
        {
            AddModelHandlers<Models.Standard, Aggregates.StandardAggregate, ValueObjects.CreateStandardInput, Events.StandardCreated>(
                    services,
                    Events.StandardCreated.From,
                    Events.StandardDeleted.From
                    );
        }

        private static void AddModelHandlers<TModel, TAggregate, TCreateInput, TCreatedEvent>(
                IServiceCollection services,
                Func<Guid, Commands.Create<TCreateInput>, Events.ICreatedEvent> newCreatedEvent,
                Func<Commands.Delete<TModel>, Events.IDeletedEvent> newDeletedEvent
                    )
                where TModel : Models.IModel
                where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
                where TCreatedEvent : Events.ICreatedEvent
        {
            AddCreateHandler<TCreateInput, TAggregate>(
                    services, newCreatedEvent
                    );
            AddDeleteHandler<TModel, TAggregate>(
                services, newDeletedEvent
                );
            AddGetModelsForTimestampedIdsHandler<TModel, TAggregate, TCreatedEvent>(services);
            AddGetModelHandler<TModel, TAggregate>(services);
        }

        private static void AddCreateHandler<TInput, TAggregate>(
                IServiceCollection services,
                Func<Guid, Commands.Create<TInput>, Events.ICreatedEvent> newEvent
        )
          where TAggregate : class, IEventSourcedAggregate, new()
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.Create<TInput>,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.CreateModelHandler<Commands.Create<TInput>, TAggregate>>(serviceProvider =>
                  new Handlers.CreateModelHandler<Commands.Create<TInput>, TAggregate>(
                    serviceProvider.GetRequiredService<IAggregateRepository>(),
                    newEvent
                    )
                  );
        }

        private static void AddDeleteHandler<TModel, TAggregate>(
                IServiceCollection services,
                Func<Commands.Delete<TModel>, Events.IDeletedEvent> newEvent
        )
          where TAggregate : class, IEventSourcedAggregate, new()
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.Delete<TModel>,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.DeleteModelHandler<TModel, TAggregate>>(serviceProvider =>
                  new Handlers.DeleteModelHandler<TModel, TAggregate>(
                    serviceProvider.GetRequiredService<IAggregateRepository>(),
                    newEvent
                    )
                  );
        }

        private static void AddGetModelsForTimestampedIdsHandler<TModel, TAggregate, TCreatedEvent>(
                IServiceCollection services
                )
                where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
                where TCreatedEvent : Events.ICreatedEvent
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsAtTimestamps<TModel>,
              IEnumerable<Result<IEnumerable<Result<TModel, Errors>>, Errors>>
                >,
              Handlers.GetModelsAtTimestampsHandler<TModel, TAggregate, TCreatedEvent>
                >();
        }

        private static void AddGetModelHandler<TModel, TAggregate>(
                IServiceCollection services
                )
                where TModel : Models.IModel
                where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
        {
            services.AddScoped<
                MediatR.IRequestHandler<
                Queries.GetModelsForTimestampedIds<TModel>,
                IEnumerable<Result<TModel, Errors>>
                    >,
                Handlers.GetModelsForTimestampedIdsHandler<TModel, TAggregate>
                    >();
        }

        private static void AddComponentConcretizationHandlers(IServiceCollection services)
        {
            AddReflexiveComponentAssociationHandlers<
                Models.ComponentConcretization,
                Aggregates.ComponentConcretizationAggregate,
                Events.ComponentConcretizationAdded,
                ValueObjects.AddComponentConcretizationInput,
                Events.ComponentConcretizationRemoved
                >(
                    services,
                    Events.ComponentConcretizationAdded.From,
                    Events.ComponentConcretizationRemoved.From
                    );
        }

        private static void AddComponentPartHandlers(IServiceCollection services)
        {
            AddReflexiveComponentAssociationHandlers<
                Models.ComponentPart,
                Aggregates.ComponentPartAggregate,
                Events.ComponentPartAdded,
                ValueObjects.AddComponentPartInput,
                Events.ComponentPartRemoved
                >(
                    services,
                    Events.ComponentPartAdded.From,
                    Events.ComponentPartRemoved.From
                    );
        }

        private static void AddComponentVariantHandlers(IServiceCollection services)
        {
            AddReflexiveComponentAssociationHandlers<
                Models.ComponentVariant,
                Aggregates.ComponentVariantAggregate,
                Events.ComponentVariantAdded,
                ValueObjects.AddComponentVariantInput,
                Events.ComponentVariantRemoved
                >(
                    services,
                    Events.ComponentVariantAdded.From,
                    Events.ComponentVariantRemoved.From
                    );
        }

        private static void AddComponentVersionHandlers(IServiceCollection services)
        {
            AddReflexiveComponentAssociationHandlers<
                Models.ComponentVersion,
                Aggregates.ComponentVersionAggregate,
                Events.ComponentVersionAdded,
                ValueObjects.AddComponentVersionInput,
                Events.ComponentVersionRemoved
                >(
                    services,
                    Events.ComponentVersionAdded.From,
                    Events.ComponentVersionRemoved.From
                    );
        }

        private static void AddReflexiveComponentAssociationHandlers<TAssociationModel, TAssociationAggregate, TAddedEvent, TAddInput, TRemovedEvent>(
                        IServiceCollection services,
                        Func<Guid, Commands.Add<TAddInput>, Events.IAddedEvent> newAddedEvent,
                        Func<Guid, Commands.Remove<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>, Events.IRemovedEvent> newRemovedEvent
                        )
            where TAssociationModel : Models.IManyToManyAssociation
            where TAssociationAggregate : class, IEventSourcedAggregate, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAddedEvent : Events.IAddedEvent
            where TAddInput : ValueObjects.AddManyToManyAssociationInput
            where TRemovedEvent : Events.IRemovedEvent
        {
            AddReflexiveAssociationHandlers<Models.Component, TAssociationModel, Aggregates.ComponentAggregate, TAssociationAggregate, TAddedEvent, TAddInput, TRemovedEvent>(
                    services,
                    newAddedEvent,
                    newRemovedEvent
                    );
        }

        private static void AddComponentManufacturerHandlers(IServiceCollection services)
        {
            AddAssociationHandlersWithAssociationGetters<
                Models.Component,
                Models.ComponentManufacturer,
                Models.Institution,
                Aggregates.ComponentAggregate,
                Aggregates.ComponentManufacturerAggregate,
                Aggregates.InstitutionAggregate,
                Events.ComponentManufacturerAdded,
                ValueObjects.AddComponentManufacturerInput,
                Events.ComponentManufacturerRemoved
                    >(
                            services,
                            Events.ComponentManufacturerAdded.From,
                            Events.ComponentManufacturerRemoved.From
                     );
        }

        private static void AddInstitutionRepresentativeHandlers(IServiceCollection services)
        {
            AddAssociationHandlersWithAssociationGetters<
                Models.Institution,
                Models.InstitutionRepresentative,
                Models.User,
                Aggregates.InstitutionAggregate,
                Aggregates.InstitutionRepresentativeAggregate,
                Aggregates.UserAggregate,
                Events.InstitutionRepresentativeAdded,
                ValueObjects.AddInstitutionRepresentativeInput,
                Events.InstitutionRepresentativeRemoved
                    >(
                            services,
                            Events.InstitutionRepresentativeAdded.From,
                            Events.InstitutionRepresentativeRemoved.From
                     );
        }

        private static void AddMethodDeveloperHandlers(IServiceCollection services)
        {
            // Add
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.Add<ValueObjects.AddMethodDeveloperInput>,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.AddMethodDeveloperHandler>();

            // TODO Remove

            // Get
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<Models.MethodDeveloper>,
              IEnumerable<Result<Models.MethodDeveloper, Errors>>
                >,
              Handlers.GetMethodDevelopersHandler
                >();

            // Get associates
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetForwardManyToManyAssociatesOfModels<Models.Method, Models.MethodDeveloper, Models.Stakeholder>,
              IEnumerable<Result<IEnumerable<Result<Models.Stakeholder, Errors>>, Errors>>
                >,
              Handlers.GetDevelopersOfMethodsHandler
                >();
            AddGetBackwardManyToManyAssociatesHandler<Models.Person, Models.MethodDeveloper, Models.Method, Aggregates.PersonAggregate, Aggregates.PersonMethodDeveloperAggregate, Aggregates.MethodAggregate, Events.PersonMethodDeveloperAdded>(services);
            AddGetBackwardManyToManyAssociatesHandler<Models.Institution, Models.MethodDeveloper, Models.Method, Aggregates.InstitutionAggregate, Aggregates.InstitutionMethodDeveloperAggregate, Aggregates.MethodAggregate, Events.InstitutionMethodDeveloperAdded>(services);
        }

        private static void AddPersonAffiliationHandlers(IServiceCollection services)
        {
            AddAssociationHandlers<
                Models.Person,
                Models.PersonAffiliation,
                Models.Institution,
                Aggregates.PersonAggregate,
                Aggregates.PersonAffiliationAggregate,
                Aggregates.InstitutionAggregate,
                Events.PersonAffiliationAdded,
                ValueObjects.AddPersonAffiliationInput,
                Events.PersonAffiliationRemoved
                    >(
                            services,
                            Events.PersonAffiliationAdded.From,
                            Events.PersonAffiliationRemoved.From
                     );
        }

        private static void AddReflexiveAssociationHandlers<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAddedEvent, TAddInput, TRemovedEvent>(
                        IServiceCollection services,
                        Func<Guid, Commands.Add<TAddInput>, Events.IAddedEvent> newAddedEvent,
                        Func<Guid, Commands.Remove<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>, Events.IRemovedEvent> newRemovedEvent
                        )
            where TModel : Models.IModel
            where TAssociationModel : Models.IManyToManyAssociation
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, IEventSourcedAggregate, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAddedEvent : Events.IAddedEvent
            where TAddInput : ValueObjects.AddManyToManyAssociationInput
            where TRemovedEvent : Events.IRemovedEvent
        {
            AddAssociationHandlers<TModel, TAssociationModel, TModel, TAggregate, TAssociationAggregate, TAggregate, TAddedEvent, TAddInput, TRemovedEvent>(
                    services,
                    newAddedEvent,
                    newRemovedEvent
                    );
        }

        private static void AddAssociationHandlersWithAssociationGetters<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAddedEvent, TAddInput, TRemovedEvent>(
                        IServiceCollection services,
                        Func<Guid, Commands.Add<TAddInput>, Events.IAddedEvent> newAddedEvent,
                        Func<Guid, Commands.Remove<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>, Events.IRemovedEvent> newRemovedEvent
                        )
                    where TModel : Models.IModel
                    where TAssociationModel : Models.IManyToManyAssociation
                    where TAssociateModel : Models.IModel
                    where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
                    where TAssociationAggregate : class, IEventSourcedAggregate, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
                    where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
                    where TAddedEvent : Events.IAddedEvent
                    where TAddInput : ValueObjects.AddManyToManyAssociationInput
                    where TRemovedEvent : Events.IRemovedEvent
        {
            AddAssociationHandlers<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAddedEvent, TAddInput, TRemovedEvent>(
                    services,
                    newAddedEvent,
                    newRemovedEvent
                    );
            AddGetAssociationsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAddedEvent>(services);
        }

        private static void AddAssociationHandlers<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAddedEvent, TAddInput, TRemovedEvent>(
                        IServiceCollection services,
                        Func<Guid, Commands.Add<TAddInput>, Events.IAddedEvent> newAddedEvent,
                        Func<Guid, Commands.Remove<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>, Events.IRemovedEvent> newRemovedEvent
                        )
                    where TModel : Models.IModel
                    where TAssociationModel : Models.IManyToManyAssociation
                    where TAssociateModel : Models.IModel
                    where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
                    where TAssociationAggregate : class, IEventSourcedAggregate, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
                    where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
                    where TAddedEvent : Events.IAddedEvent
                    where TAddInput : ValueObjects.AddManyToManyAssociationInput
                    where TRemovedEvent : Events.IRemovedEvent
        {
            AddAddManyToManyAssociationHandler<TAddInput, TAggregate, TAssociationAggregate, TAssociateAggregate>(services, newAddedEvent);
            AddRemoveManyToManyAssociationHandler<TAssociationModel, TAssociationAggregate>(services, newRemovedEvent);
            AddGetHandler<TAssociationModel, TAssociationAggregate>(services);
            AddGetManyToManyAssociatesHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAddedEvent>(services);
        }

        private static void AddAddManyToManyAssociationHandler<TInput, TAggregate, TAssociationAggregate, TAssociateAggregate>(
                IServiceCollection services,
                Func<Guid, Commands.Add<TInput>, Events.IAddedEvent> newEvent
                )
            where TInput : ValueObjects.AddManyToManyAssociationInput
            where TAggregate : class, IEventSourcedAggregate, new()
            where TAssociationAggregate : class, IEventSourcedAggregate, Aggregates.IManyToManyAssociationAggregate, new()
            where TAssociateAggregate : class, IEventSourcedAggregate, new()
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.Add<TInput>,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.AddManyToManyAssociationHandler<TInput, TAggregate, TAssociationAggregate, TAssociateAggregate>>(serviceProvider =>
                  new Handlers.AddManyToManyAssociationHandler<TInput, TAggregate, TAssociationAggregate, TAssociateAggregate>(
                    serviceProvider.GetRequiredService<IAggregateRepository>(),
                    newEvent
                    )
                  );
        }

        private static void AddRemoveManyToManyAssociationHandler<TAssociationModel, TAssociationAggregate>(
                IServiceCollection services,
                Func<Guid, Commands.Remove<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>, Events.IRemovedEvent> newEvent
                )
            where TAssociationAggregate : class, IEventSourcedAggregate, Aggregates.IManyToManyAssociationAggregate, new()
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.Remove<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.RemoveManyToManyAssociationHandler<TAssociationModel, TAssociationAggregate>>(serviceProvider =>
                  new Handlers.RemoveManyToManyAssociationHandler<TAssociationModel, TAssociationAggregate>(
                    serviceProvider.GetRequiredService<IAggregateRepository>(),
                    newEvent
                    )
                  );
        }

        private static void AddGetHandler<TModel, TAggregate>(IServiceCollection services)
            where TModel : Models.IModel
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<TModel>,
              IEnumerable<Result<TModel, Errors>>
                >,
              Handlers.GetModelsForTimestampedIdsHandler<TModel, TAggregate>
                >();
        }

        /* private static void AddGetOneToManyAssociatesHandler<TModel, TAssociateModel, TAssociateAggregate, TCreatedEvent>( */
        /*     IServiceCollection services, */
        /*                 Func<Guid[], Expression<Func<TCreatedEvent, bool>>> where, */
        /*                 Expression<Func<TCreatedEvent, Handlers.GetOneToManyAssociatesOfModelsHandler<TModel, TAssociateModel, TAssociateAggregate, TCreatedEvent>.Select>> select */
        /*     ) */
        /*     where TModel : Models.IModel */
        /*     where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new() */
        /*     where TCreatedEvent : Events.ICreatedEvent */
        /* { */
        /*     services.AddScoped< */
        /*       MediatR.IRequestHandler< */
        /*       Queries.GetOneToManyAssociatesOfModels<TModel, TAssociateModel>, */
        /*       IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>> */
        /*         >, */
        /*       Handlers.GetOneToManyAssociatesOfModelsHandler<TModel, TAssociateModel, TAssociateAggregate, TCreatedEvent> */
        /*         >(serviceProvider => */
        /*           new Handlers.GetOneToManyAssociatesOfModelsHandler<TModel, TAssociateModel, TAssociateAggregate, TCreatedEvent>( */
        /*             where, */
        /*                                 select, */
        /*             serviceProvider.GetRequiredService<IAggregateRepository>() */
        /*             ) */
        /*          ); */
        /* } */

        private static void AddGetManyToManyAssociatesHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAddedEvent>(IServiceCollection services)
            where TModel : Models.IModel
            where TAssociationModel : Models.IManyToManyAssociation
            where TAssociateModel : Models.IModel
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, IEventSourcedAggregate, IConvertible<TAssociationModel>, Aggregates.IManyToManyAssociationAggregate, new()
            where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
            where TAddedEvent : Events.IAddedEvent
        {
            AddGetForwardManyToManyAssociatesHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAddedEvent>(services);
            AddGetBackwardManyToManyAssociatesHandler<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate, TAddedEvent>(services);
        }

        private static void AddGetForwardManyToManyAssociatesHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAddedEvent>(IServiceCollection services)
            where TModel : Models.IModel
            where TAssociationModel : Models.IManyToManyAssociation
            where TAssociateModel : Models.IModel
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, IEventSourcedAggregate, IConvertible<TAssociationModel>, Aggregates.IManyToManyAssociationAggregate, new()
            where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
            where TAddedEvent : Events.IAddedEvent
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetForwardManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>,
              IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>
                >,
              Handlers.GetForwardManyToManyAssociatesOfModelsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAddedEvent>
                >();
        }

        private static void AddGetBackwardManyToManyAssociatesHandler<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate, TAddedEvent>(IServiceCollection services)
            where TAssociateModel : Models.IModel
            where TAssociationModel : Models.IManyToManyAssociation
            where TModel : Models.IModel
            where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
            where TAssociationAggregate : class, IEventSourcedAggregate, IConvertible<TAssociationModel>, Aggregates.IManyToManyAssociationAggregate, new()
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAddedEvent : Events.IAddedEvent
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetBackwardManyToManyAssociatesOfModels<TAssociateModel, TAssociationModel, TModel>,
              IEnumerable<Result<IEnumerable<Result<TModel, Errors>>, Errors>>
                >,
              Handlers.GetBackwardManyToManyAssociatesOfModelsHandler<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate, TAddedEvent>
                >();
        }

        private static void AddGetAssociationsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAddedEvent>(IServiceCollection services)
    where TModel : Models.IModel
    where TAssociateModel : Models.IModel
    where TAssociationModel : Models.IManyToManyAssociation
    where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
    where TAssociationAggregate : class, IEventSourcedAggregate, IConvertible<TAssociationModel>, new()
    where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
    where TAddedEvent : Events.IAddedEvent
        {
            AddGetForwardManyToManyAssociationsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAddedEvent>(services);
            AddGetBackwardManyToManyAssociationsHandler<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAddedEvent>(services);
        }

        private static void AddGetForwardManyToManyAssociationsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAddedEvent>(IServiceCollection services)
    where TModel : Models.IModel
    where TAssociationModel : Models.IManyToManyAssociation
    where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
    where TAssociationAggregate : class, IEventSourcedAggregate, IConvertible<TAssociationModel>, new()
    where TAddedEvent : Events.IAddedEvent
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetForwardManyToManyAssociationsOfModels<TModel, TAssociationModel>,
              IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>
                >,
              Handlers.GetForwardManyToManyAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAddedEvent>
                >();
        }

        private static void AddGetBackwardManyToManyAssociationsHandler<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAddedEvent>(IServiceCollection services)
    where TAssociateModel : Models.IModel
    where TAssociationModel : Models.IManyToManyAssociation
    where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
    where TAssociationAggregate : class, IEventSourcedAggregate, IConvertible<TAssociationModel>, new()
    where TAddedEvent : Events.IAddedEvent
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetBackwardManyToManyAssociationsOfModels<TAssociateModel, TAssociationModel>,
              IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>
                >,
              Handlers.GetBackwardManyToManyAssociationsOfModelsHandler<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAddedEvent>
                >();
        }
    }
}