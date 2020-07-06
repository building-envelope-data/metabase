using IQueryBus = Icon.Infrastructure.Queries.IQueryBus;

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