using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.Handlers;
using Infrastructure.Models;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using CancellationToken = System.Threading.CancellationToken;
using ErrorCodes = Infrastructure.ErrorCodes;
using Errors = Infrastructure.Errors;
using Exception = System.Exception;
using Type = System.Type;

namespace Database.Handlers
{
    public sealed class GetModelsOfUnknownTypeForTimestampedIdsHandler
      : Infrastructure.Handlers.GetModelsOfUnknownTypeForTimestampedIdsHandler
    {
        public GetModelsOfUnknownTypeForTimestampedIdsHandler(IModelRepository repository)
          : base(
              repository,
              new Dictionary<Type, IGetModelsForTimestampedIdsHandler>
              {
                {
                  typeof(Aggregates.CalorimetricDataAggregate),
                  new GetModelsForTimestampedIdsHandler<Models.CalorimetricData, Aggregates.CalorimetricDataAggregate>(repository)
                },
                {
                  typeof(Aggregates.HygrothermalDataAggregate),
                  new GetModelsForTimestampedIdsHandler<Models.HygrothermalData, Aggregates.HygrothermalDataAggregate>(repository)
                },
                {
                  typeof(Aggregates.OpticalDataAggregate),
                  new GetModelsForTimestampedIdsHandler<Models.OpticalData, Aggregates.OpticalDataAggregate>(repository)
                },
                {
                  typeof(Aggregates.PhotovoltaicDataAggregate),
                  new GetModelsForTimestampedIdsHandler<Models.PhotovoltaicData, Aggregates.PhotovoltaicDataAggregate>(repository)
                }
              }
              )
        {
        }
    }
}