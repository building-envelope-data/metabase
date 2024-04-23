using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Execution.Instrumentation;
using HotChocolate.Execution.Processing;
using HotChocolate.Resolvers;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Metabase.GraphQl;

public static partial class Log
{
    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Error,
        Message = "Failed executing the query {Query}.")]
    public static partial void FailedQueryExecution(
        this ILogger logger,
        Exception exception,
        IQuery? query
    );

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "Failed executing the operation {Operation} with the error {Error}.")]
    public static partial void FailedOperationExecution(
        this ILogger logger,
        Exception? exception,
        IOperation operation,
        string error
    );

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Error,
        Message = "Failed handling the subscription event of the operation {Operation}.")]
    public static partial void FailedSubscriptionEvent(
        this ILogger logger,
        Exception exception,
        IOperation operation
    );

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Error,
        Message = "Failed transporting the subscription of the operation {Operation}.")]
    public static partial void FailedSubscriptionTransport(
        this ILogger logger,
        Exception exception,
        IOperation operation
    );

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Error,
        Message = "The query {Query} has the syntax error {Error}.")]
    public static partial void FailedSyntax(
        this ILogger logger,
        Exception? exception,
        IQuery? query,
        string error
    );

    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Error,
        Message = "Failed processing the task {Task} with the error {Error}.")]
    public static partial void FailedTask(
        this ILogger logger,
        Exception? exception,
        IExecutionTask task,
        string error
    );

    [LoggerMessage(
        EventId = 6,
        Level = LogLevel.Error,
        Message = "Failed validating the query {Query} with the error {Error}.")]
    public static partial void FailedValidation(
        this ILogger logger,
        Exception? exception,
        IQuery? query,
        string error
    );
}

// Inspired by https://chillicream.com/blog/2019/03/19/logging-with-hotchocolate
// and https://chillicream.com/blog/2021/01/10/hot-chocolate-logging
public sealed class LoggingDiagnosticEventListener
    : ExecutionDiagnosticEventListener
{
    private static Stopwatch s_queryTimer = null!;
    private readonly ILogger<LoggingDiagnosticEventListener> _logger;

    public LoggingDiagnosticEventListener(
        ILogger<LoggingDiagnosticEventListener> logger
    )
    {
        _logger = logger;
    }

    // this diagnostic event is raised when a request is executed ...
    public override IDisposable ExecuteRequest(IRequestContext context)
    {
        // ... we will return an activity scope that is used to signal when the request is finished.
        return new RequestScope(_logger, context);
    }

    public override void RequestError(
        IRequestContext context,
        Exception exception
    )
    {
        _logger.FailedQueryExecution(exception, context.Request.Query);
    }

    public override void ResolverError(
        IMiddlewareContext context,
        IError error
    )
    {
        _logger.FailedOperationExecution(error.Exception, context.Operation, ConvertErrorToString(error));
    }

    public override void SubscriptionEventError(SubscriptionEventContext context, Exception exception)
    {
        _logger.FailedSubscriptionEvent(exception, context.Subscription.Operation);
    }

    public override void SubscriptionTransportError(ISubscription subscription, Exception exception)
    {
        _logger.FailedSubscriptionTransport(exception, subscription.Operation);
    }

    public override void SyntaxError(
        IRequestContext context,
        IError error
    )
    {
        _logger.FailedSyntax(error.Exception, context.Request.Query, ConvertErrorToString(error));
    }

    public override void TaskError(
        IExecutionTask task,
        IError error
    )
    {
        _logger.FailedTask(error.Exception, task, ConvertErrorToString(error));
    }

    public override void ValidationErrors(
        IRequestContext context,
        IReadOnlyList<IError> errors
    )
    {
        foreach (var error in errors)
            _logger.FailedValidation(error.Exception, context.Request.Query, ConvertErrorToString(error));
    }

    private static string ConvertErrorToString(
        IError error
    )
    {
        return $"{error}(Code {error.Code}, Message {error.Message}, Path {error.Path?.Print()})";
    }

    private sealed class RequestScope : IDisposable
    {
        private readonly IRequestContext _context;
        private readonly ILogger<LoggingDiagnosticEventListener> _logger;

        public RequestScope(ILogger<LoggingDiagnosticEventListener> logger, IRequestContext context)
        {
            _logger = logger;
            _context = context;
            s_queryTimer = new Stopwatch();
            s_queryTimer.Start();
        }

        public void Dispose()
        {
            // when the request is finished it will dispose the activity scope and
            // this is when we print the parsed query.
            if (_context.Document is not null)
            {
                // we just need to do a ToString on the Document which represents the parsed
                // GraphQL request document.
                StringBuilder stringBuilder = new(_context.Document.ToString(true));
                stringBuilder.AppendLine();

                if (_context.Variables != null)
                {
                    var variablesConcrete = _context.Variables!.ToList();
                    if (variablesConcrete.Count > 0)
                    {
                        stringBuilder.AppendFormat(CultureInfo.InvariantCulture,
                            $"Variables {Environment.NewLine}");
                        try
                        {
                            foreach (var variableValue in _context.Variables!)
                            {
                                static string PadRightHelper(string existingString, int lengthToPadTo)
                                {
                                    if (string.IsNullOrEmpty(existingString)) return "".PadRight(lengthToPadTo);

                                    if (existingString.Length > lengthToPadTo) return existingString[..lengthToPadTo];

                                    return existingString + " ".PadRight(lengthToPadTo - existingString.Length);
                                }

                                stringBuilder.AppendFormat(
                                    CultureInfo.InvariantCulture,
                                    $"  {PadRightHelper(variableValue.Name, 20)} :  {PadRightHelper(variableValue.Value.ToString(), 20)}: {variableValue.Type}");
                                stringBuilder.AppendFormat(CultureInfo.InvariantCulture, $"{Environment.NewLine}");
                            }
                        }
                        catch
                        {
                            // all input type records will land here.
                            stringBuilder.Append("  Formatting Variables Error. Continuing...");
                            stringBuilder.AppendFormat(CultureInfo.InvariantCulture, $"{Environment.NewLine}");
                        }
                    }
                }

                s_queryTimer.Stop();
                stringBuilder.AppendFormat(CultureInfo.InvariantCulture,
                    $"Ellapsed time for query is {s_queryTimer.Elapsed.TotalMilliseconds:0.#} milliseconds.");
#pragma warning disable CA1848 // Use the LoggerMessage delegates
#pragma warning disable CA2254 // Template should be a static expression
                _logger.LogInformation(stringBuilder.ToString());
#pragma warning restore CA2254 // Template should be a static expression
#pragma warning restore CA1848 // Use the LoggerMessage delegates
            }
        }
    }
}