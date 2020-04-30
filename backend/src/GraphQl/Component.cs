using Models = Icon.Models;
using GreenDonut;
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
            ValueObjects.Id id,
            ComponentInformation information,
            ValueObjects.Timestamp timestamp,
            ValueObjects.Timestamp requestTimestamp
            )
          : base(
              id: id,
              timestamp: timestamp,
              requestTimestamp: requestTimestamp
              )
        {
            Information = information;
        }

        public ComponentManufacturerConnection GetManufacturers(
            [Parent] Component component
            )
        {
            return new ComponentManufacturerConnection(component);
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