using Icon.Infrastructure.Queries;

namespace Icon.GraphQl
{
    public sealed class WhichDatabasesHaveDataForComponentDataLoader<TDataModel>
      : DataOfComponentFromDatabasesDataLoader<
          Queries.WhichDatabasesHaveDataForComponents<TDataModel>,
          Models.Database,
          Database
        >
    {
        public WhichDatabasesHaveDataForComponentDataLoader(
            IQueryBus queryBus
            )
          : base(
              Queries.WhichDatabasesHaveDataForComponents<TDataModel>.From,
              Database.FromModel,
              queryBus
              )
        {
        }
    }
}