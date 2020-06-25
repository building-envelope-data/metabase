using System;
using Icon.Infrastructure.Query;

namespace Icon.GraphQl
{
    public abstract class DataArrayOfComponentFromDatabasesDataLoader<TDataModel, TDataGraphQlObject>
      : DataOfComponentFromDatabasesDataLoader<
                    Queries.QueryDataArrayOfComponentsFromDatabases<TDataModel>,
                    TDataModel,
                    TDataGraphQlObject
                >
    {
        public DataArrayOfComponentFromDatabasesDataLoader(
            Func<TDataModel, ValueObjects.Timestamp, TDataGraphQlObject> mapModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(
              timestampedIds => Queries.QueryDataArrayOfComponentsFromDatabases<TDataModel>.From(timestampedIds),
              mapModelToGraphQlObject,
              queryBus
              )
        {
        }
    }
}