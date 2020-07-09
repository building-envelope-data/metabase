using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Database.GraphQl
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