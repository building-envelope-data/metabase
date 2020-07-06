using IQueryBus = Icon.Infrastructure.Queries.IQueryBus;

namespace Icon.GraphQl
{
    public sealed class CalorimetricDataOfComponentDataLoader
      : DataOfComponentDataLoader<Models.CalorimetricData, CalorimetricData>
    {
        public CalorimetricDataOfComponentDataLoader(IQueryBus queryBus)
          : base(CalorimetricData.FromModel, queryBus)
        {
        }
    }
}