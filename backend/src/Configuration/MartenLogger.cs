// Inspired by http://jasperfx.github.io/marten/documentation/documents/diagnostics/
// and https://github.com/JasperFx/marten/blob/master/src/Marten.Testing/Examples/SimpleSessionListener.cs

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Baseline;
using Marten;
using Marten.Services;
using Microsoft.Extensions.Logging;
using Npgsql;
using CancellationToken = System.Threading.CancellationToken;

namespace Icon.Configuration
{
    public sealed class MartenLogger : IMartenLogger, IMartenSessionLogger, IDocumentSessionListener
    {
        private readonly ILogger<EventStore> _logger;

        internal MartenLogger(ILogger<EventStore> logger)
        {
            _logger = logger;
        }

        ///////////////////
        // IMartenLogger //
        ///////////////////
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

        //////////////////////////
        // IMartenSessionLogger //
        //////////////////////////
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

        //////////////////////////////
        // IDocumentSessionListener //
        //////////////////////////////
        public void DocumentLoaded(object id, object document)
        {
            _logger.LogDebug(
                $"Document with id {id} loaded: {document}");
        }

        public void DocumentAddedForStorage(object id, object document)
        {
            _logger.LogDebug(
                $"Document with id {id} added for storage: {document}");
        }

        public void BeforeSaveChanges(IDocumentSession session)
        {
            _logger.LogDebug(
                $"About to append the following pending changes to the event store:");
            session.PendingChanges.Streams()
                  .Each(s => _logger.LogDebug(s.ToString()));
        }

        public Task BeforeSaveChangesAsync(IDocumentSession session, CancellationToken token)
        {
            _logger.LogDebug(
                $"About to append the following pending asynchronous changes to the event store:");
            session.PendingChanges.Streams()
                  .Each(s => _logger.LogDebug(s.ToString()));
            return Task.CompletedTask;
        }

        public Task AfterCommitAsync(IDocumentSession session, IChangeSet commit, CancellationToken token)
        {
            commit.Updated.Each(x => Debug.WriteLine($"{x} was updated"));
            commit.Deleted.Each(x => Debug.WriteLine($"{x} was deleted"));
            commit.Inserted.Each(x => Debug.WriteLine($"{x} was inserted"));
            return Task.CompletedTask;
        }

        public void AfterCommit(IDocumentSession session, IChangeSet commit)
        {
            commit.Updated.Each(x => Debug.WriteLine($"{x} was updated"));
            commit.Deleted.Each(x => Debug.WriteLine($"{x} was deleted"));
            commit.Inserted.Each(x => Debug.WriteLine($"{x} was inserted"));
        }
    }
}