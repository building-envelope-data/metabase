using Guid = System.Guid;
using Models = Icon.Models;
using DateTime = System.DateTime;

namespace Icon.GraphQl
{
    public sealed class User
      : NodeBase
    {
        public static User FromModel(
            Models.User model,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            // TODO Event source user and use its timestamp
            return new User(
                id: model.Id,
                timestamp: DateTime.MinValue,
                requestTimestamp: requestTimestamp
                );
        }

        public User(
            Guid id,
            DateTime timestamp,
            DateTime requestTimestamp
            )
          : base(
              id: id,
              timestamp: timestamp,
              requestTimestamp: requestTimestamp
              )
        {
        }
    }
}