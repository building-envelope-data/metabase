using System.Collections.Generic;
using Icon.Infrastructure.Query;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace Icon.GraphQl
{
    public class ResolversBase
    {
        protected IQueryBus QueryBus { get; }

        public ResolversBase(IQueryBus queryBus)
        {
            QueryBus = queryBus;
        }
    }
}