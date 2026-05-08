using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Data;
using Metabase.GraphQl.Methods;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.UserMethodDevelopers;

public sealed class RemoveUserMethodDeveloperPayload
{
    private readonly UserMethodDeveloper? _association;

    public RemoveUserMethodDeveloperPayload(
        UserMethodDeveloper userMethodDeveloper
    )
    {
        _association = userMethodDeveloper;
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
        : this([error])
    {
    }

    public IReadOnlyCollection<RemoveUserMethodDeveloperError>? Errors { get; }

    public async Task<Method?> GetMethodAsync(
        IMethodByIdDataLoader byId,
        CancellationToken cancellationToken
    )
    {
        if (_association is null)
        {
            return null;
        }
        return await byId.LoadAsync(_association.MethodId, cancellationToken);
    }

    public async Task<User?> GetUserAsync(
        IUserByIdDataLoader byId,
        CancellationToken cancellationToken
    )
    {
        if (_association is null)
        {
            return null;
        }
        return await byId.LoadAsync(_association.UserId, cancellationToken);
    }
}