using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
    public sealed class AddUserRolePayload
    {
        public Data.User? User { get; }
        public IReadOnlyCollection<AddUserRoleError>? Errors { get; }

        public AddUserRolePayload(
            Data.User user
        )
        {
            User = user;
        }

        public AddUserRolePayload(
            IReadOnlyCollection<AddUserRoleError> errors
        )
        {
            Errors = errors;
        }

        public AddUserRolePayload(
            Data.User user,
            IReadOnlyCollection<AddUserRoleError> errors
        )
        {
            User = user;
            Errors = errors;
        }

        public AddUserRolePayload(
            AddUserRoleError error
        )
            : this(new[] { error })
        {
        }
    }
}