using Infrastructure.GraphQl;
using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Database.GraphQl
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