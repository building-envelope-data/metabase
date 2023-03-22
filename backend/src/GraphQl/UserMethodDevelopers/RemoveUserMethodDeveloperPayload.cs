using System.Collections.Generic;
using Metabase.GraphQl.Methods;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.UserMethodDevelopers
{
    public sealed class RemoveUserMethodDeveloperPayload
    {
        public UserDevelopedMethodEdge? DevelopedMethodEdge { get; }
        public UserMethodDeveloperEdge? MethodDeveloperEdge { get; }
        public IReadOnlyCollection<RemoveUserMethodDeveloperError>? Errors { get; }

        public RemoveUserMethodDeveloperPayload(
            Data.UserMethodDeveloper userMethodDeveloper
            )
        {
            DevelopedMethodEdge = new UserDevelopedMethodEdge(userMethodDeveloper);
            MethodDeveloperEdge = new UserMethodDeveloperEdge(userMethodDeveloper);
        }

        public RemoveUserMethodDeveloperPayload(
            IReadOnlyCollection<RemoveUserMethodDeveloperError> errors
            )
        {
            Errors = errors;
        }

        public RemoveUserMethodDeveloperPayload(
            RemoveUserMethodDeveloperError error
            )
            : this(new[] { error })
        {
        }
    }
}