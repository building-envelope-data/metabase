using System;
using Icon.Infrastructure.Query;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Models = Icon.Models;
using Queries = Icon.Queries;
using System.Linq;
using HotChocolate.Resolvers;

namespace Icon.GraphQl
{
    public abstract class QueryAndMutationBase
    {
        protected DateTime SetTimestamp(DateTime timestamp, IResolverContext context)
        {
          // TODO Is there a better way to pass data down the tree to resolvers? Something with proper types? See https://hotchocolate.io/docs/custom-context
            context.ScopedContextData = context.ScopedContextData.SetItem(
                "timestamp",
                timestamp
                );
            return timestamp;
        }
    }
}
