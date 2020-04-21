using Guid = System.Guid;
using Models = Icon.Models;
using DateTime = System.DateTime;
using CancellationToken = System.Threading.CancellationToken;
using HotChocolate;
using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using IResolverContext = HotChocolate.Resolvers.IResolverContext;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;

namespace Icon.GraphQl
{
    public sealed class Institution
      : NodeBase, Stakeholder
    {
        public static Institution FromModel(
            Models.Institution model,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            return new Institution(
                id: model.Id,
                information: InstitutionInformation.FromModel(model.Information),
                publicKey: model.PublicKey?.Value,
                state: model.State,
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public InstitutionInformation Information { get; }
        public string? PublicKey { get; }
        public ValueObjects.InstitutionState State { get; }

        public Institution(
            Guid id,
            InstitutionInformation information,
            string? publicKey,
            ValueObjects.InstitutionState state,
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
            PublicKey = publicKey;
            State = state;
        }

        public Task<IEnumerable<Component>> GetComponents(
            [Parent] Institution institution,
            IResolverContext context
            )
        {
            return null!;
        }

        [UsePaging]
        public Task<IReadOnlyList<Method>> GetMethods(
            [Parent] Institution institution,
            [DataLoader] MethodsDevelopedByInstitutionIdentifiedByTimestampedIdDataLoader methodsLoader,
            IResolverContext context
            )
        {
            return methodsLoader.LoadAsync(
                TimestampId(institution.Id, GraphQl.Timestamp.Fetch(context)),
                default(CancellationToken)
                );
        }

        public sealed class MethodsDevelopedByInstitutionIdentifiedByTimestampedIdDataLoader
            : AssociatesOfModelIdentifiedByTimestampedIdDataLoader<Method, Models.Institution, Models.Method>
        {
            public MethodsDevelopedByInstitutionIdentifiedByTimestampedIdDataLoader(IQueryBus queryBus)
              : base(Method.FromModel, queryBus)
            {
            }
        }

        // TODO Return role information associated with representatives!
        public Task<IReadOnlyList<User>> GetRepresentatives(
            IEnumerable<ValueObjects.InstitutionRepresentativeRole>? roles,
            [Parent] Institution institution,
            [DataLoader] RepresentativesOfInstitutionIdentifiedByTimestampedIdDataLoader representativesLoader,
            IResolverContext context
            )
        {
            return representativesLoader.LoadAsync(
                TimestampId(institution.Id, GraphQl.Timestamp.Fetch(context)),
                default(CancellationToken)
                );
        }

        public sealed class RepresentativesOfInstitutionIdentifiedByTimestampedIdDataLoader
            : AssociatesOfModelIdentifiedByTimestampedIdDataLoader<User, Models.Institution, Models.User>
        {
            public RepresentativesOfInstitutionIdentifiedByTimestampedIdDataLoader(IQueryBus queryBus)
              : base(User.FromModel, queryBus)
            {
            }
        }
    }
}