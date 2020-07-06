using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
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