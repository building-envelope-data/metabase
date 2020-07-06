using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Infrastructure;
using Infrastructure.Aggregates;
using Infrastructure.Commands;
using Infrastructure.Events;
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
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup));

            /* services.AddScoped<MediatR.IMediator, MediatR.Mediator>(); */
            /* services.AddTransient<MediatR.ServiceFactory>(sp => t => sp.GetService(t)); */

            services.AddScoped<IQueryBus, QueryBus>();
            services.AddScoped<ICommandBus, CommandBus>();
            services.AddScoped<IEventBus, EventBus>();

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

            AddOpticalDataFromDatabaseHandlers(services);
            AddCalorimetricDataFromDatabaseHandlers(services);
            AddPhotovoltaicDataFromDatabaseHandlers(services);
            AddHygrothermalDataFromDatabaseHandlers(services);

            // TODO Shall we broadcast events?
            /* services.AddScoped<INotificationHandler<ClientCreated>, ClientsEventHandler>(); */
            /* services.AddScoped<IRequestHandler<CreateClient, Unit>, ClientsCommandHandler>(); */
            /* services.AddScoped<IRequestHandler<GetClientView, ClientView>, ClientsQueryHandler>(); */
        }

        private static void AddModelOfUnknownTypeHandlers(IServiceCollection services)
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetModelsForTimestampedIds<IModel>,
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
                    Enumerable.Empty<Func<IAggregateRepositorySession, Id, Commands.Create<ValueObjects.CreateComponentInput>, CancellationToken, Task<Result<Id, Errors>>>>(),
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
                    new Func<IAggregateRepositorySession, Id, Commands.Create<ValueObjects.CreateDatabaseInput>, CancellationToken, Task<Result<Id, Errors>>>[]
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
                    Enumerable.Empty<Func<IAggregateRepositorySession, Id, Commands.Create<ValueObjects.CreateInstitutionInput>, CancellationToken, Task<Result<Id, Errors>>>>(),
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
                    Enumerable.Empty<Func<IAggregateRepositorySession, Id, Commands.Create<ValueObjects.CreateMethodInput>, CancellationToken, Task<Result<Id, Errors>>>>(),
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
            AddModelHandlers<Models.OpticalData, Aggregates.OpticalDataAggregate, ValueObjects.CreateOpticalDataInput, Events.OpticalDataCreated>(
                    services,
                    Events.OpticalDataCreated.From,
                    Enumerable.Empty<Func<IAggregateRepositorySession, Id, Commands.Create<ValueObjects.CreateOpticalDataInput>, CancellationToken, Task<Result<Id, Errors>>>>(),
                    Events.OpticalDataDeleted.From,
                    Enumerable.Empty<Func<IAggregateRepositorySession, TimestampedId, Id, CancellationToken, Task<Result<bool, Errors>>>>()
                    );
            AddGetAndHasDataForComponentsHandler<Models.OpticalData, Aggregates.OpticalDataAggregate, Events.OpticalDataCreated>(services);
        }

        private static void AddCalorimetricDataHandlers(IServiceCollection services)
        {
            AddModelHandlers<Models.CalorimetricData, Aggregates.CalorimetricDataAggregate, ValueObjects.CreateCalorimetricDataInput, Events.CalorimetricDataCreated>(
                    services,
                    Events.CalorimetricDataCreated.From,
                    Enumerable.Empty<Func<IAggregateRepositorySession, Id, Commands.Create<ValueObjects.CreateCalorimetricDataInput>, CancellationToken, Task<Result<Id, Errors>>>>(),
                    Events.CalorimetricDataDeleted.From,
                    Enumerable.Empty<Func<IAggregateRepositorySession, TimestampedId, Id, CancellationToken, Task<Result<bool, Errors>>>>()
                    );
            AddGetAndHasDataForComponentsHandler<Models.CalorimetricData, Aggregates.CalorimetricDataAggregate, Events.CalorimetricDataCreated>(services);
        }

        private static void AddPhotovoltaicDataHandlers(IServiceCollection services)
        {
            AddModelHandlers<Models.PhotovoltaicData, Aggregates.PhotovoltaicDataAggregate, ValueObjects.CreatePhotovoltaicDataInput, Events.PhotovoltaicDataCreated>(
                    services,
                    Events.PhotovoltaicDataCreated.From,
                    Enumerable.Empty<Func<IAggregateRepositorySession, Id, Commands.Create<ValueObjects.CreatePhotovoltaicDataInput>, CancellationToken, Task<Result<Id, Errors>>>>(),
                    Events.PhotovoltaicDataDeleted.From,
                    Enumerable.Empty<Func<IAggregateRepositorySession, TimestampedId, Id, CancellationToken, Task<Result<bool, Errors>>>>()
                    );
            AddGetAndHasDataForComponentsHandler<Models.PhotovoltaicData, Aggregates.PhotovoltaicDataAggregate, Events.PhotovoltaicDataCreated>(services);
        }

        private static void AddHygrothermalDataHandlers(IServiceCollection services)
        {
            AddModelHandlers<Models.HygrothermalData, Aggregates.HygrothermalDataAggregate, ValueObjects.CreateHygrothermalDataInput, Events.HygrothermalDataCreated>(
                    services,
                    Events.HygrothermalDataCreated.From,
                    Enumerable.Empty<Func<IAggregateRepositorySession, Id, Commands.Create<ValueObjects.CreateHygrothermalDataInput>, CancellationToken, Task<Result<Id, Errors>>>>(),
                    Events.HygrothermalDataDeleted.From,
                    Enumerable.Empty<Func<IAggregateRepositorySession, TimestampedId, Id, CancellationToken, Task<Result<bool, Errors>>>>()
                    );
            AddGetAndHasDataForComponentsHandler<Models.HygrothermalData, Aggregates.HygrothermalDataAggregate, Events.HygrothermalDataCreated>(services);
        }

        private static void AddGetAndHasDataForComponentsHandler<TDataModel, TDataAggregate, TDataCreatedEvent>(
            IServiceCollection services
            )
          where TDataAggregate : class, IEventSourcedAggregate, IConvertible<TDataModel>, new()
          where TDataCreatedEvent : Events.DataCreatedEvent
        {
            AddGetDataOfComponentsHandler<TDataModel, TDataAggregate, TDataCreatedEvent>(services);
            AddHasDataForComponentsHandler<TDataModel, TDataAggregate, TDataCreatedEvent>(services);
        }

        private static void AddGetDataOfComponentsHandler<TDataModel, TDataAggregate, TDataCreatedEvent>(
            IServiceCollection services
            )
          where TDataAggregate : class, IEventSourcedAggregate, IConvertible<TDataModel>, new()
          where TDataCreatedEvent : Events.DataCreatedEvent
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.GetDataOfComponents<TDataModel>,
              IEnumerable<Result<IEnumerable<Result<TDataModel, Errors>>, Errors>>
                >,
              Handlers.GetDataOfComponentsHandler<TDataModel, TDataAggregate, TDataCreatedEvent>
                >();
        }

        private static void AddHasDataForComponentsHandler<TDataModel, TDataAggregate, TDataCreatedEvent>(
            IServiceCollection services
            )
          where TDataAggregate : class, IEventSourcedAggregate, IConvertible<TDataModel>, new()
          where TDataCreatedEvent : Events.DataCreatedEvent
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Queries.HasDataForComponents<TDataModel>,
              IEnumerable<Result<bool, Errors>>
                >,
              Handlers.HasDataForComponentsHandler<TDataModel, TDataAggregate, TDataCreatedEvent>
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

        private static void AddCalorimetricDataFromDatabaseHandlers(IServiceCollection services)
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

        private static void AddPhotovoltaicDataFromDatabaseHandlers(IServiceCollection services)
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

        private static void AddHygrothermalDataFromDatabaseHandlers(IServiceCollection services)
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
                    Enumerable.Empty<Func<IAggregateRepositorySession, Id, Commands.Create<ValueObjects.CreatePersonInput>, CancellationToken, Task<Result<Id, Errors>>>>(),
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
                    Enumerable.Empty<Func<IAggregateRepositorySession, Id, Commands.Create<ValueObjects.CreateStandardInput>, CancellationToken, Task<Result<Id, Errors>>>>(),
                    Events.StandardDeleted.From,
                    Enumerable.Empty<Func<IAggregateRepositorySession, TimestampedId, Id, CancellationToken, Task<Result<bool, Errors>>>>()
                    );
        }

        private static void AddModelHandlers<TModel, TAggregate, TCreateInput, TCreatedEvent>(
                IServiceCollection services,
                Func<Guid, Commands.Create<TCreateInput>, ICreatedEvent> newCreatedEvent,
                IEnumerable<Func<IAggregateRepositorySession, Id, Commands.Create<TCreateInput>, CancellationToken, Task<Result<Id, Errors>>>> addAssociations,
                Func<Commands.Delete<TModel>, IDeletedEvent> newDeletedEvent,
                IEnumerable<Func<IAggregateRepositorySession, TimestampedId, Id, CancellationToken, Task<Result<bool, Errors>>>> removeAssociations
                    )
                where TModel : IModel
                where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
                where TCreatedEvent : ICreatedEvent
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
                Func<Guid, Commands.Create<TInput>, ICreatedEvent> newEvent,
                IEnumerable<Func<IAggregateRepositorySession, Id, Commands.Create<TInput>, CancellationToken, Task<Result<Id, Errors>>>> addAssociations
        )
          where TAggregate : class, IEventSourcedAggregate, new()
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.Create<TInput>,
              Result<TimestampedId, Errors>
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
                Func<Commands.Delete<TModel>, IDeletedEvent> newEvent,
                IEnumerable<Func<IAggregateRepositorySession, TimestampedId, Id, CancellationToken, Task<Result<bool, Errors>>>> removeAssociations
        )
          where TAggregate : class, IEventSourcedAggregate, new()
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.Delete<TModel>,
              Result<TimestampedId, Errors>
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
                where TCreatedEvent : ICreatedEvent
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
                where TModel : IModel
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
                        Func<Guid, Commands.AddAssociation<TAddInput>, IAssociationAddedEvent> newAssociationAddedEvent,
                        Func<Guid, Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>, IAssociationRemovedEvent> newAssociationRemovedEvent
                        )
            where TAssociationModel : IManyToManyAssociation
            where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociationAddedEvent : IAssociationAddedEvent
            where TAddInput : ValueObjects.AddManyToManyAssociationInput
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
              Commands.AddAssociation<ValueObjects.AddMethodDeveloperInput>,
              Result<TimestampedId, Errors>
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
                        Func<Guid, Commands.AddAssociation<TAddInput>, IAssociationAddedEvent> newAssociationAddedEvent,
                        Func<Guid, Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>, IAssociationRemovedEvent> newAssociationRemovedEvent
                        )
            where TModel : IModel
            where TAssociationModel : IManyToManyAssociation
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
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

        private static void AddManyToManyAssociationHandlersWithAssociationGetters<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent, TAddInput, TAssociationRemovedEvent>(
                        IServiceCollection services,
                        Func<Guid, Commands.AddAssociation<TAddInput>, IAssociationAddedEvent> newAssociationAddedEvent,
                        Func<Guid, Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>, IAssociationRemovedEvent> newAssociationRemovedEvent
                        )
                    where TModel : IModel
                    where TAssociationModel : IManyToManyAssociation
                    where TAssociateModel : IModel
                    where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
                    where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
                    where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
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

        private static void AddManyToManyAssociationHandlers<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent, TAddInput, TAssociationRemovedEvent>(
                        IServiceCollection services,
                        Func<Guid, Commands.AddAssociation<TAddInput>, IAssociationAddedEvent> newAssociationAddedEvent,
                        Func<Guid, Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>, IAssociationRemovedEvent> newAssociationRemovedEvent
                        )
                    where TModel : IModel
                    where TAssociationModel : IManyToManyAssociation
                    where TAssociateModel : IModel
                    where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
                    where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
                    where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
                    where TAssociationAddedEvent : IAssociationAddedEvent
                    where TAddInput : ValueObjects.AddManyToManyAssociationInput
                    where TAssociationRemovedEvent : IAssociationRemovedEvent
        {
            AddAddManyToManyAssociationHandler<TAddInput, TAggregate, TAssociationAggregate, TAssociateAggregate>(services, newAssociationAddedEvent);
            AddRemoveManyToManyAssociationHandler<TAssociationModel, TAssociationAggregate>(services, newAssociationRemovedEvent);
            AddGetHandler<TAssociationModel, TAssociationAggregate>(services);
            AddGetManyToManyAssociatesHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(services);
        }

        private static void AddAddManyToManyAssociationHandler<TInput, TAggregate, TAssociationAggregate, TAssociateAggregate>(
                IServiceCollection services,
                Func<Guid, Commands.AddAssociation<TInput>, IAssociationAddedEvent> newEvent
                )
            where TInput : ValueObjects.AddManyToManyAssociationInput
            where TAggregate : class, IEventSourcedAggregate, new()
            where TAssociationAggregate : class, IManyToManyAssociationAggregate, new()
            where TAssociateAggregate : class, IEventSourcedAggregate, new()
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.AddAssociation<TInput>,
              Result<TimestampedId, Errors>
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
                Func<Guid, Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>, IAssociationRemovedEvent> newEvent
                )
            where TAssociationAggregate : class, IManyToManyAssociationAggregate, new()
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<TAssociationModel>>,
              Result<TimestampedId, Errors>
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
                        Func<Guid, Commands.AddAssociation<TAddInput>, IAssociationAddedEvent> newAssociationAddedEvent,
                        Func<Guid, Commands.RemoveAssociation<ValueObjects.RemoveOneToManyAssociationInput<TAssociationModel>>, IAssociationRemovedEvent> newAssociationRemovedEvent
                        )
                    where TModel : IModel
                    where TAssociationModel : IOneToManyAssociation
                    where TAssociateModel : IModel
                    where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
                    where TAssociationAggregate : class, IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
                    where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
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

        private static void AddOneToManyAssociationHandlers<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent, TAddInput, TAssociationRemovedEvent>(
                        IServiceCollection services,
                        Func<Guid, Commands.AddAssociation<TAddInput>, IAssociationAddedEvent> newAssociationAddedEvent,
                        Func<Guid, Commands.RemoveAssociation<ValueObjects.RemoveOneToManyAssociationInput<TAssociationModel>>, IAssociationRemovedEvent> newAssociationRemovedEvent
                        )
                    where TModel : IModel
                    where TAssociationModel : IOneToManyAssociation
                    where TAssociateModel : IModel
                    where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
                    where TAssociationAggregate : class, IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
                    where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
                    where TAssociationAddedEvent : IAssociationAddedEvent
                    where TAddInput : ValueObjects.AddOneToManyAssociationInput
                    where TAssociationRemovedEvent : IAssociationRemovedEvent
        {
            AddAddOneToManyAssociationHandler<TAddInput, TAggregate, TAssociationAggregate, TAssociateAggregate>(services, newAssociationAddedEvent);
            AddRemoveOneToManyAssociationHandler<TAssociationModel, TAssociationAggregate>(services, newAssociationRemovedEvent);
            AddGetHandler<TAssociationModel, TAssociationAggregate>(services);
            AddGetOneToManyAssociatesHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(services);
        }

        private static void AddAddOneToManyAssociationHandler<TInput, TAggregate, TAssociationAggregate, TAssociateAggregate>(
                IServiceCollection services,
                Func<Guid, Commands.AddAssociation<TInput>, IAssociationAddedEvent> newEvent
                )
            where TInput : ValueObjects.AddOneToManyAssociationInput
            where TAggregate : class, IEventSourcedAggregate, new()
            where TAssociationAggregate : class, IOneToManyAssociationAggregate, new()
            where TAssociateAggregate : class, IEventSourcedAggregate, new()
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.AddAssociation<TInput>,
              Result<TimestampedId, Errors>
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
                Func<Guid, Commands.RemoveAssociation<ValueObjects.RemoveOneToManyAssociationInput<TAssociationModel>>, IAssociationRemovedEvent> newEvent
                )
            where TAssociationAggregate : class, IOneToManyAssociationAggregate, new()
        {
            services.AddScoped<
              MediatR.IRequestHandler<
              Commands.RemoveAssociation<ValueObjects.RemoveOneToManyAssociationInput<TAssociationModel>>,
              Result<TimestampedId, Errors>
                >,
              Handlers.RemoveOneToManyAssociationHandler<TAssociationModel, TAssociationAggregate>>(serviceProvider =>
                  new Handlers.RemoveOneToManyAssociationHandler<TAssociationModel, TAssociationAggregate>(
                    serviceProvider.GetRequiredService<IAggregateRepository>(),
                    newEvent
                    )
                  );
        }

        private static void AddGetHandler<TModel, TAggregate>(IServiceCollection services)
            where TModel : IModel
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
            where TModel : IModel
            where TAssociationModel : IManyToManyAssociation
            where TAssociateModel : IModel
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
            where TAssociationAddedEvent : IAssociationAddedEvent
        {
            AddGetForwardManyToManyAssociatesHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(services);
            AddGetBackwardManyToManyAssociatesHandler<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate, TAssociationAddedEvent>(services);
        }

        private static void AddGetForwardManyToManyAssociatesHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(IServiceCollection services)
            where TModel : IModel
            where TAssociationModel : IManyToManyAssociation
            where TAssociateModel : IModel
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
            where TAssociationAddedEvent : IAssociationAddedEvent
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
            where TAssociateModel : IModel
            where TAssociationModel : IManyToManyAssociation
            where TModel : IModel
            where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
            where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAssociationAddedEvent : IAssociationAddedEvent
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
    where TModel : IModel
    where TAssociateModel : IModel
    where TAssociationModel : IManyToManyAssociation
    where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
    where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
    where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
    where TAssociationAddedEvent : IAssociationAddedEvent
        {
            AddGetForwardManyToManyAssociationsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(services);
            AddGetBackwardManyToManyAssociationsHandler<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(services);
        }

        private static void AddGetForwardManyToManyAssociationsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(IServiceCollection services)
    where TModel : IModel
    where TAssociationModel : IManyToManyAssociation
    where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
    where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
    where TAssociationAddedEvent : IAssociationAddedEvent
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
    where TAssociateModel : IModel
    where TAssociationModel : IManyToManyAssociation
    where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
    where TAssociationAggregate : class, IManyToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
    where TAssociationAddedEvent : IAssociationAddedEvent
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
            where TModel : IModel
            where TAssociationModel : IOneToManyAssociation
            where TAssociateModel : IModel
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
            where TAssociationAddedEvent : IAssociationAddedEvent
        {
            AddGetForwardOneToManyAssociatesHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(services);
            AddGetBackwardOneToManyAssociateHandler<TAssociateModel, TAssociationModel, TModel, TAssociateAggregate, TAssociationAggregate, TAggregate, TAssociationAddedEvent>(services);
        }

        private static void AddGetForwardOneToManyAssociatesHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate, TAssociationAddedEvent>(IServiceCollection services)
            where TModel : IModel
            where TAssociationModel : IOneToManyAssociation
            where TAssociateModel : IModel
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAssociationAggregate : class, IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
            where TAssociationAddedEvent : IAssociationAddedEvent
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
            where TAssociateModel : IModel
            where TAssociationModel : IOneToManyAssociation
            where TModel : IModel
            where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
            where TAssociationAggregate : class, IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
            where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
            where TAssociationAddedEvent : IAssociationAddedEvent
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
    where TModel : IModel
    where TAssociateModel : IModel
    where TAssociationModel : IOneToManyAssociation
    where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
    where TAssociationAggregate : class, IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
    where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
    where TAssociationAddedEvent : IAssociationAddedEvent
        {
            AddGetForwardOneToManyAssociationsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(services);
            AddGetBackwardOneToManyAssociationHandler<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(services);
        }

        private static void AddGetForwardOneToManyAssociationsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate, TAssociationAddedEvent>(IServiceCollection services)
    where TModel : IModel
    where TAssociationModel : IOneToManyAssociation
    where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
    where TAssociationAggregate : class, IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
    where TAssociationAddedEvent : IAssociationAddedEvent
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
    where TAssociateModel : IModel
    where TAssociationModel : IOneToManyAssociation
    where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
    where TAssociationAggregate : class, IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
    where TAssociationAddedEvent : IAssociationAddedEvent
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