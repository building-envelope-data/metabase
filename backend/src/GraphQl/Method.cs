using Guid = System.Guid;
using Models = Icon.Models;
using DateTime = System.DateTime;
using IResolverContext = HotChocolate.Resolvers.IResolverContext;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

namespace Icon.GraphQl
{
    public sealed class Method
      : NodeBase
    {
        public static Method FromModel(
            Models.Method model,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            return new Method(
                id: model.Id,
                information: MethodInformation.FromModel(model.Information),
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public MethodInformation Information { get; }

        public Method(
            Guid id,
            MethodInformation information,
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

        public Task<IReadOnlyList<Stakeholder>> GetDevelopers(
            [Parent] Method method,
            [DataLoader] DevelopersOfMethodIdentifiedByTimestampedIdDataLoader developersLoader,
            IResolverContext context
            )
        {
            return developersLoader.LoadAsync(
                TimestampId(method.Id, GraphQl.Timestamp.Fetch(context)),
                default(CancellationToken)
                );
        }

        public sealed class DevelopersOfMethodIdentifiedByTimestampedIdDataLoader
            : AssociatesOfModelIdentifiedByTimestampedIdDataLoader<Stakeholder, Models.Method, Models.Stakeholder>
        {
            public DevelopersOfMethodIdentifiedByTimestampedIdDataLoader(IQueryBus queryBus)
              : base(StakeholderBase.FromModel, queryBus)
            {
            }
        }
    }
}