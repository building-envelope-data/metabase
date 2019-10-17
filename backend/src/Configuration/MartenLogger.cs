// Inspired by http://jasperfx.github.io/marten/documentation/documents/diagnostics/

using System;
using System.Diagnostics;
using System.Linq;
using Marten.Services;
using Marten;
using Npgsql;
using Microsoft.Extensions.Logging;

namespace Icon.Configuration
{
    public class MartenLogger : IMartenLogger, IMartenSessionLogger
    {
        private readonly ILogger<EventStore> _logger;

        internal MartenLogger(ILogger<EventStore> logger)
        {
            _logger = logger;
        }

        public IMartenSessionLogger StartSession(IQuerySession session)
        {
            return this;
        }

        public void SchemaChange(string sql)
        {
            _logger.LogDebug("Executing DDL change:");
            _logger.LogDebug(sql);
            _logger.LogDebug("");
        }

        public void LogSuccess(NpgsqlCommand command)
        {
            _logger.LogDebug(command.CommandText);
            foreach (var p in command.Parameters.OfType<NpgsqlParameter>())
            {
                _logger.LogDebug($"  {p.ParameterName}: {p.Value}");
            }
        }

        public void LogFailure(NpgsqlCommand command, Exception ex)
        {
            _logger.LogDebug("Postgresql command failed!");
            _logger.LogDebug(command.CommandText);
            foreach (var p in command.Parameters.OfType<NpgsqlParameter>())
            {
                _logger.LogDebug($"  {p.ParameterName}: {p.Value}");
            }
            _logger.LogDebug(ex.ToString());
        }

        public void RecordSavedChanges(IDocumentSession session, IChangeSet commit)
        {
            var lastCommit = commit;
            _logger.LogDebug(
                $"Persisted {lastCommit.Updated.Count()} updates, {lastCommit.Inserted.Count()} inserts, and {lastCommit.Deleted.Count()} deletions");
        }
    }
}