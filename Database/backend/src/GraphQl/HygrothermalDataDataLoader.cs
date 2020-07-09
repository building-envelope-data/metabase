using Infrastructure.GraphQl;
using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Database.GraphQl
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