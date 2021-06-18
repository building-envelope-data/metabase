using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using HotChocolate.Execution;
using HotChocolate.Execution.Instrumentation;
using Microsoft.Extensions.Logging;

namespace Metabase.GraphQl
{
    // Inspired by https://chillicream.com/blog/2019/03/19/logging-with-hotchocolate
    // and https://chillicream.com/blog/2021/01/10/hot-chocolate-logging
    public sealed class LoggingDiagnosticEventListener
    : DiagnosticEventListener
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
        public override IActivityScope ExecuteRequest(IRequestContext context)
        {
            // ... we will return an activity scope that is used to signal when the request is finished.
            return new RequestScope(_logger, context);
        }

        public override void RequestError(
            IRequestContext context,
            Exception exception
            )
        {
            _logger.LogError($"During execution of the query {context.Request.Query} the exception {exception} occurred.");
        }

        private class RequestScope : IActivityScope
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
                            stringBuilder.AppendFormat($"Variables {Environment.NewLine}");
                            try
                            {
                                foreach (var variableValue in _context.Variables!)
                                {
                                    static string PadRightHelper(string existingString, int lengthToPadTo)
                                    {
                                        if (string.IsNullOrEmpty(existingString))
                                        {
                                            return "".PadRight(lengthToPadTo);
                                        }

                                        if (existingString.Length > lengthToPadTo)
                                        {
                                            return existingString.Substring(0, lengthToPadTo);
                                        }

                                        return existingString + " ".PadRight(lengthToPadTo - existingString.Length);
                                    }
                                    stringBuilder.AppendFormat(
                                        $"  {PadRightHelper(variableValue.Name, 20)} :  {PadRightHelper(variableValue.Value.ToString(), 20)}: {variableValue.Type}");
                                    stringBuilder.AppendFormat($"{Environment.NewLine}");
                                }
                            }
                            catch
                            {
                                // all input type records will land here.
                                stringBuilder.Append("  Formatting Variables Error. Continuing...");
                                stringBuilder.AppendFormat($"{Environment.NewLine}");
                            }
                        }
                    }
                    s_queryTimer.Stop();
                    stringBuilder.AppendFormat($"Ellapsed time for query is {s_queryTimer.Elapsed.TotalMilliseconds:0.#} milliseconds.");
                    _logger.LogInformation(stringBuilder.ToString());
                }
            }
        }
    }
}