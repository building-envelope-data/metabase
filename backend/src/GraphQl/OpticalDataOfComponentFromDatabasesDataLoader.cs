using Icon.Infrastructure.Queries;

namespace Icon.GraphQl
{
    public sealed class OpticalDataOfComponentFromDatabasesDataLoader
      : DataArrayOfComponentFromDatabasesDataLoader<Models.OpticalDataFromDatabase, OpticalDataFromDatabase>
    {
        public OpticalDataOfComponentFromDatabasesDataLoader(
            IQueryBus queryBus
            )
          : base(
              OpticalDataFromDatabase.FromModel,
              queryBus
              )
        {
        }
    }
}