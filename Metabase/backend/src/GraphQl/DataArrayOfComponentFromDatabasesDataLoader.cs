using System;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
{
    public abstract class DataArrayOfComponentFromDatabasesDataLoader<TDataModel, TDataGraphQlObject>
      : DataOfComponentFromDatabasesDataLoader<
                    Queries.QueryDataArrayOfComponentsFromDatabases<TDataModel>,
                    TDataModel,
                    TDataGraphQlObject
                >
    {
        public DataArrayOfComponentFromDatabasesDataLoader(
            Func<TDataModel, Timestamp, TDataGraphQlObject> mapModelToGraphQlObject,
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