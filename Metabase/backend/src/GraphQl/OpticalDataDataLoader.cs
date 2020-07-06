using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
{
    public sealed class OpticalDataDataLoader
      : ModelDataLoader<OpticalData, Models.OpticalData>
    {
        public OpticalDataDataLoader(IQueryBus queryBus)
          : base(OpticalData.FromModel, queryBus)
        {
        }
    }
}