using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.ValueObjects;
using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
{
    public sealed class RepresentedInstitutionConnection
      : Connection
    {
        public RepresentedInstitutionConnection(
            User user
            )
          : base(
              fromId: user.Id,
              pageInfo: null!,
              requestTimestamp: user.RequestTimestamp
              )
        {
        }

        public async Task<IReadOnlyList<RepresentedInstitutionEdge>> GetEdges(
            [DataLoader] InstitutionsRepresentedByUserAssociationDataLoader representedInstitutionsLoader
            )
        {
            return (await representedInstitutionsLoader.LoadAsync(
                  TimestampHelpers.TimestampId(FromId, RequestTimestamp)
                  )
                .ConfigureAwait(false)
                )
              .Select(a => new RepresentedInstitutionEdge(a))
              .ToList().AsReadOnly();
        }

        public sealed class InstitutionsRepresentedByUserAssociationDataLoader
            : BackwardManyToManyAssociationsOfModelDataLoader<InstitutionRepresentative, Models.User, Models.InstitutionRepresentative>
        {
            public InstitutionsRepresentedByUserAssociationDataLoader(IQueryBus queryBus)
              : base(InstitutionRepresentative.FromModel, queryBus)
            {
            }
        }

        public Task<IReadOnlyList<Institution>> GetNodes(
            [DataLoader] InstitutionsRepresentedByUserDataLoader representedInstitutionsLoader
            )
        {
            return representedInstitutionsLoader.LoadAsync(
                TimestampHelpers.TimestampId(FromId, RequestTimestamp)
                );
        }

        public sealed class InstitutionsRepresentedByUserDataLoader
            : BackwardManyToManyAssociatesOfModelDataLoader<Institution, Models.User, Models.InstitutionRepresentative, Models.Institution>
        {
            public InstitutionsRepresentedByUserDataLoader(IQueryBus queryBus)
              : base(Institution.FromModel, queryBus)
            {
            }
        }
    }
}