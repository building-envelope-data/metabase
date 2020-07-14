using Infrastructure.GraphQl;
using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Database.GraphQl
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