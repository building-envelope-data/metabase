using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
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