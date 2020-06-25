using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

namespace Icon.GraphQl
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