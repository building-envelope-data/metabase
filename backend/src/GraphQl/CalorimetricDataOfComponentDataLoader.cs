using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

namespace Icon.GraphQl
{
    public sealed class CalorimetricDataOfComponentDataLoader
      : ForwardOneToManyAssociatesOfModelDataLoader<CalorimetricData, Models.Component, Models.ComponentCalorimetricData, Models.CalorimetricData>
    {
        public CalorimetricDataOfComponentDataLoader(IQueryBus queryBus)
          : base(CalorimetricData.FromModel, queryBus)
        {
        }
    }
}