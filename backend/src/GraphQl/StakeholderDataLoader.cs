using Models = Icon.Models;
using Icon.Infrastructure.Query;

namespace Icon.GraphQl
{
    public sealed class StakeholderDataLoader
      : ModelDataLoader<Stakeholder, Models.Stakeholder>
    {
        public StakeholderDataLoader(IQueryBus queryBus)
          : base(StakeholderBase.FromModel, queryBus)
        {
        }
    }
}