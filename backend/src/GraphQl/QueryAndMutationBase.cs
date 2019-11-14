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
using IHttpContextAccessor = Microsoft.AspNetCore.Http.IHttpContextAccessor;

namespace Icon.GraphQl
{
    public abstract class QueryAndMutationBase
    {
        protected DateTime HandleTimestamp(
            DateTime? timestamp,
            IResolverContext resolverContext
            )
        {
            var timestampResult = Timestamp.Sanitize(
                timestamp ?? DateTime.UtcNow // TODO Set timestamp in IQueryContext when the request arrives and use it here instead of `UtcNow`. It should be possible to use a request interceptor for this as told in https://github.com/ChilliCream/hotchocolate/issues/1054#issuecomment-527206145 Maybe it would also be possible to use a middleware as in https://hotchocolate.io/docs/custom-context
                );
            if (timestampResult.IsFailure)
            {
                throw new QueryException(timestampResult.Error);
            }
            Timestamp.Store(timestampResult.Value, resolverContext);
            return timestampResult.Value;
        }
    }
}