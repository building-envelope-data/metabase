using System;
using Guid = System.Guid;
using System.Collections.Generic;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Event;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Query;
using Models = Icon.Models;
using Queries = Icon.Queries;
using Events = Icon.Events;
using Aggregates = Icon.Aggregates;
using System.Linq;
using Marten;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.Handlers
{
    public class ListComponentsHandler :
      IQueryHandler<Queries.ListComponents, IEnumerable<Result<Models.Component, IError>>>
    {
        private readonly IAggregateRepository _repository;

        public ListComponentsHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Result<Models.Component, IError>>> Handle(Queries.ListComponents query, CancellationToken cancellationToken)
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                var ids =
                  await session
                  .Query<Events.ComponentCreated>()
                  .Select(e => e.ComponentId)
                  .ToListAsync();

                return
                  (await session
                   .LoadAllThatExisted<Aggregates.ComponentAggregate>(
                     ids,
                     query.Timestamp,
                     cancellationToken
                     )
                  ).Select(result =>
                    result.Map(a => a.ToModel())
                    );

            }
        }
    }
}