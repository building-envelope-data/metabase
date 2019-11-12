using System;
using Icon.Infrastructure.Query;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Models = Icon.Models;
using Queries = Icon.Queries;
using System.Linq;
using HotChocolate.Resolvers;
using QueryException = HotChocolate.Execution.QueryException;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public abstract class QueryAndMutationBase
    {
        protected DateTime HandleTimestamp(DateTime? timestamp, IResolverContext context)
        {
            var timestampResult = Timestamp.Sanitize(timestamp);
            if (timestampResult.IsFailure) {
              throw new QueryException(timestampResult.Error);
            }
            Timestamp.Store(timestampResult.Value, context);
            return timestampResult.Value;
        }
    }
}