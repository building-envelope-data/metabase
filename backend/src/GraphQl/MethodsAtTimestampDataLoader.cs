using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using Models = Icon.Models;

namespace Icon.GraphQl
{
    public class MethodsAtTimestampDataLoader
      : ModelsAtTimestampDataLoader<Method, Models.Method>
    {
        public MethodsAtTimestampDataLoader(IQueryBus queryBus)
          : base(Method.FromModel, queryBus)
        {
        }
    }
}