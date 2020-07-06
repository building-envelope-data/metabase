using Infrastructure.ValueObjects;
using IQueryBus = Infrastructure.Queries.IQueryBus;

namespace Metabase.GraphQl
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