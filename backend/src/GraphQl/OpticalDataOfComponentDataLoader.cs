using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

namespace Icon.GraphQl
{
  public sealed class OpticalDataOfComponentDataLoader
    : ForwardOneToManyAssociatesOfModelDataLoader<OpticalData, Models.Component, Models.ComponentOpticalData, Models.OpticalData>
  {
    public OpticalDataOfComponentDataLoader(IQueryBus queryBus)
      : base(OpticalData.FromModel, queryBus)
    {
    }
  }
}
