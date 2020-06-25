using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

namespace Icon.GraphQl
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