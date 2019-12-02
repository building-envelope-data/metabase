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
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public abstract class QueryAndMutationBase
    {
        protected ValueObjects.Timestamp HandleTimestamp(
            DateTime? timestamp,
            IResolverContext resolverContext
            )
        {
            var timestampResult = Timestamp.Sanitize(
                timestamp,
                Timestamp.FetchRequest(resolverContext)
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