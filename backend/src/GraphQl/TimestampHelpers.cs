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
using ValueObjects = Icon.ValueObjects;
using HotChocolate.Execution;

namespace Icon.GraphQl
{
    public static class TimestampHelpers
    {
        public static Result<ValueObjects.Timestamp, Errors> Sanitize(
            DateTime? maybeTimestamp,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            var timestamp = maybeTimestamp ?? requestTimestamp.Value;
            return
              ValueObjects.Timestamp.From(
                timestamp,
                now: requestTimestamp.Value,
                path: Array.Empty<object>() // TODO What is the proper path for variables?
                );
        }

        public static void StoreRequest(ValueObjects.Timestamp timestamp, IQueryRequestBuilder requestBuilder)
        {
            requestBuilder.SetProperty("requestTimestamp", timestamp);
        }

        public static ValueObjects.Timestamp FetchRequest(IResolverContext context)
        {
            return (ValueObjects.Timestamp)context.ContextData["requestTimestamp"];
        }

        public static void Store(ValueObjects.Timestamp timestamp, IResolverContext context)
        {
            // TODO Is there a better way to pass data down the tree to resolvers? Something with proper types? See https://hotchocolate.io/docs/custom-context
            context.ScopedContextData = context.ScopedContextData.SetItem(
                "timestamp",
                timestamp
                );
        }

        public static ValueObjects.Timestamp Fetch(IResolverContext context)
        {
            return (ValueObjects.Timestamp)context.ScopedContextData["timestamp"];
        }

        public static ValueObjects.TimestampedId TimestampId(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp
            )
        {
            return ResultHelpers.HandleFailure(
                ValueObjects.TimestampedId.From(
                  id, timestamp
                  )
                );
        }
    }
}