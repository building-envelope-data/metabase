using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

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