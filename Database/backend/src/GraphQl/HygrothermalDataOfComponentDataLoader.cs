using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Database.GraphQl
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