using Infrastructure.Queries;

namespace Metabase.GraphQl
{
    public sealed class OpticalDataOfComponentFromDatabasesDataLoader
      : DataArrayOfComponentFromDatabasesDataLoader<Models.OpticalData, OpticalData>
    {
        public OpticalDataOfComponentFromDatabasesDataLoader(
            IQueryBus queryBus
            )
          : base(
              OpticalData.FromModel,
              queryBus
              )
        {
        }
    }
}