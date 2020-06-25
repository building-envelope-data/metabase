using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

namespace Icon.GraphQl
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