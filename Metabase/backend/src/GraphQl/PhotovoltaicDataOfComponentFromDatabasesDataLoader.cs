using Infrastructure.Queries;

namespace Metabase.GraphQl
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