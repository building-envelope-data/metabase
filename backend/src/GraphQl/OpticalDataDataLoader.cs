using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using Models = Icon.Models;

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