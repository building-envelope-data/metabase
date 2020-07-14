using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Errors = Infrastructure.Errors;
using QueryException = HotChocolate.Execution.QueryException;

namespace Infrastructure.GraphQl
{
    public static class ResultHelpers
    {
        public static T HandleFailure<T>(Result<T, Errors> result)
        {
            if (result.IsFailure) throw new QueryException(result.Error);
            return result.Value;
        }

        public static IEnumerable<T> HandleFailure<T>(Result<IEnumerable<T>, Errors> result)
        {
            if (result.IsFailure) throw new QueryException(result.Error);
            return result.Value;
        }

        public static IEnumerable<T> HandleFailures<T>(IEnumerable<Result<T, Errors>> results)
        {
            var result = results.Combine();
            return HandleFailure(result);
        }

        public static GreenDonut.Result<T> ToDataLoaderResult<T>(Result<T, Errors> result)
        {
            if (result.IsFailure)
            {
                return GreenDonut.Result<T>.Reject(new QueryException(result.Error));
            }
            return GreenDonut.Result<T>.Resolve(result.Value);
        }

        public static IReadOnlyList<GreenDonut.Result<T>> ToDataLoaderResults<T>(IEnumerable<Result<T, Errors>> results)
        {
            return results.Select(ToDataLoaderResult).ToList().AsReadOnly();
        }

        public static IReadOnlyList<GreenDonut.Result<IReadOnlyList<T>>> ToDataLoaderResultsX<T>(IEnumerable<Result<IEnumerable<Result<T, Errors>>, Errors>> results)
        {
            return results.Select(result =>
                {
                    if (result.IsFailure)
                    {
                        return GreenDonut.Result<IReadOnlyList<T>>.Reject(new QueryException(result.Error));
                    }
                    // TODO If one of the results in `result.Value` is a failure, then
                    // `combinedResult` is a failure even if there are many successful
                    // results. Should we just discard failures and return all successful
                    // results instead? Or is it possible to return both, failures and
                    // successful results? The latter would be terrific! See
                    // https://github.com/vkhorikov/CSharpFunctionalExtensions/blob/master/CSharpFunctionalExtensions/Result/Extensions/Combine.cs
                    var combinedResult = result.Value.Combine();
                    if (combinedResult.IsFailure)
                    {
                        return GreenDonut.Result<IReadOnlyList<T>>.Reject(new QueryException(combinedResult.Error));
                    }
                    return GreenDonut.Result<IReadOnlyList<T>>.Resolve(combinedResult.Value.ToList().AsReadOnly());
                }
              ).ToList().AsReadOnly();
        }
    }
}