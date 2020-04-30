using Models = Icon.Models;
using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

namespace Icon.GraphQl
{
    public sealed class MethodDeveloperForTimestampedIdDataLoader
      : ModelForTimestampedIdDataLoader<MethodDeveloper, Models.MethodDeveloper>
    {
        public MethodDeveloperForTimestampedIdDataLoader(IQueryBus queryBus)
          : base(MethodDeveloper.FromModel, queryBus)
        {
        }
    }
}