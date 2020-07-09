// Inspired by https://hotchocolate.io/docs/instrumentation
// and https://github.com/ChilliCream/hotchocolate-examples/tree/master/Instrumentation

using System;
using System.Collections.Generic;
using System.Linq;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Execution.Instrumentation;
using HotChocolate.Resolvers;
using Microsoft.Extensions.DiagnosticAdapter;
using Microsoft.Extensions.Logging;

namespace Infrastructure.GraphQl
{
    public class DiagnosticObserver
      : IDiagnosticObserver
    {
        private readonly ILogger _logger;

        public DiagnosticObserver(ILogger<DiagnosticObserver> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /* QUERY */

        [DiagnosticName("HotChocolate.Execution.Query")]
        public void OnQuery(IQueryContext context)
        {
            // This method is used as marker to enable begin and end events
            // in the case that you want to explicitly track the start and the
            // end of this event.
        }

        [DiagnosticName("HotChocolate.Execution.Query.Start")]
        public void BeginQueryExecute(IQueryContext context)
        {
            _logger.LogInformation(context.Request.Query.ToString());
            /* _logger.LogInformation(context.Request.QueryName); */
            /* _logger.LogInformation(context.Request.QueryHash); */
            /* _logger.LogInformation(context.Request.OperationName); */
            /* _logger.LogInformation(JsonSerializer.Serialize(context.Request.VariableValues)); */
            /* _logger.LogInformation(context.Request.InitialValue?.ToString()); */
            /* _logger.LogInformation(JsonSerializer.Serialize(context.Request.Properties)); */
            /* _logger.LogInformation(JsonSerializer.Serialize(context.Request.Extensions)); */
        }

        [DiagnosticName("HotChocolate.Execution.Query.Stop")]
        public void EndQueryExecute(IQueryContext context)
        {
            // using (var stream = new MemoryStream())
            // {
            //     var resultSerializer = new JsonQueryResultSerializer();
            //     resultSerializer.SerializeAsync(
            //         (IReadOnlyQueryResult)context.Result,
            //         stream).Wait();
            //     _logger.LogInformation(
            //         Encoding.UTF8.GetString(stream.ToArray()));
            // }
        }

        [DiagnosticName("HotChocolate.Execution.Query.Error")]
        public virtual void OnQueryError(
              IQueryContext context,
              Exception exception)
        {
            _logger.LogError(exception.ToString());
        }

        /* PARSING */

        [DiagnosticName("HotChocolate.Execution.Parsing.Start")]
        public void BeginParsing(IQueryContext context)
        {
            // ... your code
        }

        [DiagnosticName("HotChocolate.Execution.Parsing.Stop")]
        public void EndParsing(IQueryContext context)
        {
            // ... your code
        }

        /* VALIDATION */

        [DiagnosticName("HotChocolate.Execution.Validation.Start")]
        public void BeginValidation(IQueryContext context)
        {
            // ... your code
        }

        [DiagnosticName("HotChocolate.Execution.Validation.Stop")]
        public void EndValidation(IQueryContext context)
        {
            // ... your code
        }

        [DiagnosticName("HotChocolate.Execution.Validation.Error")]
        public void OnValidationError(
              IQueryContext context,
              IReadOnlyCollection<IError> errors)
        {
            Log(errors);
        }

        /* OPERATION */

        [DiagnosticName("HotChocolate.Execution.Operation.Start")]
        public void BeginOperationExecute(IQueryContext context)
        {
            // ... your code
        }

        [DiagnosticName("HotChocolate.Execution.Operation.Stop")]
        public void EndOperationExecute(
            IQueryContext context,
            IExecutionResult result)
        {
            // ... your code
        }

        /* RESOLVER */

        [DiagnosticName("HotChocolate.Execution.Resolver.Start")]
        public void BeginResolveField(IResolverContext context)
        {
            // ... your code
        }

        [DiagnosticName("HotChocolate.Execution.Resolver.Stop")]
        public void EndResolveField(
            IResolverContext context,
            object result)
        {
            // ... your code
        }

        [DiagnosticName("HotChocolate.Execution.Resolver.Error")]
        public void OnResolverError(
              IResolverContext context,
              IEnumerable<IError> errors)
        {
            Log(errors);
        }

        /* HELPER */

        private void Log(IEnumerable<IError> errors)
        {
            foreach (IError error in errors)
            {
                var path = error.Path is null
                  ? "unknown-path"
                  : string.Join(
                    "/",
                    error.Path.Select(t => t.ToString())
                    );

                if (error.Exception is null)
                {
                    _logger.LogError("{0}\r\n{1}", path, error.Message);
                }
                else
                {
                    _logger.LogError(error.Exception,
                        "{0}\r\n{1}", path, error.Message);
                }
            }
        }
    }
}