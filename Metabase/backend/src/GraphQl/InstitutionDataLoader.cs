using Infrastructure.GraphQl;
using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
{
    public sealed class InstitutionDataLoader
      : ModelDataLoader<Institution, Models.Institution>
    {
        public InstitutionDataLoader(IQueryBus queryBus)
          : base(Institution.FromModel, queryBus)
        {
        }
    }
}