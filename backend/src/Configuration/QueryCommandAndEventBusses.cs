using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Icon.Data;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.Validation;
using Marten; // IsOneOf
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;
using WebPWrecover.Services;
using AddAssociationCheck = Icon.Infrastructure.Aggregate.AddAssociationCheck;
using CancellationToken = System.Threading.CancellationToken;
using Command = Icon.Infrastructure.Command;
using Commands = Icon.Commands;
using Events = Icon.Events;
using Handlers = Icon.Handlers;
using IAggregateRepository = Icon.Infrastructure.Aggregate.IAggregateRepository;
using IAggregateRepositoryReadOnlySession = Icon.Infrastructure.Aggregate.IAggregateRepositoryReadOnlySession;
using IAggregateRepositorySession = Icon.Infrastructure.Aggregate.IAggregateRepositorySession;
using IError = HotChocolate.IError;
using IEventSourcedAggregate = Icon.Infrastructure.Aggregate.IEventSourcedAggregate;
using Models = Icon.Models;
using Queries = Icon.Queries;
using Query = Icon.Infrastructure.Query;

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
            AddOpticalDataHandlers(services);
            AddOpticalDataFromDatabaseHandlers(services);
            AddPersonHandlers(services);
            AddStakeholderHandlers(services);
            AddStandardHandlers(services);

            AddComponentConcretizationHandlers(services);
            AddComponentPartHandlers(services);
            AddComponentVariantHandlers(services);
            AddComponentVersionHandlers(services);

            AddComponentManufacturerHandlers(services);
            AddComponentOpticalDataHandlers(services);
            AddInstitutionOperatedDatabaseHandlers(services);
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
                    Enumerable.Empty<Func<IAggregateRepositorySession, ValueObjects.Id, Commands.Create<ValueObjects.CreateComponentInput>, CancellationToken, Task<Result<ValueObjects.Id, Errors>>>>(),
                    Events.ComponentDeleted.From,
                    new Func<IAggregateRepositorySession, ValueObjects.TimestampedId, ValueObjects.Id, CancellationToken, Task<Result<bool, Errors>>>[]
                    {
                    (session, timestampedId, creatorId, cancellationToken) =>
                    session.RemoveForwardManyToManyAssociationsOfModel<Models.Component, Models.ComponentManufacturer, Aggregates.ComponentAggregate, Aggregates.ComponentManufacturerAggregate, Events.ComponentManufacturerAdded>(
                        timestampedId,
                        componentManufacturerId => new Events.ComponentManufacturerRemoved(componentManufacturerId, creatorId),
                        cancellationToken
                        ),
                    (session, timestampedId, creatorId, cancellationToken) =>
                    session.RemoveForwardOneToManyAssociationsOfModel<Models.Component, Models.ComponentOpticalData, Aggregates.ComponentAggregate, Aggregates.ComponentOpticalDataAggregate, Events.ComponentOpticalDataAdded>(
                        timestampedId,
                        componentOpticalDataId => new Events.ComponentOpticalDataRemoved(componentOpticalDataId, creatorId),
                        cancellationToken
                        ),
                    (session, timestampedId, creatorId, cancellationToken) =>
                    session.RemoveForwardManyToManyAssociationsOfModel<Models.Component, Models.ComponentConcretization, Aggregates.ComponentAggregate, Aggregates.ComponentConcretizationAggregate, Events.ComponentConcretizationAdded>(
                        timestampedId,
                        componentConcretizationId => new Events.ComponentConcretizationRemoved(componentConcretizationId, creatorId),
                        cancellationToken
                        ),
                    (session, timestampedId, creatorId, cancellationToken) =>
                    session.RemoveBackwardManyToManyAssociationsOfModel<Models.Component, Models.ComponentConcretization, Aggregates.ComponentAggregate, Aggregates.ComponentConcretizationAggregate, Events.ComponentConcretizationAdded>(
                        timestampedId,
                        componentConcretizationId => new Events.ComponentConcretizationRemoved(componentConcretizationId, creatorId),
                        cancellationToken
                        ),
                    (session, timestampedId, creatorId, cancellationToken) =>
                    session.RemoveForwardManyToManyAssociationsOfModel<Models.Component, Models.ComponentPart, Aggregates.ComponentAggregate, Aggregates.ComponentPartAggregate, Events.ComponentPartAdded>(
                        timestampedId,
                        componentPartId => new Events.ComponentPartRemoved(componentPartId, creatorId),
                        cancellationToken
                        ),
                    (session, timestampedId, creatorId, cancellationToken) =>
                    session.RemoveBackwardManyToManyAssociationsOfModel<Models.Component, Models.ComponentPart, Aggregates.ComponentAggregate, Aggregates.ComponentPartAggregate, Events.ComponentPartAdded>(
                        timestampedId,
                        componentPartId => new Events.ComponentPartRemoved(componentPartId, creatorId),
                        cancellationToken
                        ),
                    (session, timestampedId, creatorId, cancellationToken) =>
                    session.RemoveForwardManyToManyAssociationsOfModel<Models.Component, Models.ComponentVariant, Aggregates.ComponentAggregate, Aggregates.ComponentVariantAggregate, Events.ComponentVariantAdded>(
                        timestampedId,
                        componentVariantId => new Events.ComponentVariantRemoved(componentVariantId, creatorId),
                        cancellationToken
                        ),
                    (session, timestampedId, creatorId, cancellationToken) =>
                    session.RemoveBackwardManyToManyAssociationsOfModel<Models.Component, Models.ComponentVariant, Aggregates.ComponentAggregate, Aggregates.ComponentVariantAggregate, Events.ComponentVariantAdded>(
                        timestampedId,
                        componentVariantId => new Events.ComponentVariantRemoved(componentVariantId, creatorId),
                        cancellationToken
                        ),
                    (session, timestampedId, creatorId, cancellationToken) =>
                    session.RemoveForwardManyToManyAssociationsOfModel<Models.Component, Models.ComponentVersion, Aggregates.ComponentAggregate, Aggregates.ComponentVersionAggregate, Events.ComponentVersionAdded>(
                        timestampedId,
                        componentVersionId => new Events.ComponentVersionRemoved(componentVersionId, creatorId),
                        cancellationToken
                        ),
                    (session, timestampedId, creatorId, cancellationToken) =>
                    session.RemoveBackwardManyToManyAssociationsOfModel<Models.Component, Models.ComponentVersion, Aggregates.ComponentAggregate, Aggregates.ComponentVersionAggregate, Events.ComponentVersionAdded>(
                        timestampedId,
                        componentVersionId => new Events.ComponentVersionRemoved(componentVersionId, creatorId),
                        cancellationToken
                        )
                    }
                    );
        }

        private static void AddDatabaseHandlers(IServiceCollection services)
        {
            AddModelHandlers<Models.Database, Aggregates.DatabaseAggregate, ValueObjects.CreateDatabaseInput, Events.DatabaseCreated>(
                    services,
                    Events.DatabaseCreated.From,
                    new Func<IAggregateRepositorySession, ValueObjects.Id, Commands.Create<ValueObjects.CreateDatabaseInput>, CancellationToken, Task<Result<ValueObjects.Id, Errors>>>[]
                    {
                    (session, databaseId, command, cancellationToken) =>
                    session.AddOneToManyAssociation<Aggregates.InstitutionAggregate, Aggregates.InstitutionOperatedDatabaseAggregate, Aggregates.DatabaseAggregate>(
                        id => Events.InstitutionOperatedDatabaseAdded.From(id, databaseId, command),
                        AddAssociationCheck.PARENT,
                        cancellationToken
                        )
                    },
                    Events.DatabaseDeleted.From,
                    new Func<IAggregateRepositorySession, ValueObjects.TimestampedId, ValueObjects.Id, CancellationToken, Task<Result<bool, Errors>>>[]
                    {
                      (session, timestampedId, creatorId, cancellationToken) =>
                      session.RemoveBackwardOneToManyAssociationOfModel<Models.Database, Models.InstitutionOperatedDatabase, Aggregates.DatabaseAggregate, Aggregates.InstitutionOperatedDatabaseAggregate, Events.InstitutionOperatedDatabaseAdded>(
                          timestampedId,
                          institutionOperatedDatabaseId => new Events.InstitutionOperatedDatabaseRemoved(institutionOperatedDatabaseId, creatorId),
                          cancellationToken
                          ),
                    }
                    );
        }

        private static void AddInstitutionHandlers(IServiceCollection services)
        {
            AddModelHandlers<Models.Institution, Aggregates.InstitutionAggregate, ValueObjects.CreateInstitutionInput, Events.InstitutionCreated>(
                    services,
                    Events.InstitutionCreated.From,
                    Enumerable.Empty<Func<IAggregateRepositorySession, ValueObjects.Id, Commands.Create<ValueObjects.CreateInstitutionInput>, CancellationToken, Task<Result<ValueObjects.Id, Errors>>>>(),
                    Events.InstitutionDeleted.From,
                    new Func<IAggregateRepositorySession, ValueObjects.TimestampedId, ValueObjects.Id, CancellationToken, Task<Result<bool, Errors>>>[]
                    {
                      (session, timestampedId, creatorId, cancellationToken) =>
                      session.RemoveBackwardManyToManyAssociationsOfModel<Models.Institution, Models.PersonAffiliation, Aggregates.InstitutionAggregate, Aggregates.PersonAffiliationAggregate, Events.PersonAffiliationAdded>(
                          timestampedId,
                          personAffiliationId => new Events.PersonAffiliationRemoved(personAffiliationId, creatorId),
                          cancellationToken
                          ),
                      (session, timestampedId, creatorId, cancellationToken) =>
                      session.RemoveBackwardManyToManyAssociationsOfModel<Models.Institution, Models.MethodDeveloper, Aggregates.InstitutionAggregate, Aggregates.InstitutionMethodDeveloperAggregate, Events.InstitutionMethodDeveloperAdded>(
                          timestampedId,
                          institutionMethodDeveloperId => new Events.InstitutionMethodDeveloperRemoved(institutionMethodDeveloperId, creatorId),
                          cancellationToken
                          ),
                      (session, timestampedId, creatorId, cancellationToken) =>
                      session.RemoveForwardOneToManyAssociationsOfModel<Models.Institution, Models.InstitutionOperatedDatabase, Aggregates.InstitutionAggregate, Aggregates.InstitutionOperatedDatabaseAggregate, Events.InstitutionOperatedDatabaseAdded>(
                          timestampedId,
                          institutionOperatedDatabaseId => new Events.InstitutionOperatedDatabaseRemoved(institutionOperatedDatabaseId, creatorId),
                          cancellationToken
                          ),
                      (session, timestampedId, creatorId, cancellationToken) =>
                      session.RemoveBackwardManyToManyAssociationsOfModel<Models.Institution, Models.ComponentManufacturer, Aggregates.InstitutionAggregate, Aggregates.ComponentManufacturerAggregate, Events.ComponentManufacturerAdded>(
                          timestampedId,
                          componentManufacturerId => new Events.ComponentManufacturerRemoved(componentManufacturerId, creatorId),
                          cancellationToken
                          )
                    }
            );
        }

        private static void AddMethodHandlers(IServiceCollection services)
        {
            AddModelHandlers<Models.Method, Aggregates.MethodAggregate, ValueObjects.CreateMethodInput, Events.MethodCreated>(
                    services,
                    Events.MethodCreated.From,
                    Enumerable.Empty<Func<IAggregateRepositorySession, ValueObjects.Id, Commands.Create<ValueObjects.CreateMethodInput>, CancellationToken, Task<Result<ValueObjects.Id, Errors>>>>(),
                    Events.MethodDeleted.From,
                    new Func<IAggregateRepositorySession, ValueObjects.TimestampedId, ValueObjects.Id, CancellationToken, Task<Result<bool, Errors>>>[]
                    {
                      (session, timestampedId, creatorId, cancellationToken) =>
                      session.RemoveForwardManyToManyAssociationsOfModel<Models.Method, Models.MethodDeveloper, Aggregates.MethodAggregate, Aggregates.PersonMethodDeveloperAggregate, Events.PersonMethodDeveloperAdded>(
                          timestampedId,
                          personMethodDeveloperId => new Events.PersonMethodDeveloperRemoved(personMethodDeveloperId, creatorId),
                          cancellationToken
                          ),
                      (session, timestampedId, creatorId, cancellationToken) =>
                      session.RemoveForwardManyToManyAssociationsOfModel<Models.Method, Models.MethodDeveloper, Aggregates.MethodAggregate, Aggregates.InstitutionMethodDeveloperAggregate, Events.InstitutionMethodDeveloperAdded>(
                          timestampedId,
                          institutionMethodDeveloperId => new Events.InstitutionMethodDeveloperRemoved(institutionMethodDeveloperId, creatorId),
                          cancellationToken
                          )
                    }
                    );
        }

        private static void AddOpticalDataHandlers(IServiceCollection services)
        {
            AddModelHandlers<Models.OpticalData, Aggregates.OpticalDataAggregate, ValueObjects.CreateOpticalDataInput, Events.OpticalDataCreated>(
                    services,
                    Events.OpticalDataCreated.From,
                    new Func<IAggregateRepositorySession, ValueObjects.Id, Commands.Create<ValueObjects.CreateOpticalDataInput>, CancellationToken, Task<Result<ValueObjects.Id, Errors>>>[]
                    {
                    (session, opticalDataId, command, cancellationToken) =>
                    session.AddOneToManyAssociation<Aggregates.ComponentAggregate, Aggregates.ComponentOpticalDataAggregate, Aggregates.OpticalDataAggregate>(
                        id => Events.ComponentOpticalDataAdded.From(id, opticalDataId, command),
                        AddAssociationCheck.PARENT,
                        cancellationToken
                        )
                    },
                    Events.OpticalDataDeleted.From,
                    new Func<IAggregateRepositorySession, ValueObjects.TimestampedId, ValueObjects.Id, CancellationToken, Task<Result<bool, Errors>>>[]
                    {
                      (session, timestampedId, creatorId, cancellationToken) =>
                      session.RemoveBackwardOneToManyAssociationOfModel<Models.OpticalData, Models.ComponentOpticalData, Aggregates.OpticalDataAggregate, Aggregates.ComponentOpticalDataAggregate, Events.ComponentOpticalDataAdded>(
                          timestampedId,
                          componentOpticalDataId => new Events.ComponentOpticalDataRemoved(componentOpticalDataId, creatorId),
                          cancellationToken
                          ),
                    }
                    );

            // Has
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.HasOpticalDataForComponents,
              IEnumerable<Result<bool, Errors>>
                >,
              Handlers.HasOpticalDataForComponentsHandler
                >();
        }

        private static void AddOpticalDataFromDatabaseHandlers(IServiceCollection services)
        {
            // Get
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.QueryDataArrayOfComponentsFromDatabases<Models.OpticalDataFromDatabase>,
              IEnumerable<Result<IEnumerable<Result<Models.OpticalDataFromDatabase, Errors>>, Errors>>
                >,
              Handlers.QueryOpticalDataOfComponentsFromDatabasesHandler
                >();

            // Who Has
            AddWhichDatabasesHaveDataForComponentsHandler<Models.OpticalDataFromDatabase>(services, "hasOpticalData"); // TODO Get rid of magical string!
        }

        private static void AddWhichDatabasesHaveDataForComponentsHandler<TDataModel>(
            IServiceCollection services,
            string graphQlQueryName
            )
        {
            // Who Has
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.WhichDatabasesHaveDataForComponents<TDataModel>,
              IEnumerable<Result<IEnumerable<Result<Models.Database, Errors>>, Errors>>
                >,
              Handlers.WhichDatabasesHaveDataForComponentsHandler<TDataModel>
                >(serviceProvider =>
                    new Handlers.WhichDatabasesHaveDataForComponentsHandler<TDataModel>(
                      graphQlQueryName,
                      serviceProvider.GetRequiredService<IAggregateRepository>()
                      )
                 );
        }

        private static void AddPersonHandlers(IServiceCollection services)
        {
            AddModelHandlers<Models.Person, Aggregates.PersonAggregate, ValueObjects.CreatePersonInput, Events.PersonCreated>(
                    services,
                    Events.PersonCreated.From,
                    Enumerable.Empty<Func<IAggregateRepositorySession, ValueObjects.Id, Commands.Create<ValueObjects.CreatePersonInput>, CancellationToken, Task<Result<ValueObjects.Id, Errors>>>>(),
                    Events.PersonDeleted.From,
                    new Func<IAggregateRepositorySession, ValueObjects.TimestampedId, ValueObjects.Id, CancellationToken, Task<Result<bool, Errors>>>[]
                    {
                      (session, timestampedId, creatorId, cancellationToken) =>
                      session.RemoveForwardManyToManyAssociationsOfModel<Models.Person, Models.PersonAffiliation, Aggregates.PersonAggregate, Aggregates.PersonAffiliationAggregate, Events.PersonAffiliationAdded>(
                          timestampedId,
                          personAffiliationId => new Events.PersonAffiliationRemoved(personAffiliationId, creatorId),
                          cancellationToken
                          ),
                      (session, timestampedId, creatorId, cancellationToken) =>
                      session.RemoveBackwardManyToManyAssociationsOfModel<Models.Person, Models.MethodDeveloper, Aggregates.PersonAggregate, Aggregates.PersonMethodDeveloperAggregate, Events.PersonMethodDeveloperAdded>(
                          timestampedId,
                          personMethodDeveloperId => new Events.PersonMethodDeveloperRemoved(personMethodDeveloperId, creatorId),
                          cancellationToken
                          )
                    }
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
                    Enumerable.Empty<Func<IAggregateRepositorySession, ValueObjects.Id, Commands.Create<ValueObjects.CreateStandardInput>, CancellationToken, Task<Result<ValueObjects.Id, Errors>>>>(),
                    Events.StandardDeleted.From,
                    Enumerable.Empty<Func<IAggregateRepositorySession, ValueObjects.TimestampedId, ValueObjects.Id, CancellationToken, Task<Result<bool, Errors>>>>()
                    );
        }

        private static void AddModelHandlers<TModel, TAggregate, TCreateInput, TCreatedEvent>(
                IServiceCollection services,
                Func<Guid, Commands.Create<TCreateInput>, Events.ICreatedEvent> newCreatedEvent,
                IEnumerable<Func<IAggregateRepositorySession, ValueObjects.Id, Commands.Create<TCreateInput>, CancellationToken, Task<Result<ValueObjects.Id, Errors>>>> addAssociations,
                Func<Commands.Delete<TModel>, Events.IDeletedEvent> newDeletedEvent,
                IEnumerable<Func<IAggregateRepositorySession, ValueObjects.TimestampedId, ValueObjects.Id, CancellationToken, Task<Result<bool, Errors>>>> removeAssociations
                    )
                where TModel : Models.IModel
                where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
                where TCreatedEvent : Events.ICreatedEvent
        {
            AddCreateHandler<TCreateInput, TAggregate>(
                    services, newCreatedEvent, addAssociations
                    );
            AddDeleteHandler<TModel, TAggregate>(
                services, newDeletedEvent, removeAssociations
                );
            AddGetModelsForTimestampedIdsHandler<TModel, TAggregate, TCreatedEvent>(services);
            AddGetModelHandler<TModel, TAggregate>(services);
        }

        private static void AddCreateHandler<TInput, TAggregate>(
                IServiceCollection services,
                Func<Guid, Commands.Create<TInput>, Events.ICreatedEvent> newEvent,
                IEnumerable<Func<IAggregateRepositorySession, ValueObjects.Id, Commands.Create<TInput>, CancellationToken, Task<Result<ValueObjects.Id, Errors>>>> addAssociations
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
                    newEvent,
                    addAssociations
                    )
                  );
        }

        private static void AddDeleteHandler<TModel, TAggregate>(
                IServiceCollection services,
                Func<Commands.Delete<TModel>, Events.IDeletedEvent> newEvent,
                IEnumerable<Func<IAggregateRepositorySession, ValueObjects.TimestampedId, ValueObjects.Id, CancellationToken, Task<Result<bool, Errors>>>> removeAssociations
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
                    newEvent,
                    removeAssociations
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
            AddReflexiveManyToManyComponentAssociationHandlers<
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
            AddReflexiveManyToManyComponentAssociationHandlers<
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
            AddReflexiveManyToManyComponentAssociationHandlers<
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
            AddReflexiveManyToManyComponentAssociationHandlers<
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

        private static void AddReflexiveManyToManyComponentAssociationHandlers<TAssociationModel, TAssociationAggregate, TAssociationAddedEvent, TAddInput, TAssociationRemovedEvent>(
                        IServiceCollection services,
                        Func<Guid, Commands.AddAssociation<TAddInput>, Events.IAssociationAddedEvent> newAssociationAddedEvent,
                        Func<Guid, Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>, Events.IAssociationRemovedEvent> newAssociationRemovedEvent
                        )
            where TAssociationModel : Models.IManyToManyAssociation
            where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent
            where TAddInput : ValueObjects.AddManyToManyAssociationInput
            where TAssociationRemovedEvent : Events.IAssociationRemovedEvent
        {
            AddReflexiveManyToManyAssociationHandlers<Models.Component, TAssociationModel, Aggregates.ComponentAggregate, TAssociationAggregate, TAssociationAddedEvent, TAddInput, TAssociationRemovedEvent>(
                    services,
                    newAssociationAddedEvent,
                    newAssociationRemovedEvent
                    );
        }

        private static void AddComponentManufacturerHandlers(IServiceCollection services)
        {
            AddManyToManyAssociationHandlersWithAssociationGetters<
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

        private static void AddComponentOpticalDataHandlers(IServiceCollection services)
        {
            AddOneToManyAssociationHandlers<
                Models.Component,
                Models.ComponentOpticalData,
                Models.OpticalData,
                Aggregates.ComponentAggregate,
                Aggregates.ComponentOpticalDataAggregate,
                Aggregates.OpticalDataAggregate,
                Events.ComponentOpticalDataAdded,
                ValueObjects.AddComponentOpticalDataInput,
                Events.ComponentOpticalDataRemoved
                    >(
                            services,
                            Events.ComponentOpticalDataAdded.From,
                            Events.ComponentOpticalDataRemoved.From
                     );
        }

        private static void AddInstitutionOperatedDatabaseHandlers(IServiceCollection services)
        {
            AddOneToManyAssociationHandlersWithAssociationGetters<
                Models.Institution,
                Models.InstitutionOperatedDatabase,
                Models.Database,
                Aggregates.InstitutionAggregate,
                Aggregates.InstitutionOperatedDatabaseAggregate,
                Aggregates.DatabaseAggregate,
                Events.InstitutionOperatedDatabaseAdded,
                ValueObjects.AddInstitutionOperatedDatabaseInput,
                Events.InstitutionOperatedDatabaseRemoved
                    >(
                            services,
                            Events.InstitutionOperatedDatabaseAdded.From,
                            Events.InstitutionOperatedDatabaseRemoved.From
                     );
        }

        private static void AddInstitutionRepresentativeHandlers(IServiceCollection services)
        {
            AddManyToManyAssociationHandlersWithAssociationGetters<
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
              Commands.AddAssociation<ValueObjects.AddMethodDeveloperInput>,
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
            AddManyToManyAssociationHandlers<
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

        private static void AddReflexiveManyToManyAssociationHandlers<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent, TAddInput, TAssociationRemovedEvent>(
                        IServiceCollection services,
                        Func<Guid, Commands.AddAssociation<TAddInput>, Events.IAssociationAddedEvent> newAssociationAddedEvent,
                        Func<Guid, Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>, Events.IAssociationRemovedEvent> newAssociationRemovedEvent
                        )
            where TModel : Models.IModel
            where TAssociationModel : Models.IManyToManyAssociation
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent
            where TAddInput : ValueObjects.AddManyToManyAssociationInput
            where TAssociationRemovedEvent : Events.IAssociationRemovedEvent
        {
            AddManyToManyAssociationHandlers<TModel, TAssociationModel, TModel, TAggregate, TAssociationAggregate, TAggregate, TAssociationAddedEvent, TAddInput, TAssociationRemovedEvent>(
                    services,
                    newAssociationAddedEvent,
                    newAssociationRemovedEvent
                    );
        }

        private static void AddManyToManyAssociationHandlersWithAssociationGetters<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent, TAddInput, TAssociationRemovedEvent>(
                        IServiceCollection services,
                        Func<Guid, Commands.AddAssociation<TAddInput>, Events.IAssociationAddedEvent> newAssociationAddedEvent,
                        Func<Guid, Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>, Events.IAssociationRemovedEvent> newAssociationRemovedEvent
                        )
                    where TModel : Models.IModel
                    where TAssociationModel : Models.IManyToManyAssociation
                    where TAssociateModel : Models.IModel
                    where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
                    where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
                    where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
                    where TAssociationAddedEvent : Events.IAssociationAddedEvent
                    where TAddInput : ValueObjects.AddManyToManyAssociationInput
                    where TAssociationRemovedEvent : Events.IAssociationRemovedEvent
        {
            AddManyToManyAssociationHandlers<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent, TAddInput, TAssociationRemovedEvent>(
                    services,
                    newAssociationAddedEvent,
                    newAssociationRemovedEvent
                    );
            AddGetManyToManyAssociationsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(services);
        }

        private static void AddManyToManyAssociationHandlers<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent, TAddInput, TAssociationRemovedEvent>(
                        IServiceCollection services,
                        Func<Guid, Commands.AddAssociation<TAddInput>, Events.IAssociationAddedEvent> newAssociationAddedEvent,
                        Func<Guid, Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>, Events.IAssociationRemovedEvent> newAssociationRemovedEvent
                        )
                    where TModel : Models.IModel
                    where TAssociationModel : Models.IManyToManyAssociation
                    where TAssociateModel : Models.IModel
                    where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
                    where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
                    where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
                    where TAssociationAddedEvent : Events.IAssociationAddedEvent
                    where TAddInput : ValueObjects.AddManyToManyAssociationInput
                    where TAssociationRemovedEvent : Events.IAssociationRemovedEvent
        {
            AddAddManyToManyAssociationHandler<TAddInput, TAggregate, TAssociationAggregate, TAssociateAggregate>(services, newAssociationAddedEvent);
            AddRemoveManyToManyAssociationHandler<TAssociationModel, TAssociationAggregate>(services, newAssociationRemovedEvent);
            AddGetHandler<TAssociationModel, TAssociationAggregate>(services);
            AddGetManyToManyAssociatesHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(services);
        }

        private static void AddAddManyToManyAssociationHandler<TInput, TAggregate, TAssociationAggregate, TAssociateAggregate>(
                IServiceCollection services,
                Func<Guid, Commands.AddAssociation<TInput>, Events.IAssociationAddedEvent> newEvent
                )
            where TInput : ValueObjects.AddManyToManyAssociationInput
            where TAggregate : class, IEventSourcedAggregate, new()
            where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, new()
            where TAssociateAggregate : class, IEventSourcedAggregate, new()
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.AddAssociation<TInput>,
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
                Func<Guid, Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>, Events.IAssociationRemovedEvent> newEvent
                )
            where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, new()
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.RemoveManyToManyAssociationHandler<TAssociationModel, TAssociationAggregate>>(serviceProvider =>
                  new Handlers.RemoveManyToManyAssociationHandler<TAssociationModel, TAssociationAggregate>(
                    serviceProvider.GetRequiredService<IAggregateRepository>(),
                    newEvent
                    )
                  );
        }

        private static void AddOneToManyAssociationHandlersWithAssociationGetters<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent, TAddInput, TAssociationRemovedEvent>(
                        IServiceCollection services,
                        Func<Guid, Commands.AddAssociation<TAddInput>, Events.IAssociationAddedEvent> newAssociationAddedEvent,
                        Func<Guid, Commands.RemoveAssociation<ValueObjects.RemoveOneToManyAssociationInput<TAssociationModel>>, Events.IAssociationRemovedEvent> newAssociationRemovedEvent
                        )
                    where TModel : Models.IModel
                    where TAssociationModel : Models.IOneToManyAssociation
                    where TAssociateModel : Models.IModel
                    where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
                    where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
                    where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
                    where TAssociationAddedEvent : Events.IAssociationAddedEvent
                    where TAddInput : ValueObjects.AddOneToManyAssociationInput
                    where TAssociationRemovedEvent : Events.IAssociationRemovedEvent
        {
            AddOneToManyAssociationHandlers<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent, TAddInput, TAssociationRemovedEvent>(
                    services,
                    newAssociationAddedEvent,
                    newAssociationRemovedEvent
                    );
            AddGetOneToManyAssociationsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(services);
        }

        private static void AddOneToManyAssociationHandlers<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent, TAddInput, TAssociationRemovedEvent>(
                        IServiceCollection services,
                        Func<Guid, Commands.AddAssociation<TAddInput>, Events.IAssociationAddedEvent> newAssociationAddedEvent,
                        Func<Guid, Commands.RemoveAssociation<ValueObjects.RemoveOneToManyAssociationInput<TAssociationModel>>, Events.IAssociationRemovedEvent> newAssociationRemovedEvent
                        )
                    where TModel : Models.IModel
                    where TAssociationModel : Models.IOneToManyAssociation
                    where TAssociateModel : Models.IModel
                    where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
                    where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
                    where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
                    where TAssociationAddedEvent : Events.IAssociationAddedEvent
                    where TAddInput : ValueObjects.AddOneToManyAssociationInput
                    where TAssociationRemovedEvent : Events.IAssociationRemovedEvent
        {
            AddAddOneToManyAssociationHandler<TAddInput, TAggregate, TAssociationAggregate, TAssociateAggregate>(services, newAssociationAddedEvent);
            AddRemoveOneToManyAssociationHandler<TAssociationModel, TAssociationAggregate>(services, newAssociationRemovedEvent);
            AddGetHandler<TAssociationModel, TAssociationAggregate>(services);
            AddGetOneToManyAssociatesHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(services);
        }

        private static void AddAddOneToManyAssociationHandler<TInput, TAggregate, TAssociationAggregate, TAssociateAggregate>(
                IServiceCollection services,
                Func<Guid, Commands.AddAssociation<TInput>, Events.IAssociationAddedEvent> newEvent
                )
            where TInput : ValueObjects.AddOneToManyAssociationInput
            where TAggregate : class, IEventSourcedAggregate, new()
            where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, new()
            where TAssociateAggregate : class, IEventSourcedAggregate, new()
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.AddAssociation<TInput>,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.AddOneToManyAssociationHandler<TInput, TAggregate, TAssociationAggregate, TAssociateAggregate>>(serviceProvider =>
                  new Handlers.AddOneToManyAssociationHandler<TInput, TAggregate, TAssociationAggregate, TAssociateAggregate>(
                    serviceProvider.GetRequiredService<IAggregateRepository>(),
                    newEvent
                    )
                  );
        }

        private static void AddRemoveOneToManyAssociationHandler<TAssociationModel, TAssociationAggregate>(
                IServiceCollection services,
                Func<Guid, Commands.RemoveAssociation<ValueObjects.RemoveOneToManyAssociationInput<TAssociationModel>>, Events.IAssociationRemovedEvent> newEvent
                )
            where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, new()
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.RemoveAssociation<ValueObjects.RemoveOneToManyAssociationInput<TAssociationModel>>,
              Result<ValueObjects.TimestampedId, Errors>
                >,
              Handlers.RemoveOneToManyAssociationHandler<TAssociationModel, TAssociationAggregate>>(serviceProvider =>
                  new Handlers.RemoveOneToManyAssociationHandler<TAssociationModel, TAssociationAggregate>(
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

        private static void AddGetManyToManyAssociatesHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(IServiceCollection services)
            where TModel : Models.IModel
            where TAssociationModel : Models.IManyToManyAssociation
            where TAssociateModel : Models.IModel
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            AddGetForwardManyToManyAssociatesHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(services);
            AddGetBackwardManyToManyAssociatesHandler<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate, TAssociationAddedEvent>(services);
        }

        private static void AddGetForwardManyToManyAssociatesHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(IServiceCollection services)
            where TModel : Models.IModel
            where TAssociationModel : Models.IManyToManyAssociation
            where TAssociateModel : Models.IModel
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetForwardManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>,
              IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>
                >,
              Handlers.GetForwardManyToManyAssociatesOfModelsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>
                >();
        }

        private static void AddGetBackwardManyToManyAssociatesHandler<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate, TAssociationAddedEvent>(IServiceCollection services)
            where TAssociateModel : Models.IModel
            where TAssociationModel : Models.IManyToManyAssociation
            where TModel : Models.IModel
            where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
            where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetBackwardManyToManyAssociatesOfModels<TAssociateModel, TAssociationModel, TModel>,
              IEnumerable<Result<IEnumerable<Result<TModel, Errors>>, Errors>>
                >,
              Handlers.GetBackwardManyToManyAssociatesOfModelsHandler<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate, TAssociationAddedEvent>
                >();
        }

        private static void AddGetManyToManyAssociationsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(IServiceCollection services)
    where TModel : Models.IModel
    where TAssociateModel : Models.IModel
    where TAssociationModel : Models.IManyToManyAssociation
    where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
    where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
    where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
    where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            AddGetForwardManyToManyAssociationsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(services);
            AddGetBackwardManyToManyAssociationsHandler<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(services);
        }

        private static void AddGetForwardManyToManyAssociationsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(IServiceCollection services)
    where TModel : Models.IModel
    where TAssociationModel : Models.IManyToManyAssociation
    where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
    where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
    where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetForwardManyToManyAssociationsOfModels<TModel, TAssociationModel>,
              IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>
                >,
              Handlers.GetForwardManyToManyAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>
                >();
        }

        private static void AddGetBackwardManyToManyAssociationsHandler<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(IServiceCollection services)
    where TAssociateModel : Models.IModel
    where TAssociationModel : Models.IManyToManyAssociation
    where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
    where TAssociationAggregate : class, Aggregates.IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
    where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetBackwardManyToManyAssociationsOfModels<TAssociateModel, TAssociationModel>,
              IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>
                >,
              Handlers.GetBackwardManyToManyAssociationsOfModelsHandler<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>
                >();
        }

        private static void AddGetOneToManyAssociatesHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(IServiceCollection services)
            where TModel : Models.IModel
            where TAssociationModel : Models.IOneToManyAssociation
            where TAssociateModel : Models.IModel
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            AddGetForwardOneToManyAssociatesHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(services);
            AddGetBackwardOneToManyAssociateHandler<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate, TAssociationAddedEvent>(services);
        }

        private static void AddGetForwardOneToManyAssociatesHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(IServiceCollection services)
            where TModel : Models.IModel
            where TAssociationModel : Models.IOneToManyAssociation
            where TAssociateModel : Models.IModel
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetForwardOneToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>,
              IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>
                >,
              Handlers.GetForwardOneToManyAssociatesOfModelsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>
                >();
        }

        private static void AddGetBackwardOneToManyAssociateHandler<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate, TAssociationAddedEvent>(IServiceCollection services)
            where TAssociateModel : Models.IModel
            where TAssociationModel : Models.IOneToManyAssociation
            where TModel : Models.IModel
            where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
            where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetBackwardOneToManyAssociateOfModels<TAssociateModel, TAssociationModel, TModel>,
              IEnumerable<Result<TModel, Errors>>
                >,
              Handlers.GetBackwardOneToManyAssociateOfModelsHandler<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate, TAssociationAddedEvent>
                >();
        }

        private static void AddGetOneToManyAssociationsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(IServiceCollection services)
    where TModel : Models.IModel
    where TAssociateModel : Models.IModel
    where TAssociationModel : Models.IOneToManyAssociation
    where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
    where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
    where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
    where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            AddGetForwardOneToManyAssociationsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(services);
            AddGetBackwardOneToManyAssociationHandler<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(services);
        }

        private static void AddGetForwardOneToManyAssociationsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(IServiceCollection services)
    where TModel : Models.IModel
    where TAssociationModel : Models.IOneToManyAssociation
    where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
    where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
    where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetForwardOneToManyAssociationsOfModels<TModel, TAssociationModel>,
              IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>
                >,
              Handlers.GetForwardOneToManyAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>
                >();
        }

        private static void AddGetBackwardOneToManyAssociationHandler<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(IServiceCollection services)
    where TAssociateModel : Models.IModel
    where TAssociationModel : Models.IOneToManyAssociation
    where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
    where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
    where TAssociationAddedEvent : Events.IAssociationAddedEvent
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetBackwardOneToManyAssociationOfModels<TAssociateModel, TAssociationModel>,
              IEnumerable<Result<TAssociationModel, Errors>>
                >,
              Handlers.GetBackwardOneToManyAssociationOfModelsHandler<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>
                >();
        }
    }
}