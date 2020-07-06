using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
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