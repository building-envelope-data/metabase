using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using CancellationToken = System.Threading.CancellationToken;
using Errors = Infrastructure.Errors;

namespace Metabase.Handlers
{
    public interface IGetModelsForTimestampedIdsHandler
    {
        public Task<IEnumerable<Result<IModel, Errors>>> HandleX(
            IAggregateRepositoryReadOnlySession session,
            IEnumerable<TimestampedId> timestampedIds,
            CancellationToken cancellationToken
            );
    }
}