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
    public sealed class PhotovoltaicDataOfComponentFromDatabasesDataLoader
      : DataArrayOfComponentFromDatabasesDataLoader<Models.PhotovoltaicDataFromDatabase, PhotovoltaicDataFromDatabase>
    {
        public PhotovoltaicDataOfComponentFromDatabasesDataLoader(
            IQueryBus queryBus
            )
          : base(
              PhotovoltaicDataFromDatabase.FromModel,
              queryBus
              )
        {
        }
    }
}