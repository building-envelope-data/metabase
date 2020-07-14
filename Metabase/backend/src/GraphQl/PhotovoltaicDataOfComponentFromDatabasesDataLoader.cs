using Infrastructure.Queries;

namespace Metabase.GraphQl
{
    public sealed class PhotovoltaicDataOfComponentFromDatabasesDataLoader
      : DataArrayOfComponentFromDatabasesDataLoader<Models.PhotovoltaicData, PhotovoltaicData>
    {
        public PhotovoltaicDataOfComponentFromDatabasesDataLoader(
            IQueryBus queryBus
            )
          : base(
              PhotovoltaicData.FromModel,
              queryBus
              )
        {
        }
    }
}