using Infrastructure.GraphQl;
using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
{
    public sealed class StandardDataLoader
      : ModelDataLoader<Standard, Models.Standard>
    {
        public StandardDataLoader(IQueryBus queryBus)
          : base(Standard.FromModel, queryBus)
        {
        }
    }
}