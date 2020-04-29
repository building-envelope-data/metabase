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
            var timestampResult = TimestampHelpers.Sanitize(
                timestamp,
                TimestampHelpers.FetchRequest(resolverContext)
                );
            if (timestampResult.IsFailure)
            {
                throw new QueryException(timestampResult.Error);
            }
            TimestampHelpers.Store(timestampResult.Value, resolverContext);
            return timestampResult.Value;
        }

        // TODO This method timestamps the ID and stores the timestamp whereas
        // the other method only timestamps the ID. The first is what we want
        // in queries, the latter is used in mutations. Make this distinction
        // obvious from the naming.
        protected ValueObjects.TimestampedId TimestampId(
            ValueObjects.Id id,
            DateTime? timestamp,
            IResolverContext resolverContext
            )
        {
            var requestTimestamp = HandleTimestamp(timestamp, resolverContext);
            return ResultHelpers.HandleFailure(
                ValueObjects.TimestampedId.From(
                  id, requestTimestamp
                  )
                );
        }
    }
}