using IQueryBus = Icon.Infrastructure.Queries.IQueryBus;

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