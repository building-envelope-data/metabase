using Infrastructure.Queries;

namespace Metabase.GraphQl
{
    public sealed class CalorimetricDataOfComponentFromDatabasesDataLoader
      : DataArrayOfComponentFromDatabasesDataLoader<Models.CalorimetricData, CalorimetricData>
    {
        public CalorimetricDataOfComponentFromDatabasesDataLoader(
            IQueryBus queryBus
            )
          : base(
              CalorimetricData.FromModel,
              queryBus
              )
        {
        }
    }
}