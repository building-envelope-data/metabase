using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

namespace Icon.GraphQl
{
    public sealed class OpticalDataOfComponentDataLoader
      : DataOfComponentDataLoader<Models.OpticalData, OpticalData>
    {
        public OpticalDataOfComponentDataLoader(IQueryBus queryBus)
          : base(OpticalData.FromModel, queryBus)
        {
        }
    }
}