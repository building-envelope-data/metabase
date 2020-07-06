using IQueryBus = Icon.Infrastructure.Queries.IQueryBus;

namespace Icon.GraphQl
{
    public sealed class CalorimetricDataDataLoader
      : ModelDataLoader<CalorimetricData, Models.CalorimetricData>
    {
        public CalorimetricDataDataLoader(IQueryBus queryBus)
          : base(CalorimetricData.FromModel, queryBus)
        {
        }
    }
}