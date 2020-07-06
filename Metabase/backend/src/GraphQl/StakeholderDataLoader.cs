using Infrastructure.Queries;

namespace Metabase.GraphQl
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