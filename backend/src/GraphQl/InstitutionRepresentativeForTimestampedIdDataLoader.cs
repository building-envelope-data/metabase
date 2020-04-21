using Models = Icon.Models;
using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

namespace Icon.GraphQl
{
    public class InstitutionRepresentativeForTimestampedIdDataLoader
      : ModelForTimestampedIdDataLoader<InstitutionRepresentative, Models.InstitutionRepresentative>
    {
        public InstitutionRepresentativeForTimestampedIdDataLoader(IQueryBus queryBus)
          : base(InstitutionRepresentative.FromModel, queryBus)
        {
        }
    }
}