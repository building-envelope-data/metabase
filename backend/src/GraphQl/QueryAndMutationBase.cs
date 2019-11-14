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
                timestamp ?? DateTime.UtcNow // TODO Set timestamp in IQueryContext when the request arrives and use it here.
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