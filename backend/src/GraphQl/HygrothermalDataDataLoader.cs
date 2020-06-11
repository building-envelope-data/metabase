using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using Models = Icon.Models;

namespace Icon.GraphQl
{
    public sealed class HygrothermalDataDataLoader
      : ModelDataLoader<HygrothermalData, Models.HygrothermalData>
    {
        public HygrothermalDataDataLoader(IQueryBus queryBus)
          : base(HygrothermalData.FromModel, queryBus)
        {
        }
    }
}