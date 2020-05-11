using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using Models = Icon.Models;

namespace Icon.GraphQl
{
    public sealed class InstitutionsAtTimestampDataLoader
      : ModelsAtTimestampDataLoader<Institution, Models.Institution>
    {
        public InstitutionsAtTimestampDataLoader(IQueryBus queryBus)
          : base(Institution.FromModel, queryBus)
        {
        }
    }
}