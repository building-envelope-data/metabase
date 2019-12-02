using System;
using Errors = Icon.Errors;
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
    internal static class ResultHelpers
    {
        internal static T HandleFailure<T>(Result<T, Errors> result)
        {
            if (result.IsFailure) throw new QueryException(result.Error);
            return result.Value;
        }

        internal static IEnumerable<T> HandleFailure<T>(Result<IEnumerable<T>, Errors> result)
        {
            if (result.IsFailure) throw new QueryException(result.Error);
            return result.Value;
        }

        internal static IEnumerable<T> HandleFailures<T>(IEnumerable<Result<T, Errors>> results)
        {
            var result = results.Combine();
            return HandleFailure(result);
        }

        internal static GreenDonut.Result<T> ToDataLoaderResult<T>(Result<T, Errors> result)
        {
            if (result.IsFailure)
            {
                return GreenDonut.Result<T>.Reject(new QueryException(result.Error));
            }
            return GreenDonut.Result<T>.Resolve(result.Value);
        }

        internal static IReadOnlyList<GreenDonut.Result<T>> ToDataLoaderResults<T>(IEnumerable<Result<T, Errors>> results)
        {
            return results.Select(ToDataLoaderResult).ToList().AsReadOnly();
        }
    }
}