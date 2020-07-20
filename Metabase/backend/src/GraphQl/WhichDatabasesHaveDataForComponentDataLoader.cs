using Infrastructure.Queries;

namespace Metabase.GraphQl
{
    public sealed class WhichDatabasesHaveDataForComponentDataLoader<TDataModel>
      : DataOfComponentFromDatabasesDataLoader<
          Queries.WhichDatabasesHaveDataForComponentsQuery<TDataModel>,
          Models.Database,
          Database
        >
    {
        public WhichDatabasesHaveDataForComponentDataLoader(
            IQueryBus queryBus
            )
          : base(
              Queries.WhichDatabasesHaveDataForComponentsQuery<TDataModel>.From,
              Database.FromModel,
              queryBus
              )
        {
        }
    }
}