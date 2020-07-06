using Infrastructure.Queries;

namespace Metabase.GraphQl
{
    public sealed class HygrothermalDataOfComponentFromDatabasesDataLoader
      : DataArrayOfComponentFromDatabasesDataLoader<Models.HygrothermalDataFromDatabase, HygrothermalDataFromDatabase>
    {
        public HygrothermalDataOfComponentFromDatabasesDataLoader(
            IQueryBus queryBus
            )
          : base(
              HygrothermalDataFromDatabase.FromModel,
              queryBus
              )
        {
        }
    }
}