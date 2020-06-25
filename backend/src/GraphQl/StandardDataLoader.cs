using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

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