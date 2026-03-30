using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using GraphQL;
using HotChocolate;
using HotChocolate.Resolvers;
using Metabase.Json;
using Microsoft.Extensions.Logging;

namespace Metabase.GraphQl.Requests;

public static partial class Log
{
    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Failed with status code {StatusCode} to request {Locator} for {Request}.")]
    public static partial void FailedWithStatusCode(
        this ILogger<GraphQlRequestHelper> logger,
        Exception exception,
        HttpStatusCode? StatusCode,
        Uri Locator,
        string Request
    );

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Failed to deserialize GraphQL response of request to {Locator} for {Request}. The details given are: Zero-based number of bytes read within the current line before the exception are {BytePositionInLine}, zero-based number of lines read before the exception are {LineNumber}, message that describes the current exception is '{Message}', path within the JSON where the exception was encountered is {Path}.")]
    public static partial void FailedToDeserialize(
        this ILogger<GraphQlRequestHelper> logger,
        Exception exception,
        Uri Locator,
        string Request,
        long? BytePositionInLine,
        long? LineNumber,
        string Message,
        string? Path
    );

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Failed to request {Locator} for {Request} or failed to deserialize the response.")]
    public static partial void FailedToRequestOrDeserialize(
        this ILogger<GraphQlRequestHelper> logger,
        Exception exception,
        Uri Locator,
        string Request
    );
}

public sealed class GraphQlRequestHelper(
    ILogger<GraphQlRequestHelper> logger
)
{
    public async Task<T?> TransformExceptionsAsync<T>(
        Func<Task<T>> action,
        Uri databaseLocator,
        GraphQLRequest request,
        IResolverContext resolverContext
    )
    where T : class
    {
        try
        {
            return await action();
        }
        catch (HttpRequestException exception)
        {
            logger.FailedWithStatusCode(
                exception,
                exception.StatusCode,
                databaseLocator,
                JsonSerializer.Serialize(request, JsonSerializerSettings.GraphQl)
            );
            resolverContext.ReportError(
                ErrorBuilder.New()
                    .SetCode("EXTERNAL_GRAPHQL_REQUEST_FAILED")
                    .SetPath(resolverContext.Path)
                    .SetMessage($"Failed with status code '{exception.StatusCode}' to request the endpoint '{databaseLocator}' for {JsonSerializer.Serialize(request, JsonSerializerSettings.GraphQl)}.")
                    .SetException(exception)
                    .Build()
            );
            return null;
        }
        catch (JsonException exception)
        {
            logger.FailedToDeserialize(
                exception,
                databaseLocator,
                JsonSerializer.Serialize(request, JsonSerializerSettings.GraphQl),
                exception.BytePositionInLine,
                exception.LineNumber,
                exception.Message,
                exception.Path
            );
            resolverContext.ReportError(
                ErrorBuilder.New()
                    .SetCode("JSON_DESERIALIZATION_FAILED")
                    .SetPath(resolverContext.Path) // TODO Add the error path. I would do it as follows as a workaround, however splitting the path at '.' is wrong in general: .SetPath(resolverContext.Path.ToList().Concat(e.Path?.Split('.') ?? []).ToList())
                    .SetMessage($"Failed to deserialize the GraphQL response of the request to the endpoint '{databaseLocator}' for {JsonSerializer.Serialize(request, JsonSerializerSettings.GraphQl)}. The details given are: Zero-based number of bytes read within the current line before the exception are '{exception.BytePositionInLine}', zero-based number of lines read before the exception are '{exception.LineNumber}', message that describes the current exception is \"{exception.Message}\", path within the JSON where the exception was encountered is '{exception.Path}'.")
                    .SetException(exception)
                    .Build()
            );
            return null;
        }
        catch (Exception exception)
        {
            logger.FailedToRequestOrDeserialize(
                exception,
                databaseLocator,
                JsonSerializer.Serialize(request, JsonSerializerSettings.GraphQl)
            );
            resolverContext.ReportError(
                ErrorBuilder.New()
                    .SetCode("DATABASE_REQUEST_FAILED")
                    .SetPath(resolverContext.Path)
                    .SetMessage($"Failed to request {databaseLocator} for {JsonSerializer.Serialize(request, JsonSerializerSettings.GraphQl)} or failed to deserialize the response.")
                    .SetException(exception)
                    .Build()
            );
            return null;
        }
    }
}