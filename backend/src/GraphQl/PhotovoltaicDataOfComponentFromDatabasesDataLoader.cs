using Icon.Infrastructure.Queries;

namespace Icon.GraphQl
{
    public sealed class PhotovoltaicDataOfComponentFromDatabasesDataLoader
      : DataArrayOfComponentFromDatabasesDataLoader<Models.PhotovoltaicDataFromDatabase, PhotovoltaicDataFromDatabase>
    {
        public PhotovoltaicDataOfComponentFromDatabasesDataLoader(
            IQueryBus queryBus
            )
          : base(
              PhotovoltaicDataFromDatabase.FromModel,
              queryBus
              )
        {
        }
    }
}