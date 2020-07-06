using Icon.Infrastructure.Queries;

namespace Icon.GraphQl
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