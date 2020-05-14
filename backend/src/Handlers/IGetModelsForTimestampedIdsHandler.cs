using Guid = System.Guid;
using System.Collections.Generic;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure.Command;
using Icon.Events;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Query;
using Marten;
using DateTime = System.DateTime;
using Models = Icon.Models;
using Queries = Icon.Queries;
using Aggregates = Icon.Aggregates;
using System.Linq;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.Handlers
{
    public interface IGetModelsForTimestampedIdsHandler
    {
        public Task<IEnumerable<Result<Models.IModel, Errors>>> HandleX(
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            IAggregateRepositoryReadOnlySession session,
            CancellationToken cancellationToken
            );
    }
}