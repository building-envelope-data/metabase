using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using Models = Icon.Models;

namespace Icon.GraphQl
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