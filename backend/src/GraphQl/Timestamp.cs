using System;
using Icon.Infrastructure.Query;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Models = Icon.Models;
using Queries = Icon.Queries;
using System.Linq;
using HotChocolate.Resolvers;
using HotChocolate;
using CSharpFunctionalExtensions;
using ErrorCodes = Icon.ErrorCodes;
using HotChocolate.Execution;

namespace Icon.GraphQl
{
    public sealed class Timestamp
    {
        public static Result<ValueObjects.Timestamp, IError> Sanitize(
            DateTime? maybeTimestamp,
            DateTime requestTimestamp
            )
        {
            var timestamp = maybeTimestamp ?? requestTimestamp;
            return ValueObjects.Timestamp.From(
                timestamp,
                now: requestTimestamp,
                path: Array.Empty<object>() // TODO What is the proper path for variables?
                );
        }

        public static void StoreRequest(DateTime timestamp, IQueryRequestBuilder requestBuilder)
        {
            requestBuilder.SetProperty("requestTimestamp", timestamp);
        }

        public static DateTime FetchRequest(IResolverContext context)
        {
            return (System.DateTime)context.ContextData["requestTimestamp"];
        }

        public static void Store(DateTime timestamp, IResolverContext context)
        {
            // TODO Is there a better way to pass data down the tree to resolvers? Something with proper types? See https://hotchocolate.io/docs/custom-context
            context.ScopedContextData = context.ScopedContextData.SetItem(
                "timestamp",
                timestamp
                );
        }

        public static DateTime Fetch(IResolverContext context)
        {
            return (System.DateTime)context.ScopedContextData["timestamp"];
        }
    }
}