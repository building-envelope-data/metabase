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
              Queries.GetModels<Models.IModel>,
              IEnumerable<Result<Models.IModel, Errors>>
                >,
              Handlers.GetModelsOfUnknownTypeHandler
                >();
        }

        private static void AddComponentHandlers(IServiceCollection services)
        {
            AddModelHandlers<Models.Component, Aggregates.ComponentAggregate, ValueObjects.CreateComponentInput, Events.ComponentCreated>(
                    services,
                    Events.ComponentCreated.From
                    );
        }

        private static void AddDatabaseHandlers(IServiceCollection services)
        {
            AddModelHandlers<Models.Database, Aggregates.DatabaseAggregate, ValueObjects.CreateDatabaseInput, Events.DatabaseCreated>(
                    services,
                    Events.DatabaseCreated.From
                    );
        }

        private static void AddInstitutionHandlers(IServiceCollection services)
        {
            AddModelHandlers<Models.Institution, Aggregates.InstitutionAggregate, ValueObjects.CreateInstitutionInput, Events.InstitutionCreated>(
                    services,
                    Events.InstitutionCreated.From
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
                    Events.MethodCreated.From
                    );
        }

        private static void AddPersonHandlers(IServiceCollection services)
        {
            AddModelHandlers<Models.Person, Aggregates.PersonAggregate, ValueObjects.CreatePersonInput, Events.PersonCreated>(
                    services,
                    Events.PersonCreated.From
                    );
        }

        private static void AddStakeholderHandlers(IServiceCollection services)
        {
            // Get
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModels<Models.Stakeholder>,
              IEnumerable<Result<Models.Stakeholder, Errors>>
                >,
              Handlers.GetStakeholdersHandler
                >();
        }

        private static void AddStandardHandlers(IServiceCollection services)
        {
            AddModelHandlers<Models.Standard, Aggregates.StandardAggregate, ValueObjects.CreateStandardInput, Events.StandardCreated>(
                    services,
                    Events.StandardCreated.From
                    );
        }

        private static void AddModelHandlers<TModel, TAggregate, TCreateInput, TCreatedEvent>(
                IServiceCollection services,
                Func<Guid, Commands.Create<TCreateInput>, Events.IEvent> newCreatedEvent
                    )
                where TModel : Models.IModel
                where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
                where TCreatedEvent : Events.ICreatedEvent
        {
            AddCreateHandler<TCreateInput, TAggregate>(
                    services,
                    newCreatedEvent
                    );
            AddGetModelsHandler<TModel, TAggregate, TCreatedEvent>(services);
            AddGetModelHandler<TModel, TAggregate>(services);
        }

        private static void AddCreateHandler<TInput, TAggregate>(
                IServiceCollection services,
                Func<Guid, Commands.Create<TInput>, Events.IEvent> newEvent
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

        private static void AddGetModelsHandler<TModel, TAggregate, TCreatedEvent>(
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
                Queries.GetModels<TModel>,
                IEnumerable<Result<TModel, Errors>>
                    >,
                Handlers.GetModelsHandler<TModel, TAggregate>
                    >();
        }

        private static void AddComponentConcretizationHandlers(IServiceCollection services)
        {
            AddReflexiveComponentAssociationHandlers<
                Models.ComponentConcretization,
                Aggregates.ComponentConcretizationAggregate,
                Events.ComponentConcretizationAdded,
                ValueObjects.AddComponentConcretizationInput
                >(
                    services,
                    Events.ComponentConcretizationAdded.From
                    );
        }

        private static void AddComponentPartHandlers(IServiceCollection services)
        {
            AddReflexiveComponentAssociationHandlers<
                Models.ComponentPart,
                Aggregates.ComponentPartAggregate,
                Events.ComponentPartAdded,
                ValueObjects.AddComponentPartInput
                >(
                    services,
                    Events.ComponentPartAdded.From
                    );
        }

        private static void AddComponentVariantHandlers(IServiceCollection services)
        {
            AddReflexiveComponentAssociationHandlers<
                Models.ComponentVariant,
                Aggregates.ComponentVariantAggregate,
                Events.ComponentVariantAdded,
                ValueObjects.AddComponentVariantInput
                >(
                    services,
                    Events.ComponentVariantAdded.From
                    );
        }

        private static void AddComponentVersionHandlers(IServiceCollection services)
        {
            AddReflexiveComponentAssociationHandlers<
                Models.ComponentVersion,
                Aggregates.ComponentVersionAggregate,
                Events.ComponentVersionAdded,
                ValueObjects.AddComponentVersionInput
                >(
                    services,
                    Events.ComponentVersionAdded.From
                    );
        }

        private static void AddReflexiveComponentAssociationHandlers<TAssociationModel, TAssociationAggregate, TAddedEvent, TAddInput>(
                        IServiceCollection services,
                        Func<Guid, Commands.Add<TAddInput>, Events.IEvent> newEvent
                        )
            where TAssociationModel : Models.IModel
            where TAssociationAggregate : class, IEventSourcedAggregate, IConvertible<TAssociationModel>, new()
            where TAddedEvent : Events.IAddedEvent
        {
            AddReflexiveAssociationHandlers<Models.Component, TAssociationModel, Aggregates.ComponentAggregate, TAssociationAggregate, TAddedEvent, TAddInput>(
                    services,
                    newEvent
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
                ValueObjects.AddComponentManufacturerInput
                    >(
                            services,
                            Events.ComponentManufacturerAdded.From
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
                ValueObjects.AddInstitutionRepresentativeInput
                    >(
                            services,
                            Events.InstitutionRepresentativeAdded.From
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

            // Get
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModels<Models.MethodDeveloper>,
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
            AddGetBackwardManyToManyAssociatesHandler<Models.Person, Models.MethodDeveloper, Models.Method, Aggregates.MethodAggregate, Events.PersonMethodDeveloperAdded>(services);
            AddGetBackwardManyToManyAssociatesHandler<Models.Institution, Models.MethodDeveloper, Models.Method, Aggregates.MethodAggregate, Events.InstitutionMethodDeveloperAdded>(services);
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
                ValueObjects.AddPersonAffiliationInput
                    >(
                            services,
                            Events.PersonAffiliationAdded.From
                     );
        }

        private static void AddReflexiveAssociationHandlers<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAddedEvent, TAddInput>(
                        IServiceCollection services,
                        Func<Guid, Commands.Add<TAddInput>, Events.IEvent> newEvent
                        )
            where TModel : Models.IModel
            where TAssociationModel : Models.IModel
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, IEventSourcedAggregate, IConvertible<TAssociationModel>, new()
            where TAddedEvent : Events.IAddedEvent
        {
            AddAssociationHandlers<TModel, TAssociationModel, TModel, TAggregate, TAssociationAggregate, TAggregate, TAddedEvent, TAddInput>(
                    services,
                    newEvent
                    );
        }

        private static void AddAssociationHandlersWithAssociationGetters<TModel, TAssociationModel, TAssociatedModel, TAggregate, TAssociationAggregate, TAssociatedAggregate, TAddedEvent, TAddInput>(
                        IServiceCollection services,
                        Func<Guid, Commands.Add<TAddInput>, Events.IEvent> newEvent
                        )
                    where TModel : Models.IModel
                    where TAssociationModel : Models.IModel
                    where TAssociatedModel : Models.IModel
                    where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
                    where TAssociationAggregate : class, IEventSourcedAggregate, IConvertible<TAssociationModel>, new()
                    where TAssociatedAggregate : class, IEventSourcedAggregate, IConvertible<TAssociatedModel>, new()
                    where TAddedEvent : Events.IAddedEvent
        {
            AddAddHandler<TAddInput, TAssociationAggregate>(services, newEvent);
            AddGetHandler<TAssociationModel, TAssociationAggregate>(services);
            AddGetManyToManyAssociatesHandler<TModel, TAssociationModel, TAssociatedModel, TAggregate, TAssociatedAggregate, TAddedEvent>(services);
            AddGetAssociationsHandler<TModel, TAssociationModel, TAssociatedModel, TAssociationAggregate, TAddedEvent>(services);
        }

        private static void AddAssociationHandlers<TModel, TAssociationModel, TAssociatedModel, TAggregate, TAssociationAggregate, TAssociatedAggregate, TAddedEvent, TAddInput>(
                        IServiceCollection services,
                        Func<Guid, Commands.Add<TAddInput>, Events.IEvent> newEvent
                        )
                    where TModel : Models.IModel
                    where TAssociationModel : Models.IModel
                    where TAssociatedModel : Models.IModel
                    where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
                    where TAssociationAggregate : class, IEventSourcedAggregate, IConvertible<TAssociationModel>, new()
                    where TAssociatedAggregate : class, IEventSourcedAggregate, IConvertible<TAssociatedModel>, new()
                    where TAddedEvent : Events.IAddedEvent
        {
            AddAddHandler<TAddInput, TAssociationAggregate>(services, newEvent);
            AddGetHandler<TAssociationModel, TAssociationAggregate>(services);
            AddGetManyToManyAssociatesHandler<TModel, TAssociationModel, TAssociatedModel, TAggregate, TAssociatedAggregate, TAddedEvent>(services);
        }

        private static void AddAddHandler<TInput, TAggregate>(
                IServiceCollection services,
                Func<Guid, Commands.Add<TInput>, Events.IEvent> newEvent
                )
            where TAggregate : class, IEventSourcedAggregate, new()
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.Add<TInput>,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.CreateModelHandler<Commands.Add<TInput>, TAggregate>>(serviceProvider =>
                  new Handlers.CreateModelHandler<Commands.Add<TInput>, TAggregate>(
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
              Queries.GetModels<TModel>,
              IEnumerable<Result<TModel, Errors>>
                >,
              Handlers.GetModelsHandler<TModel, TAggregate>
                >();
        }

        /* private static void AddGetOneToManyAssociatesHandler<TModel, TAssociatedModel, TAssociatedAggregate, TCreatedEvent>( */
        /*     IServiceCollection services, */
        /*                 Func<Guid[], Expression<Func<TCreatedEvent, bool>>> where, */
        /*                 Expression<Func<TCreatedEvent, Handlers.GetOneToManyAssociatesOfModelsHandler<TModel, TAssociatedModel, TAssociatedAggregate, TCreatedEvent>.Select>> select */
        /*     ) */
        /*     where TModel : Models.IModel */
        /*     where TAssociatedAggregate : class, IEventSourcedAggregate, IConvertible<TAssociatedModel>, new() */
        /*     where TCreatedEvent : Events.ICreatedEvent */
        /* { */
        /*     services.AddScoped< */
        /*       MediatR.IRequestHandler< */
        /*       Queries.GetOneToManyAssociatesOfModels<TModel, TAssociatedModel>, */
        /*       IEnumerable<Result<IEnumerable<Result<TAssociatedModel, Errors>>, Errors>> */
        /*         >, */
        /*       Handlers.GetOneToManyAssociatesOfModelsHandler<TModel, TAssociatedModel, TAssociatedAggregate, TCreatedEvent> */
        /*         >(serviceProvider => */
        /*           new Handlers.GetOneToManyAssociatesOfModelsHandler<TModel, TAssociatedModel, TAssociatedAggregate, TCreatedEvent>( */
        /*             where, */
        /*                                 select, */
        /*             serviceProvider.GetRequiredService<IAggregateRepository>() */
        /*             ) */
        /*          ); */
        /* } */

        private static void AddGetManyToManyAssociatesHandler<TModel, TAssociationModel, TAssociatedModel, TAggregate, TAssociatedAggregate, TAddedEvent>(IServiceCollection services)
            where TModel : Models.IModel
            where TAssociatedModel : Models.IModel
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAssociatedAggregate : class, IEventSourcedAggregate, IConvertible<TAssociatedModel>, new()
            where TAddedEvent : Events.IAddedEvent
        {
            AddGetForwardManyToManyAssociatesHandler<TModel, TAssociationModel, TAssociatedModel, TAssociatedAggregate, TAddedEvent>(services);
            AddGetBackwardManyToManyAssociatesHandler<TAssociatedModel, TAssociationModel, TModel, TAggregate, TAddedEvent>(services);
        }

        private static void AddGetForwardManyToManyAssociatesHandler<TModel, TAssociationModel, TAssociatedModel, TAssociatedAggregate, TAddedEvent>(IServiceCollection services)
            where TModel : Models.IModel
            where TAssociatedAggregate : class, IEventSourcedAggregate, IConvertible<TAssociatedModel>, new()
            where TAddedEvent : Events.IAddedEvent
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetForwardManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociatedModel>,
              IEnumerable<Result<IEnumerable<Result<TAssociatedModel, Errors>>, Errors>>
                >,
              Handlers.GetForwardManyToManyAssociatesOfModelsHandler<TModel, TAssociationModel, TAssociatedModel, TAssociatedAggregate, TAddedEvent>
                >();
        }

        private static void AddGetBackwardManyToManyAssociatesHandler<TAssociatedModel, TAssociationModel, TModel, TAggregate, TAddedEvent>(IServiceCollection services)
            where TModel : Models.IModel
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAddedEvent : Events.IAddedEvent
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetBackwardManyToManyAssociatesOfModels<TAssociatedModel, TAssociationModel, TModel>,
              IEnumerable<Result<IEnumerable<Result<TModel, Errors>>, Errors>>
                >,
              Handlers.GetBackwardManyToManyAssociatesOfModelsHandler<TAssociatedModel, TAssociationModel, TModel, TAggregate, TAddedEvent>
                >();
        }

        private static void AddGetAssociationsHandler<TModel, TAssociationModel, TAssociatedModel, TAssociationAggregate, TAddedEvent>(IServiceCollection services)
    where TModel : Models.IModel
    where TAssociatedModel : Models.IModel
    where TAssociationModel : Models.IModel
    where TAssociationAggregate : class, IEventSourcedAggregate, IConvertible<TAssociationModel>, new()
    where TAddedEvent : Events.IAddedEvent
        {
            AddGetForwardManyToManyAssociationsHandler<TModel, TAssociationModel, TAssociationAggregate, TAddedEvent>(services);
            AddGetBackwardManyToManyAssociationsHandler<TAssociatedModel, TAssociationModel, TAssociationAggregate, TAddedEvent>(services);
        }

        private static void AddGetForwardManyToManyAssociationsHandler<TModel, TAssociationModel, TAssociationAggregate, TAddedEvent>(IServiceCollection services)
    where TModel : Models.IModel
    where TAssociationModel : Models.IModel
    where TAssociationAggregate : class, IEventSourcedAggregate, IConvertible<TAssociationModel>, new()
    where TAddedEvent : Events.IAddedEvent
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetForwardManyToManyAssociationsOfModels<TModel, TAssociationModel>,
              IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>
                >,
              Handlers.GetForwardManyToManyAssociationsOfModelsHandler<TModel, TAssociationModel, TAssociationAggregate, TAddedEvent>
                >();
        }

        private static void AddGetBackwardManyToManyAssociationsHandler<TModel, TAssociationModel, TAssociationAggregate, TAddedEvent>(IServiceCollection services)
    where TModel : Models.IModel
    where TAssociationModel : Models.IModel
    where TAssociationAggregate : class, IEventSourcedAggregate, IConvertible<TAssociationModel>, new()
    where TAddedEvent : Events.IAddedEvent
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetBackwardManyToManyAssociationsOfModels<TModel, TAssociationModel>,
              IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>
                >,
              Handlers.GetBackwardManyToManyAssociationsOfModelsHandler<TModel, TAssociationModel, TAssociationAggregate, TAddedEvent>
                >();
        }
    }
}