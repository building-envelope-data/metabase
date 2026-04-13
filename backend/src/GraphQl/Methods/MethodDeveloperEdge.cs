using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Institutions;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Methods;

public sealed class MethodDeveloperEdge
{
    private readonly InstitutionMethodDeveloperEdge? _institutionMethodDeveloperEdge;
    private readonly UserMethodDeveloperEdge? _userMethodDeveloperEdge;

    public MethodDeveloperEdge(
        InstitutionMethodDeveloperEdge edge
    )
    {
        _institutionMethodDeveloperEdge = edge;
    }

    public MethodDeveloperEdge(
        UserMethodDeveloperEdge edge
    )
    {
        _userMethodDeveloperEdge = edge;
    }

    public async Task<IStakeholder> GetNodeAsync(
        InstitutionByIdDataLoader institutionById,
        UserByIdDataLoader userById,
        CancellationToken cancellationToken
    )
    {
        if (_institutionMethodDeveloperEdge is not null)
        {
            return await _institutionMethodDeveloperEdge.GetNodeAsync(institutionById, cancellationToken);
        }
        if (_userMethodDeveloperEdge is not null)
        {
            return await _userMethodDeveloperEdge.GetNodeAsync(userById, cancellationToken);
        }
        throw new ArgumentException("Impossible!");
    }

    [UseUserManager]
    public async Task<bool> IsAuthorizedToConfirmEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        InstitutionMethodDeveloperAuthorization institutionMethodDeveloperAuthorization,
        UserMethodDeveloperAuthorization userMethodDeveloperAuthorization,
        CancellationToken cancellationToken
    )
    {
        if (_institutionMethodDeveloperEdge is not null)
        {
            return await _institutionMethodDeveloperEdge.IsAuthorizedToConfirmEdgeAsync(claimsPrincipal, institutionMethodDeveloperAuthorization, cancellationToken);
        }
        if (_userMethodDeveloperEdge is not null)
        {
            return await _userMethodDeveloperEdge.IsAuthorizedToConfirmEdgeAsync(claimsPrincipal, userMethodDeveloperAuthorization, cancellationToken);
        }
        throw new ArgumentException("Impossible!");
    }

    [UseUserManager]
    public async Task<bool> IsAuthorizedToRemoveEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        InstitutionMethodDeveloperAuthorization institutionMethodDeveloperAuthorization,
        UserMethodDeveloperAuthorization userMethodDeveloperAuthorization,
        CancellationToken cancellationToken
    )
    {
        if (_institutionMethodDeveloperEdge is not null)
        {
            return await _institutionMethodDeveloperEdge
                .IsAuthorizedToRemoveEdgeAsync(claimsPrincipal, institutionMethodDeveloperAuthorization, cancellationToken);
        }
        if (_userMethodDeveloperEdge is not null)
        {
            return await _userMethodDeveloperEdge
                .IsAuthorizedToRemoveEdgeAsync(claimsPrincipal, userMethodDeveloperAuthorization, cancellationToken);
        }
        throw new ArgumentException("Impossible!");
    }
}