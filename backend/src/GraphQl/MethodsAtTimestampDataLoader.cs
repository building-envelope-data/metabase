using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using Models = Icon.Models;

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