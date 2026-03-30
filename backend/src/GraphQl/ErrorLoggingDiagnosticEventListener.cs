using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Metabase.Logging;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Execution.Instrumentation;
using HotChocolate.Resolvers;
using Microsoft.Extensions.Logging;
using HotChocolate.Language;

namespace Metabase.GraphQl;

public static partial class Log
{
    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Request error. Document: {Document}"
    )]
    public static partial void RequestError(
        this ILogger<ErrorLoggingDiagnosticEventListener> logger,
        Exception exception,
        IOperationDocument? document
    );

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Request error. Document: {Document}"
    )]
    public static partial void RequestError(
        this ILogger<ErrorLoggingDiagnosticEventListener> logger,
        IOperationDocument? document,
        [TagProvider(typeof(HotChocolateIErrorTagProvider), nameof(HotChocolateIErrorTagProvider.RecordTags))] IError error
    );

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Resolver error. Field: '{FieldName}'. Document: {Document}"
    )]
    public static partial void ResolverError(
        this ILogger<ErrorLoggingDiagnosticEventListener> logger,
        string fieldName,
        DocumentNode document,
        [TagProvider(typeof(HotChocolateIErrorTagProvider), nameof(HotChocolateIErrorTagProvider.RecordTags))] IError error,
        Exception? exception
    );

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Resolver error. Field: '{FieldName}'. Document: {Document}"
    )]
    public static partial void ResolverError(
        this ILogger<ErrorLoggingDiagnosticEventListener> logger,
        Exception? exception,
        string fieldName,
        IOperationDocument? document,
        [TagProvider(typeof(HotChocolateIErrorTagProvider), nameof(HotChocolateIErrorTagProvider.RecordTags))] IError error
    );

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Subscription event error. Id: '{SubscriptionId}'. Operation: {Document}"
    )]
    public static partial void SubscriptionEventError(
        this ILogger<ErrorLoggingDiagnosticEventListener> logger,
        Exception exception,
        ulong subscriptionId,
        IOperationDocument? document
    );

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Task error. Kind: '{Kind}'. Status: '{Status}'."
    )]
    public static partial void TaskError(
        this ILogger<ErrorLoggingDiagnosticEventListener> logger,
        Exception? exception,
        ExecutionTaskKind kind,
        ExecutionTaskStatus status,
        [TagProvider(typeof(HotChocolateIErrorTagProvider), nameof(HotChocolateIErrorTagProvider.RecordTags))] IError error
    );

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Validation error. Document: {Document}")]
    public static partial void ValidationError(
        this ILogger<ErrorLoggingDiagnosticEventListener> logger,
        Exception? exception,
        IOperationDocument? document,
        [TagProvider(typeof(HotChocolateIErrorTagProvider), nameof(HotChocolateIErrorTagProvider.RecordTags))] IError error
    );

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Operation error. Document: {Document}\nVariables: {Variables}")]
    public static partial void OperationError(
        this ILogger<ErrorLoggingDiagnosticEventListener> logger,
        IOperationDocument? document,
        string? variables,
        [TagProvider(typeof(HotChocolateIErrorTagProvider), nameof(HotChocolateIErrorTagProvider.RecordTags))] IError error
    );

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Unexpected execution error. Document: {Document}\nVariables: {Variables}")]
    public static partial void UnexpectedExecutionException(
        this ILogger<ErrorLoggingDiagnosticEventListener> logger,
        IOperationDocument? document,
        string? variables,
        Exception exception
    );

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Executed:\n{Document}\nVariables: {Variables}")]
    public static partial void Executed(
        this ILogger<ErrorLoggingDiagnosticEventListener> logger,
        string document,
        string? variables
    );
}

// Inspired by https://chillicream.com/blog/2019/03/19/logging-with-hotchocolate
// and https://chillicream.com/blog/2021/01/10/hot-chocolate-logging
public sealed partial class ErrorLoggingDiagnosticEventListener(
    ILogger<ErrorLoggingDiagnosticEventListener> logger
)
: ExecutionDiagnosticEventListener
{
    // this diagnostic event is raised when a request is executed ...
    public override IDisposable ExecuteRequest(RequestContext context)
    {
        // ... we will return an activity scope that is used to signal when the request is finished.
        return new RequestScope(logger, context);
    }

    public override void RequestError(
        RequestContext context,
        Exception error
    )
    {
        logger.RequestError(error, context.Request.Document);
        base.RequestError(context, error);
    }

    public override void RequestError(
        RequestContext context,
        IError error
    )
    {
        logger.RequestError(context.Request.Document, error);
        base.RequestError(context, error);
    }

    public override void ResolverError(
        IMiddlewareContext context,
        IError error
    )
    {
        logger.ResolverError(context.Selection.Field.Name, context.Operation.Document, error, error.Exception);
        base.ResolverError(context, error);
    }

    public override void ResolverError(
        RequestContext context,
        ISelection selection,
        IError error
    )
    {
        logger.ResolverError(error.Exception, selection.Field.Name, context.Request?.Document, error);
        base.ResolverError(context, selection, error);
    }

    public override void SubscriptionEventError(
        RequestContext context,
        ulong subscriptionId,
        Exception exception
    )
    {
        logger.SubscriptionEventError(exception, subscriptionId, context.Request.Document);
        base.SubscriptionEventError(context, subscriptionId, exception);
    }

    public override void TaskError(
        IExecutionTask task,
        IError error
    )
    {
        logger.TaskError(error.Exception, task.Kind, task.Status, error);
        base.TaskError(task, error);
    }

    public override void ValidationErrors(
        RequestContext context,
        IReadOnlyList<IError> errors
    )
    {
        foreach (var error in errors)
        {
            logger.ValidationError(error.Exception, context.Request.Document, error);
        }
        base.ValidationErrors(context, errors);
    }

    private sealed partial class RequestScope(ILogger<ErrorLoggingDiagnosticEventListener> logger, RequestContext context) : IDisposable
    {
        [GeneratedRegex(@"apiKey|authKey|privateKey|password|passphrase|secret|secure|security|token", RegexOptions.IgnoreCase, "")]
        private static partial Regex SecretRegex();

        private string? _variables;

        private string? StringifyVariables()
        {
            // TODO Where are the variables now if not anymore in context.Variables?
            // if (_variables is not null)
            // {
            //     return _variables;
            // }
            // if (context.Variables is null)
            // {
            //     return null;
            // }
            // StringBuilder stringBuilder = new();
            // foreach (var variableValueCollection in context.Variables)
            // {
            //     foreach (var variableValue in variableValueCollection)
            //     {
            //         try
            //         {
            //             stringBuilder.AppendFormat(
            //                 CultureInfo.InvariantCulture,
            //                 $"{variableValue.Name} : {variableValue.Type} = "
            //             );
            //             stringBuilder.Append('\'');
            //             stringBuilder.Append(
            //                 SecretRegex().IsMatch(variableValue.Name)
            //                 ? "<redacted>"
            //                 : variableValue.Value.ToString()
            //             );
            //             stringBuilder.Append('\'');
            //             stringBuilder.AppendFormat(CultureInfo.InvariantCulture, $"{Environment.NewLine}");
            //         }
            //         catch (Exception exception)
            //         {
            //             // all input type records will land here.
            //             stringBuilder.AppendFormat(CultureInfo.InvariantCulture, $"Failed stringifying the value: {exception.Message}");
            //             stringBuilder.AppendFormat(CultureInfo.InvariantCulture, $"{Environment.NewLine}");
            //         }
            //     }
            // }
            // _variables = stringBuilder.ToString();
            _variables = null;
            return _variables;
        }

        public void Dispose()
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                if (context.OperationDocumentInfo.Document is not null)
                {
#pragma warning disable CA1873 // Evaluation of this argument may be expensive and unnecessary if logging is disabled (https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca1873)
                    logger.Executed(
                        context.OperationDocumentInfo.Document.ToString(true),
                        StringifyVariables()
                    );
#pragma warning restore CA1873
                }
            }
            // when the request is finished it will dispose the activity scope
            if (context.Result is OperationResult { Errors.Count: > 0 } operationResult)
            {
                foreach (var error in operationResult.Errors)
                {
                    logger.OperationError(context.Request.Document, StringifyVariables(), error);
                }
            }
            // TODO Where is the exception now?
            // if (context.Exception is { })
            // {
            //     logger.UnexpectedExecutionException(context.Request.Document, StringifyVariables(), context.Exception);
            // }
        }
    }
}