using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Database.GraphQl
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