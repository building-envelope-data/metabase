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
using AddAssociationCheck = Infrastructure.Models.AddAssociationCheck;
using CancellationToken = System.Threading.CancellationToken;
using Errors = Infrastructure.Errors;
using IAggregate = Infrastructure.Aggregates.IAggregate;
using ModelRepository = Infrastructure.Models.ModelRepository;
using ModelRepositorySession = Infrastructure.Models.ModelRepositorySession;

namespace Database.Configuration
{
    public sealed class QueryCommandAndEventBusses
      : Infrastructure.Configuration.QueryCommandAndEventBusses
    {
        public static new void ConfigureServices(IServiceCollection services)
        {
            Infrastructure.Configuration.QueryCommandAndEventBusses.ConfigureServices(services);

            AddModelOfUnknownTypeHandlers(services);

            AddOpticalDataHandlers(services);
            AddCalorimetricDataHandlers(services);
            AddPhotovoltaicDataHandlers(services);
            AddHygrothermalDataHandlers(services);
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

        private static void AddOpticalDataHandlers(IServiceCollection services)
        {
            AddModelHandlers<Models.OpticalData, Aggregates.OpticalDataAggregate, ValueObjects.CreateOpticalDataInput, Events.OpticalDataCreated>(
                    services,
                    Events.OpticalDataCreated.From,
                    Enumerable.Empty<Func<ModelRepositorySession, Id, Infrastructure.Commands.Create<ValueObjects.CreateOpticalDataInput>, CancellationToken, Task<Result<Id, Errors>>>>(),
                    Events.OpticalDataDeleted.From,
                    Enumerable.Empty<Func<ModelRepositorySession, TimestampedId, Id, CancellationToken, Task<Result<bool, Errors>>>>()
                    );
            AddGetAndHasDataForComponentsHandler<Models.OpticalData, Aggregates.OpticalDataAggregate, Events.OpticalDataCreated>(services);
        }

        private static void AddCalorimetricDataHandlers(IServiceCollection services)
        {
            AddModelHandlers<Models.CalorimetricData, Aggregates.CalorimetricDataAggregate, ValueObjects.CreateCalorimetricDataInput, Events.CalorimetricDataCreated>(
                    services,
                    Events.CalorimetricDataCreated.From,
                    Enumerable.Empty<Func<ModelRepositorySession, Id, Infrastructure.Commands.Create<ValueObjects.CreateCalorimetricDataInput>, CancellationToken, Task<Result<Id, Errors>>>>(),
                    Events.CalorimetricDataDeleted.From,
                    Enumerable.Empty<Func<ModelRepositorySession, TimestampedId, Id, CancellationToken, Task<Result<bool, Errors>>>>()
                    );
            AddGetAndHasDataForComponentsHandler<Models.CalorimetricData, Aggregates.CalorimetricDataAggregate, Events.CalorimetricDataCreated>(services);
        }

        private static void AddPhotovoltaicDataHandlers(IServiceCollection services)
        {
            AddModelHandlers<Models.PhotovoltaicData, Aggregates.PhotovoltaicDataAggregate, ValueObjects.CreatePhotovoltaicDataInput, Events.PhotovoltaicDataCreated>(
                    services,
                    Events.PhotovoltaicDataCreated.From,
                    Enumerable.Empty<Func<ModelRepositorySession, Id, Infrastructure.Commands.Create<ValueObjects.CreatePhotovoltaicDataInput>, CancellationToken, Task<Result<Id, Errors>>>>(),
                    Events.PhotovoltaicDataDeleted.From,
                    Enumerable.Empty<Func<ModelRepositorySession, TimestampedId, Id, CancellationToken, Task<Result<bool, Errors>>>>()
                    );
            AddGetAndHasDataForComponentsHandler<Models.PhotovoltaicData, Aggregates.PhotovoltaicDataAggregate, Events.PhotovoltaicDataCreated>(services);
        }

        private static void AddHygrothermalDataHandlers(IServiceCollection services)
        {
            AddModelHandlers<Models.HygrothermalData, Aggregates.HygrothermalDataAggregate, ValueObjects.CreateHygrothermalDataInput, Events.HygrothermalDataCreated>(
                    services,
                    Events.HygrothermalDataCreated.From,
                    Enumerable.Empty<Func<ModelRepositorySession, Id, Infrastructure.Commands.Create<ValueObjects.CreateHygrothermalDataInput>, CancellationToken, Task<Result<Id, Errors>>>>(),
                    Events.HygrothermalDataDeleted.From,
                    Enumerable.Empty<Func<ModelRepositorySession, TimestampedId, Id, CancellationToken, Task<Result<bool, Errors>>>>()
                    );
            AddGetAndHasDataForComponentsHandler<Models.HygrothermalData, Aggregates.HygrothermalDataAggregate, Events.HygrothermalDataCreated>(services);
        }

        private static void AddGetAndHasDataForComponentsHandler<TDataModel, TDataAggregate, TDataCreatedEvent>(
            IServiceCollection services
            )
          where TDataModel : IModel
          where TDataAggregate : class, IAggregate, IConvertible<TDataModel>, new()
          where TDataCreatedEvent : Events.DataCreatedEvent
        {
            AddGetDataOfComponentsHandler<TDataModel, TDataAggregate, TDataCreatedEvent>(services);
            AddHasDataForComponentsHandler<TDataModel, TDataAggregate, TDataCreatedEvent>(services);
        }

        private static void AddGetDataOfComponentsHandler<TDataModel, TDataAggregate, TDataCreatedEvent>(
            IServiceCollection services
            )
          where TDataModel : IModel
          where TDataAggregate : class, IAggregate, IConvertible<TDataModel>, new()
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
          where TDataModel : IModel
          where TDataAggregate : class, IAggregate, IConvertible<TDataModel>, new()
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
    }
}