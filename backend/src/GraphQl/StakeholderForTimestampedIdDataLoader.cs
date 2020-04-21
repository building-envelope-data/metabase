using Models = Icon.Models;
using Icon.Infrastructure.Query;

namespace Icon.GraphQl
{
    public class StakeholderForTimestampedIdDataLoader
      : ModelForTimestampedIdDataLoader<Stakeholder, Models.Stakeholder>
    {
        public StakeholderForTimestampedIdDataLoader(IQueryBus queryBus)
          : base(StakeholderBase.FromModel, queryBus)
        {
        }
    }
}