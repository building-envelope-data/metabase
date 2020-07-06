using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
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