using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using Models = Icon.Models;

namespace Icon.GraphQl
{
    public sealed class MethodForTimestampedIdDataLoader
      : ModelForTimestampedIdDataLoader<Method, Models.Method>
    {
        public MethodForTimestampedIdDataLoader(IQueryBus queryBus)
          : base(Method.FromModel, queryBus)
        {
        }
    }
}