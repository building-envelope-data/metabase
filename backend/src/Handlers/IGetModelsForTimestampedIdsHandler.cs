using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Aggregate;
using CancellationToken = System.Threading.CancellationToken;

namespace Icon.Handlers
{
    public interface IGetModelsForTimestampedIdsHandler
    {
        public Task<IEnumerable<Result<Models.IModel, Errors>>> HandleX(
            IAggregateRepositoryReadOnlySession session,
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            CancellationToken cancellationToken
            );
    }
}