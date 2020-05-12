using Models = Icon.Models;
using DateTime = System.DateTime;
using IResolverContext = HotChocolate.Resolvers.IResolverContext;
using System.Collections.Generic;
using HotChocolate;
using GreenDonut;
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
            ValueObjects.Id id,
            MethodInformation information,
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

        public Task<IReadOnlyList<Stakeholder>> GetDevelopers(
            [Parent] Method method,
            [DataLoader] DevelopersOfMethodDataLoader developersLoader
            )
        {
            return developersLoader.LoadAsync(
                TimestampHelpers.TimestampId(method.Id, method.RequestTimestamp)
                );
        }

        public sealed class DevelopersOfMethodDataLoader
            : ForwardManyToManyAssociatesOfModelDataLoader<Stakeholder, Models.Method, Models.MethodDeveloper, Models.Stakeholder>
        {
            public DevelopersOfMethodDataLoader(IQueryBus queryBus)
              : base(StakeholderBase.FromModel, queryBus)
            {
            }
        }
    }
}