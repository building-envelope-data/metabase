using Icon.Infrastructure.Query;
using Models = Icon.Models;

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