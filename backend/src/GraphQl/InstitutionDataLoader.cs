using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using Models = Icon.Models;

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