using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using Models = Icon.Models;

namespace Icon.GraphQl
{
    public class InstitutionForTimestampedIdDataLoader
      : ModelForTimestampedIdDataLoader<Institution, Models.Institution>
    {
        public InstitutionForTimestampedIdDataLoader(IQueryBus queryBus)
          : base(Institution.FromModel, queryBus)
        {
        }
    }
}