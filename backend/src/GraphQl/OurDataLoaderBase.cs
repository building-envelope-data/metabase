using System.Collections.Generic;
using GreenDonut;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Icon.Infrastructure.Query;

namespace Icon.GraphQl
{
    public abstract class OurDataLoaderBase<TKey, TValue>
      : DataLoaderBase<TKey, TValue>
    {
        protected IQueryBus QueryBus { get; }

        protected OurDataLoaderBase(IQueryBus queryBus)
            : base()
        {
            QueryBus = queryBus;
        }

        protected OurDataLoaderBase(ITaskCache<TValue> cache, IQueryBus queryBus)
            : base(cache)
        {
            QueryBus = queryBus;
        }

        protected OurDataLoaderBase(DataLoaderOptions<TKey> options, IQueryBus queryBus)
            : base(options)
        {
            QueryBus = queryBus;
        }

        protected OurDataLoaderBase(
            DataLoaderOptions<TKey> options,
            ITaskCache<TValue> cache,
            IQueryBus queryBus)
          : base(options, cache)
        {
            QueryBus = queryBus;
        }
    }
}