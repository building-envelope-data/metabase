using IQueryBus = Icon.Infrastructure.Query.IQueryBus;
using Models = Icon.Models;

namespace Icon.GraphQl
{
    public sealed class UsersAtTimestampDataLoader
      : ModelsAtTimestampDataLoader<User, Models.User>
    {
        public UsersAtTimestampDataLoader(IQueryBus queryBus)
          : base(User.FromModel, queryBus)
        {
        }
    }
}