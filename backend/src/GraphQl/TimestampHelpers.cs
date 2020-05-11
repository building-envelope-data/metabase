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