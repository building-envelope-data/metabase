using System.Collections.Generic;
using Metabase.GraphQl.Methods;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.UserMethodDevelopers
{
    public sealed class AddUserMethodDeveloperPayload
    {
        public UserDevelopedMethodEdge? DevelopedMethodEdge { get; }
        public UserMethodDeveloperEdge? MethodDeveloperEdge { get; }
        public IReadOnlyCollection<AddUserMethodDeveloperError>? Errors { get; }

        public AddUserMethodDeveloperPayload(
            Data.UserMethodDeveloper userMethodDeveloper
            )
        {
            DevelopedMethodEdge = new UserDevelopedMethodEdge(userMethodDeveloper);
            MethodDeveloperEdge = new UserMethodDeveloperEdge(userMethodDeveloper);
        }

        public AddUserMethodDeveloperPayload(
            IReadOnlyCollection<AddUserMethodDeveloperError> errors
            )
        {
            Errors = errors;
        }

        public AddUserMethodDeveloperPayload(
            AddUserMethodDeveloperError error
            )
            : this(new[] { error })
        {
        }
    }
}