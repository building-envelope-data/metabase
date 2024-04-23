using System.Collections.Generic;
using Metabase.GraphQl.Methods;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.UserMethodDevelopers
{
    public sealed class ConfirmUserMethodDeveloperPayload
    {
        public UserDevelopedMethodEdge? DevelopedMethodEdge { get; }
        public UserMethodDeveloperEdge? MethodDeveloperEdge { get; }
        public IReadOnlyCollection<ConfirmUserMethodDeveloperError>? Errors { get; }

        public ConfirmUserMethodDeveloperPayload(
            Data.UserMethodDeveloper userMethodDeveloper
        )
        {
            DevelopedMethodEdge = new UserDevelopedMethodEdge(userMethodDeveloper);
            MethodDeveloperEdge = new UserMethodDeveloperEdge(userMethodDeveloper);
        }

        public ConfirmUserMethodDeveloperPayload(
            IReadOnlyCollection<ConfirmUserMethodDeveloperError> errors
        )
        {
            Errors = errors;
        }

        public ConfirmUserMethodDeveloperPayload(
            ConfirmUserMethodDeveloperError error
        )
            : this(new[] { error })
        {
        }
    }
}