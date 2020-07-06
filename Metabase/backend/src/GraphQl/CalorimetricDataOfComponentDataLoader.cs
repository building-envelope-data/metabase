using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
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