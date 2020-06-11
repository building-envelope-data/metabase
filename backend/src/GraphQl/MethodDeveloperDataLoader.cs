using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using Models = Icon.Models;

namespace Icon.GraphQl
{
    public sealed class MethodDeveloperDataLoader
      : ModelDataLoader<MethodDeveloper, Models.MethodDeveloper>
    {
        public MethodDeveloperDataLoader(IQueryBus queryBus)
          : base(MethodDeveloper.FromModel, queryBus)
        {
        }
    }
}