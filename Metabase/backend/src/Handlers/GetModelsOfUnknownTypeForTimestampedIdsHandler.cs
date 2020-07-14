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

namespace Metabase.Handlers
{
    public sealed class GetModelsOfUnknownTypeForTimestampedIdsHandler
      : Infrastructure.Handlers.GetModelsOfUnknownTypeForTimestampedIdsHandler
    {
        public GetModelsOfUnknownTypeForTimestampedIdsHandler(IAggregateRepository repository)
          : base(
              repository,
              new Dictionary<Type, IGetModelsForTimestampedIdsHandler>
              {
                  {
                      typeof(Aggregates.ComponentAggregate),
                      new GetModelsForTimestampedIdsHandler<Models.Component, Aggregates.ComponentAggregate>(repository)
                  },
                  {
                      typeof(Aggregates.ComponentConcretizationAggregate),
                      new GetModelsForTimestampedIdsHandler<Models.ComponentConcretization, Aggregates.ComponentConcretizationAggregate>(repository)
                  },
                  {
                      typeof(Aggregates.ComponentManufacturerAggregate),
                      new GetModelsForTimestampedIdsHandler<Models.ComponentManufacturer, Aggregates.ComponentManufacturerAggregate>(repository)
                  },
                  {
                      typeof(Aggregates.ComponentPartAggregate),
                      new GetModelsForTimestampedIdsHandler<Models.ComponentPart, Aggregates.ComponentPartAggregate>(repository)
                  },
                  {
                      typeof(Aggregates.ComponentVariantAggregate),
                      new GetModelsForTimestampedIdsHandler<Models.ComponentVariant, Aggregates.ComponentVariantAggregate>(repository)
                  },
                  {
                      typeof(Aggregates.ComponentVersionAggregate),
                      new GetModelsForTimestampedIdsHandler<Models.ComponentVersion, Aggregates.ComponentVersionAggregate>(repository)
                  },
                  {
                      typeof(Aggregates.DatabaseAggregate),
                      new GetModelsForTimestampedIdsHandler<Models.Database, Aggregates.DatabaseAggregate>(repository)
                  },
                  {
                      typeof(Aggregates.InstitutionAggregate),
                      new GetModelsForTimestampedIdsHandler<Models.Institution, Aggregates.InstitutionAggregate>(repository)
                  },
                  {
                      typeof(Aggregates.InstitutionMethodDeveloperAggregate),
                      new GetModelsForTimestampedIdsHandler<Models.MethodDeveloper, Aggregates.InstitutionMethodDeveloperAggregate>(repository)
                  },
                  {
                      typeof(Aggregates.InstitutionRepresentativeAggregate),
                      new GetModelsForTimestampedIdsHandler<Models.InstitutionRepresentative, Aggregates.InstitutionRepresentativeAggregate>(repository)
                  },
                  {
                      typeof(Aggregates.MethodAggregate),
                      new GetModelsForTimestampedIdsHandler<Models.Method, Aggregates.MethodAggregate>(repository)
                  },
                  {
                      typeof(Aggregates.PersonAffiliationAggregate),
                      new GetModelsForTimestampedIdsHandler<Models.PersonAffiliation, Aggregates.PersonAffiliationAggregate>(repository)
                  },
                  {
                      typeof(Aggregates.PersonAggregate),
                      new GetModelsForTimestampedIdsHandler<Models.Person, Aggregates.PersonAggregate>(repository)
                  },
                  {
                      typeof(Aggregates.PersonMethodDeveloperAggregate),
                      new GetModelsForTimestampedIdsHandler<Models.MethodDeveloper, Aggregates.PersonMethodDeveloperAggregate>(repository)
                  },
                  {
                      typeof(Aggregates.StandardAggregate),
                      new GetModelsForTimestampedIdsHandler<Models.Standard, Aggregates.StandardAggregate>(repository)
                  },
                  {
                      typeof(Aggregates.UserAggregate),
                      new GetModelsForTimestampedIdsHandler<Models.User, Aggregates.UserAggregate>(repository)
                  }
              }
              )
        {
        }
    }
}