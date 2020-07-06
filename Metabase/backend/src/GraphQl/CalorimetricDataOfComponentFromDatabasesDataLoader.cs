using Infrastructure.Queries;

namespace Metabase.GraphQl
{
    public sealed class CalorimetricDataOfComponentFromDatabasesDataLoader
      : DataArrayOfComponentFromDatabasesDataLoader<Models.CalorimetricDataFromDatabase, CalorimetricDataFromDatabase>
    {
        public CalorimetricDataOfComponentFromDatabasesDataLoader(
            IQueryBus queryBus
            )
          : base(
              CalorimetricDataFromDatabase.FromModel,
              queryBus
              )
        {
        }
    }
}