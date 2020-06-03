using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Resolvers;
using Icon.Infrastructure.Query;
using Microsoft.AspNetCore.Identity;
using ErrorCodes = Icon.ErrorCodes;
using Models = Icon.Models;
using Queries = Icon.Queries;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public static class TimestampHelpers
    {
        public static void Store(
                        ValueObjects.Timestamp timestamp,
                        IQueryRequestBuilder requestBuilder
                        )
        {
            requestBuilder.SetProperty("requestTimestamp", timestamp);
        }

        public static ValueObjects.Timestamp Fetch(IResolverContext context)
        {
            return (ValueObjects.Timestamp)context.ContextData["requestTimestamp"];
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