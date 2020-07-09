using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Infrastructure;
using Infrastructure.Aggregates;
using Infrastructure.Commands;
using Infrastructure.Events;
using Infrastructure.Handlers;
using Infrastructure.Models;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using AddAssociationCheck = Infrastructure.Aggregates.AddAssociationCheck;
using CancellationToken = System.Threading.CancellationToken;
using Errors = Infrastructure.Errors;
using IAggregateRepository = Infrastructure.Aggregates.IAggregateRepository;
using IAggregateRepositorySession = Infrastructure.Aggregates.IAggregateRepositorySession;
using IEventSourcedAggregate = Infrastructure.Aggregates.IEventSourcedAggregate;

namespace Metabase.Configuration
{
    public sealed class QueryCommandAndEventBusses
      : Infrastructure.Configuration.QueryCommandAndEventBusses
    {
        public static new void ConfigureServices(IServiceCollection services)
        {
            Infrastructure.Configuration.QueryCommandAndEventBusses.ConfigureServices(services);

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
            AddInstitutionOperatedDatabaseHandlers(services);
            AddInstitutionRepresentativeHandlers(services);
            AddMethodDeveloperHandlers(services);
            AddPersonAffiliationHandlers(services);

            AddOpticalDataHandlers(services);
            AddCalorimetricDataHandlers(services);
            AddPhotovoltaicDataHandlers(services);
            AddHygrothermalDataHandlers(services);

            // TODO Shall we broadcast events?
            /* services.AddScoped<INotificationHandler<ClientCreated>, ClientsEventHandler>(); */
            /* services.AddScoped<IRequestHandler<CreateClient, Unit>, ClientsCommandHandler>(); */
            /* services.AddScoped<IRequestHandler<GetClientView, ClientView>, ClientsQueryHandler>(); */
        }

        private static void AddModelOfUnknownTypeHandlers(IServiceCollection services)
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              GetModelsForTimestampedIds<IModel>,
              IEnumerable<Result<IModel, Errors>>
                >,
              Handlers.GetModelsOfUnknownTypeForTimestampedIdsHandler
                >();
        }

        private static void AddComponentHandlers(IServiceCollection services)
        {
            AddModelHandlers<Models.Component, Aggregates.ComponentAggregate, ValueObjects.CreateComponentInput, Events.ComponentCreated>(
                    services,
                    Events.ComponentCreated.From,
                    Enumerable.Empty<Func<IAggregateRepositorySession, Id, Infrastructure.Commands.Create<ValueObjects.CreateComponentInput>, CancellationToken, Task<Result<Id, Errors>>>>(),
                    Events.ComponentDeleted.From,
                    new Func<IAggregateRepositorySession, TimestampedId, Id, CancellationToken, Task<Result<bool, Errors>>>[]
                    {
                    (session, timestampedId, creatorId, cancellationToken) =>
                    session.RemoveForwardManyToManyAssociationsOfModel<Models.Component, Models.ComponentManufacturer, Aggregates.ComponentAggregate, Aggregates.ComponentManufacturerAggregate, Events.ComponentManufacturerAdded>(
                        timestampedId,
                        componentManufacturerId => new Events.ComponentManufacturerRemoved(componentManufacturerId, creatorId),
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
                    new Func<IAggregateRepositorySession, Id, Infrastructure.Commands.Create<ValueObjects.CreateDatabaseInput>, CancellationToken, Task<Result<Id, Errors>>>[]
                    {
                    (session, databaseId, command, cancellationToken) =>
                    session.AddOneToManyAssociation<Aggregates.InstitutionAggregate, Aggregates.InstitutionOperatedDatabaseAggregate, Aggregates.DatabaseAggregate>(
                        id => Events.InstitutionOperatedDatabaseAdded.From(id, databaseId, command),
                        AddAssociationCheck.PARENT,
                        cancellationToken
                        )
                    },
                    Events.DatabaseDeleted.From,
                    new Func<IAggregateRepositorySession, TimestampedId, Id, CancellationToken, Task<Result<bool, Errors>>>[]
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
                    Enumerable.Empty<Func<IAggregateRepositorySession, Id, Infrastructure.Commands.Create<ValueObjects.CreateInstitutionInput>, CancellationToken, Task<Result<Id, Errors>>>>(),
                    Events.InstitutionDeleted.From,
                    new Func<IAggregateRepositorySession, TimestampedId, Id, CancellationToken, Task<Result<bool, Errors>>>[]
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
                    Enumerable.Empty<Func<IAggregateRepositorySession, Id, Infrastructure.Commands.Create<ValueObjects.CreateMethodInput>, CancellationToken, Task<Result<Id, Errors>>>>(),
                    Events.MethodDeleted.From,
                    new Func<IAggregateRepositorySession, TimestampedId, Id, CancellationToken, Task<Result<bool, Errors>>>[]
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

        private static void AddCalorimetricDataHandlers(IServiceCollection services)
        {
            // Get
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.QueryDataArrayOfComponentsFromDatabases<Models.CalorimetricDataFromDatabase>,
              IEnumerable<Result<IEnumerable<Result<Models.CalorimetricDataFromDatabase, Errors>>, Errors>>
                >,
              Handlers.QueryCalorimetricDataOfComponentsFromDatabasesHandler
                >();

            // Who Has
            AddWhichDatabasesHaveDataForComponentsHandler<Models.CalorimetricDataFromDatabase>(services, "hasCalorimetricData"); // TODO Get rid of magical string!
        }

        private static void AddPhotovoltaicDataHandlers(IServiceCollection services)
        {
            // Get
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.QueryDataArrayOfComponentsFromDatabases<Models.PhotovoltaicDataFromDatabase>,
              IEnumerable<Result<IEnumerable<Result<Models.PhotovoltaicDataFromDatabase, Errors>>, Errors>>
                >,
              Handlers.QueryPhotovoltaicDataOfComponentsFromDatabasesHandler
                >();

            // Who Has
            AddWhichDatabasesHaveDataForComponentsHandler<Models.PhotovoltaicDataFromDatabase>(services, "hasPhotovoltaicData"); // TODO Get rid of magical string!
        }

        private static void AddHygrothermalDataHandlers(IServiceCollection services)
        {
            // Get
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.QueryDataArrayOfComponentsFromDatabases<Models.HygrothermalDataFromDatabase>,
              IEnumerable<Result<IEnumerable<Result<Models.HygrothermalDataFromDatabase, Errors>>, Errors>>
                >,
              Handlers.QueryHygrothermalDataOfComponentsFromDatabasesHandler
                >();

            // Who Has
            AddWhichDatabasesHaveDataForComponentsHandler<Models.HygrothermalDataFromDatabase>(services, "hasHygrothermalData"); // TODO Get rid of magical string!
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
                    Enumerable.Empty<Func<IAggregateRepositorySession, Id, Infrastructure.Commands.Create<ValueObjects.CreatePersonInput>, CancellationToken, Task<Result<Id, Errors>>>>(),
                    Events.PersonDeleted.From,
                    new Func<IAggregateRepositorySession, TimestampedId, Id, CancellationToken, Task<Result<bool, Errors>>>[]
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
              GetModelsForTimestampedIds<Models.Stakeholder>,
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
                    Enumerable.Empty<Func<IAggregateRepositorySession, Id, Infrastructure.Commands.Create<ValueObjects.CreateStandardInput>, CancellationToken, Task<Result<Id, Errors>>>>(),
                    Events.StandardDeleted.From,
                    Enumerable.Empty<Func<IAggregateRepositorySession, TimestampedId, Id, CancellationToken, Task<Result<bool, Errors>>>>()
                    );
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
                        Func<Guid, Infrastructure.Commands.AddAssociation<TAddInput>, IAssociationAddedEvent> newAssociationAddedEvent,
                        Func<Guid, Infrastructure.Commands.RemoveAssociation<Infrastructure.ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>, IAssociationRemovedEvent> newAssociationRemovedEvent
                        )
            where TAssociationModel : IManyToManyAssociation
            where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociationAddedEvent : IAssociationAddedEvent
            where TAddInput : Infrastructure.ValueObjects.AddManyToManyAssociationInput
            where TAssociationRemovedEvent : IAssociationRemovedEvent
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
              Infrastructure.Commands.AddAssociation<ValueObjects.AddMethodDeveloperInput>,
              Result<TimestampedId, Errors>
                >,
              Handlers.AddMethodDeveloperHandler>();

            // TODO Remove

            // Get
            services.AddScoped<
              MediatR.IRequestHandler<
              GetModelsForTimestampedIds<Models.MethodDeveloper>,
              IEnumerable<Result<Models.MethodDeveloper, Errors>>
                >,
              Handlers.GetMethodDevelopersHandler
                >();

            // Get associates
            services.AddScoped<
              MediatR.IRequestHandler<
              GetForwardManyToManyAssociatesOfModels<Models.Method, Models.MethodDeveloper, Models.Stakeholder>,
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
    }
}