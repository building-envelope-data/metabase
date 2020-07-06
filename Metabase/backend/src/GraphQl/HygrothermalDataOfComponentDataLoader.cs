using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
{
    public sealed class HygrothermalDataOfComponentDataLoader
      : DataOfComponentDataLoader<Models.HygrothermalData, HygrothermalData>
    {
        public HygrothermalDataOfComponentDataLoader(IQueryBus queryBus)
          : base(HygrothermalData.FromModel, queryBus)
        {
        }
    }
}