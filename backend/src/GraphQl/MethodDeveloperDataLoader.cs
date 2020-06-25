using IQueryBus = Icon.Infrastructure.Query.IQueryBus;

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