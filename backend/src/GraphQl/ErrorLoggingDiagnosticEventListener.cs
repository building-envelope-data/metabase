using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Execution.Instrumentation;
using HotChocolate.Execution.Processing;
using HotChocolate.Resolvers;
using Microsoft.Extensions.Logging;

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
        Message = "Resolver error. Field: {FieldName}"
    )]
    public static partial void ResolverError(
        this ILogger<ErrorLoggingDiagnosticEventListener> logger,
        Exception? exception,
        string fieldName,
        [LogProperties] IOperation operation,
        [LogProperties] IError error
    );

    [LoggerMessage(
        Level = LogLevel.Error
    )]
    public static partial void SubscriptionEventError(
        this ILogger<ErrorLoggingDiagnosticEventListener> logger,
        Exception exception,
        [LogProperties] IOperation operation
    );

    [LoggerMessage(
        Level = LogLevel.Error
    )]
    public static partial void SubscriptionTransportError(
        this ILogger<ErrorLoggingDiagnosticEventListener> logger,
        Exception exception,
        [LogProperties] IOperation operation
    );

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Syntax error. Document: {Document}"
    )]
    public static partial void SyntaxError(
        this ILogger<ErrorLoggingDiagnosticEventListener> logger,
        Exception? exception,
        IOperationDocument? document,
        [LogProperties] IError error
    );

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Task error. Kind: {Kind}. Status: {Status}."
    )]
    public static partial void TaskError(
        this ILogger<ErrorLoggingDiagnosticEventListener> logger,
        Exception? exception,
        ExecutionTaskKind kind,
        ExecutionTaskStatus status,
        [LogProperties] IError error
    );

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Validation error. Document: {Document}")]
    public static partial void ValidationError(
        this ILogger<ErrorLoggingDiagnosticEventListener> logger,
        Exception? exception,
        IOperationDocument? document,
        [LogProperties] IError error
    );

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Execution returned an error: {Document} with {Variables}")]
    public static partial void OperationError(
        this ILogger<ErrorLoggingDiagnosticEventListener> logger,
        IOperationDocument? document,
        string? variables,
        [LogProperties] IError error
    );

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Unexpected execution error while executing: {Document}\nVariables: {Variables}")]
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
    public override IDisposable ExecuteRequest(IRequestContext context)
    {
        // ... we will return an activity scope that is used to signal when the request is finished.
        return new RequestScope(logger, context);
    }

    public override void RequestError(
        IRequestContext context,
        Exception exception
    )
    {
        logger.RequestError(exception, context.Request.Document);
        base.RequestError(context, exception);
    }

    public override void ResolverError(
        IMiddlewareContext context,
        IError error
    )
    {
        logger.ResolverError(error.Exception, context.Selection.Field.Name, context.Operation, error);
        base.ResolverError(context, error);
    }

    public override void SubscriptionEventError(
        SubscriptionEventContext context,
        Exception exception
    )
    {
        logger.SubscriptionEventError(exception, context.Subscription.Operation);
        base.SubscriptionEventError(context, exception);
    }

    public override void SubscriptionTransportError(
        ISubscription subscription,
        Exception exception
    )
    {
        logger.SubscriptionTransportError(exception, subscription.Operation);
        base.SubscriptionTransportError(subscription, exception);
    }

    public override void SyntaxError(
        IRequestContext context,
        IError error
    )
    {
        logger.SyntaxError(error.Exception, context.Request.Document, error);
        base.SyntaxError(context, error);
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
        IRequestContext context,
        IReadOnlyList<IError> errors
    )
    {
        foreach (var error in errors)
        {
            logger.ValidationError(error.Exception, context.Request.Document, error);
        }
        base.ValidationErrors(context, errors);
    }

    private sealed partial class RequestScope(ILogger<ErrorLoggingDiagnosticEventListener> logger, IRequestContext context) : IDisposable
    {
        [GeneratedRegex(@"apiKey|authKey|privateKey|password|passphrase|secret|secure|security|token", RegexOptions.IgnoreCase, "")]
        private static partial Regex SecretRegex();

        private string? _variables;

        private string? StringifyVariables()
        {
            if (_variables is not null)
            {
                return _variables;
            }
            if (context.Variables is null)
            {
                return null;
            }
            StringBuilder stringBuilder = new();
            foreach (var variableValueCollection in context.Variables)
            {
                foreach (var variableValue in variableValueCollection)
                {
                    try
                    {
                        stringBuilder.AppendFormat(
                            CultureInfo.InvariantCulture,
                            $"{variableValue.Name} : {variableValue.Type} = "
                        );
                        stringBuilder.Append('\'');
                        stringBuilder.Append(
                            SecretRegex().IsMatch(variableValue.Name)
                            ? "<redacted>"
                            : variableValue.Value.ToString()
                        );
                        stringBuilder.Append('\'');
                        stringBuilder.AppendFormat(CultureInfo.InvariantCulture, $"{Environment.NewLine}");
                    }
                    catch (Exception exception)
                    {
                        // all input type records will land here.
                        stringBuilder.AppendFormat(CultureInfo.InvariantCulture, $"Failed stringifying the value: {exception.Message}");
                        stringBuilder.AppendFormat(CultureInfo.InvariantCulture, $"{Environment.NewLine}");
                    }
                }
            }
            _variables = stringBuilder.ToString();
            return _variables;
        }

        public void Dispose()
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                if (context.Document is not null)
                {
                    logger.Executed(
                        context.Document.ToString(true),
                        StringifyVariables()
                    );
                }
            }
            // when the request is finished it will dispose the activity scope
            if (context.Result is IOperationResult { Errors.Count: > 0 } operationResult)
            {
                foreach (var error in operationResult.Errors)
                {
                    logger.OperationError(context.Request.Document, StringifyVariables(), error);
                }
            }
            if (context.Exception is { })
            {
                logger.UnexpectedExecutionException(context.Request.Document, StringifyVariables(), context.Exception);
            }
        }
    }
}