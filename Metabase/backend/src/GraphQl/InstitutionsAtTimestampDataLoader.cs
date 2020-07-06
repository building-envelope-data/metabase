using Infrastructure.ValueObjects;
using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
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