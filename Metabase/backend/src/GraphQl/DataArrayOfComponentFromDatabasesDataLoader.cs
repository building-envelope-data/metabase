using System;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
{
    public abstract class DataArrayOfComponentFromDatabasesDataLoader<TDataModel, TDataGraphQlObject>
      : DataOfComponentFromDatabasesDataLoader<
                    Queries.QueryDataArrayOfComponentsFromDatabasesQuery<TDataModel>,
                    TDataModel,
                    TDataGraphQlObject
                >
    {
        protected DataArrayOfComponentFromDatabasesDataLoader(
            Func<TDataModel, Timestamp, TDataGraphQlObject> mapModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(
              timestampedIds => Queries.QueryDataArrayOfComponentsFromDatabasesQuery<TDataModel>.From(timestampedIds),
              mapModelToGraphQlObject,
              queryBus
              )
        {
        }
    }
}