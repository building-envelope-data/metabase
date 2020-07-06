using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
{
    public sealed class MethodDataLoader
      : ModelDataLoader<Method, Models.Method>
    {
        public MethodDataLoader(IQueryBus queryBus)
          : base(Method.FromModel, queryBus)
        {
        }
    }
}