using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using Models = Icon.Models;

namespace Icon.GraphQl
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