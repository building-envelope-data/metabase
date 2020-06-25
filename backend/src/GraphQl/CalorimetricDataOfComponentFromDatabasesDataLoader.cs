using Icon.Infrastructure.Query;

namespace Icon.GraphQl
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