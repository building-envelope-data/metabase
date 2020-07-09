using Infrastructure.GraphQl;
using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
{
    public sealed class UserDataLoader
      : ModelDataLoader<User, Models.User>
    {
        public UserDataLoader(IQueryBus queryBus)
          : base(User.FromModel, queryBus)
        {
        }
    }
}