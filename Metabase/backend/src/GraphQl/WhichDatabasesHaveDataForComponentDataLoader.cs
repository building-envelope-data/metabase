using Infrastructure.Queries;

namespace Metabase.GraphQl
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