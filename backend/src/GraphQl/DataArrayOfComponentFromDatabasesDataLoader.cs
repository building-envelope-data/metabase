using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Icon.Infrastructure.Query;
using CancellationToken = System.Threading.CancellationToken;
using IError = HotChocolate.IError;

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