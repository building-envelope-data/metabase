using IQueryBus = Icon.Infrastructure.Queries.IQueryBus;

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