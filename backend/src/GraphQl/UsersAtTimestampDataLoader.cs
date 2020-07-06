using IQueryBus = Icon.Infrastructure.Queries.IQueryBus;

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