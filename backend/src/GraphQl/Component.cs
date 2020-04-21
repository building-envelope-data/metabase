using Guid = System.Guid;
using Models = Icon.Models;
using DateTime = System.DateTime;
using CancellationToken = System.Threading.CancellationToken;
using HotChocolate;
using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using IResolverContext = HotChocolate.Resolvers.IResolverContext;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Icon.GraphQl
{
    public sealed class Component
      : NodeBase
    {
        public static Component FromModel(
            Models.Component model,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            return new Component(
                id: model.Id,
                information: ComponentInformation.FromModel(model.Information),
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public ComponentInformation Information { get; }

        public Component(
            Guid id,
            ComponentInformation information,
            DateTime timestamp,
            DateTime requestTimestamp
            )
          : base(
              id: id,
              timestamp: timestamp,
              requestTimestamp: requestTimestamp
              )
        {
            Information = information;
        }

        public Task<ComponentVersion[]> GetVersions(
            [Parent] Component component,
            IResolverContext context
            )
        {
            return null!;
            /* var timestamp = Timestamp.Fetch(context); */
            /* var timestampedComponentId = */
            /*   ResultHelpers.HandleFailure( */
            /*       ValueObjects.TimestampedId.From( */
            /*         component.Id, timestamp */
            /*         ) */
            /*       ); */
            /* return MakeGroupDataLoader(context) */
            /*   .LoadAsync(timestampedComponentId, default(CancellationToken)); */
        }

        /* private GreenDonut.IDataLoader<ValueObjects.TimestampedId, ComponentVersion[]> MakeGroupDataLoader(IResolverContext context) */
        /* { */
        /*     return context.GroupDataLoader<ValueObjects.TimestampedId, ComponentVersion>( */
        /*         "componentVersionsGroupedByTimestampedComponentId", */
        /*         async timestampedComponentIds => */
        /*         { */
        /*             var query = */
        /*         ResultHelpers.HandleFailure( */
        /*             Queries.ListComponentVersions.From( */
        /*               timestampedComponentIds */
        /*               ) */
        /*             ); */
        /*             return ResultHelpers.HandleFailures<ValueObjects.TimestampedId, Models.ComponentVersion>( */
        /*             await QueryBus.Send< */
        /*             Queries.ListComponentVersions, */
        /*             ILookup<ValueObjects.TimestampedId, Result<Models.ComponentVersion, Errors>> */
        /*             >(query) */
        /*             ) */
        /*         .Select(grouping => (grouping.Key, grouping.Select(c => ComponentVersion.FromModel(c, grouping.Key.Timestamp)))) */
        /*         .ToLookup(t => t.Item1, t => t.Item2); */
        /*         } */
        /*         ); */
        /* } */

        public Task<IEnumerable<ComponentManufacturer>> GetManufacturers(
            [Parent] Component component,
            IResolverContext context
            )
        {
            return null!;
        }

        public Task<IEnumerable<Component>> GetSuperComponents(
            [Parent] Component component,
            IResolverContext context
            )
        {
            return null!;
        }

        public Task<IEnumerable<Component>> GetSubComponents(
            [Parent] Component component,
            IResolverContext context
            )
        {
            return null!;
        }
    }
}