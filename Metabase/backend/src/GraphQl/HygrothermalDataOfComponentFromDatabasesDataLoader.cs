using Infrastructure.Queries;

namespace Metabase.GraphQl
{
    public sealed class HygrothermalDataOfComponentFromDatabasesDataLoader
      : DataArrayOfComponentFromDatabasesDataLoader<Models.HygrothermalData, HygrothermalData>
    {
        public HygrothermalDataOfComponentFromDatabasesDataLoader(
            IQueryBus queryBus
            )
          : base(
              HygrothermalData.FromModel,
              queryBus
              )
        {
        }
    }
}