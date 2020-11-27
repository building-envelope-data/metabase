using Infrastructure.GraphQl;
using Infrastructure.Queries;

namespace Metabase.GraphQl
{
    public sealed class StakeholderDataLoader
      : ModelDataLoader<IStakeholder, Models.Stakeholder>
    {
        public StakeholderDataLoader(IQueryBus queryBus)
          : base(StakeholderBase.FromModel, queryBus)
        {
        }
    }
}