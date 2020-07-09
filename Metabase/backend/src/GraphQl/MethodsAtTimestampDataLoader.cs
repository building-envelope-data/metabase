using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
{
    public sealed class MethodsAtTimestampDataLoader
      : ModelsAtTimestampDataLoader<Method, Models.Method>
    {
        public MethodsAtTimestampDataLoader(IQueryBus queryBus)
          : base(Method.FromModel, queryBus)
        {
        }
    }
}