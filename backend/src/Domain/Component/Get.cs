using Guid = System.Guid;
using System.Collections.Generic;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Event;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Query;
using Icon.Domain;
using Marten;
using ZonedDateTime = NodaTime.ZonedDateTime;

namespace Icon.Domain.Component.Get
{
    public class Query : IQuery<ComponentView>
    {
      public Guid ComponentId { get; set; }
      public ZonedDateTime Timestamp { get; set; }
    }

    public class QueryHandler :
        IQueryHandler<Query, ComponentView>
    {
        private readonly IDocumentSession _session;

        public QueryHandler(IDocumentSession session)
        {
            _session = session;
        }

        public async Task<ComponentView> Handle(Query query, CancellationToken cancellationToken)
        {
            // TODO Use `query.Timestamp` to load state at the specified point in time
            return await _session.LoadAsync<ComponentView>(query.ComponentId, cancellationToken);
        }
    }
}