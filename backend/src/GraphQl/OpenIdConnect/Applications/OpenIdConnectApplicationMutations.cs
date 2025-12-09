using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Metabase.Configuration;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Metabase.Extensions;
using Metabase.GraphQl.Users;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using OpenIddict.Core;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

[ExtendObjectType(nameof(Mutation))]
public sealed class OpenIdConnectApplicationMutations
{
    private static string GenerateClientSecret()
    {
        return RandomNumberGenerator.GetString("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+", 128);
    }

    [UseUserManager]
    [Authorize(Policy = AuthConfiguration.WritePolicy)]
    public async Task<CreateOpenIdConnectApplicationPayload> CreateOpenIdConnectApplicationAsync(
        CreateOpenIdConnectApplicationInput input,
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManageApplications(claimsPrincipal, input.InstitutionId, cancellationToken))
        {
            return new CreateOpenIdConnectApplicationPayload(
                new CreateOpenIdConnectApplicationError(
                    CreateOpenIdConnectApplicationErrorCode.UNAUTHORIZED,
                    "You are not authorized to create applications for the institution.",
                    [nameof(input), nameof(input.InstitutionId).FirstCharToLower()]
                )
            );
        }
        if (!await context.Institutions.AsQueryable()
                .AnyAsync(
                    x => x.Id == input.InstitutionId,
                    cancellationToken
                )
           )
        {
            return new CreateOpenIdConnectApplicationPayload(
                new CreateOpenIdConnectApplicationError(
                    CreateOpenIdConnectApplicationErrorCode.UNKNOWN_INSTITUTION,
                    "Unknown institution.",
                    [nameof(input), nameof(input.InstitutionId).FirstCharToLower()]
                )
            );
        }
        if (await applicationManager.FindByClientIdAsync(input.ClientId, cancellationToken) is not null)
        {
            return new CreateOpenIdConnectApplicationPayload(
                new CreateOpenIdConnectApplicationError(
                    CreateOpenIdConnectApplicationErrorCode.DUPLICATE_CLIENT_ID,
                    "The client ID is already in use.",
                    [nameof(input), nameof(input.ClientId).FirstCharToLower()]
                )
            );
        }
        var clientSecret = GenerateClientSecret();
        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = input.ClientId,
            ClientSecret = null,
            DisplayName = input.DisplayName,
            ConsentType = input.ConsentType.ToStringConsentType(),
            Permissions = { },
            Requirements = {
                OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange,
                OpenIddictConstants.Requirements.Features.PushedAuthorizationRequests,
            }
        };
        descriptor.Permissions.UnionWith(
            input.Endpoints.Select(x => x.ToStringEndpoint())
            .Concat(input.GrantTypes.Select(x => x.ToStringGrantType()))
            .Concat(input.ResponseTypes.Select(x => x.ToStringResponseType()))
            .Concat(input.Scopes.Select(x => x.ToStringScope()))
        );
        if (input.RedirectUri is not null)
        {
            descriptor.RedirectUris.Add(input.RedirectUri);
        }
        if (input.PostLogoutRedirectUri is not null)
        {
            descriptor.PostLogoutRedirectUris.Add(input.PostLogoutRedirectUri);
        }
        var application = new OpenIdConnectApplication
        {
            OwnerId = input.InstitutionId
        };
        await applicationManager.PopulateAsync(application, descriptor, cancellationToken);
        await applicationManager.CreateAsync(application, clientSecret, cancellationToken);
        return new CreateOpenIdConnectApplicationPayload(application, clientSecret);
    }

    [UseUserManager]
    [Authorize(Policy = AuthConfiguration.WritePolicy)]
    public async Task<ResetOpenIdConnectApplicationClientSecretPayload> ResetOpenIdConnectApplicationClientSecretAsync(
        ResetOpenIdConnectApplicationClientSecretInput input,
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManageApplication(
                claimsPrincipal,
                input.ApplicationId,
                cancellationToken
            ))
        {
            return new ResetOpenIdConnectApplicationClientSecretPayload(
                new ResetOpenIdConnectApplicationClientSecretError(
                    ResetOpenIdConnectApplicationClientSecretErrorCode.UNAUTHORIZED,
                    "You are not authorized to reset the application secret.",
                    [nameof(input), nameof(input.ApplicationId).FirstCharToLower()]
                )
            );
        }
        var application = await applicationManager.FindByIdAsync(input.ApplicationId.ToString(), cancellationToken);
        if (application is null)
        {
            return new ResetOpenIdConnectApplicationClientSecretPayload(
                new ResetOpenIdConnectApplicationClientSecretError(
                    ResetOpenIdConnectApplicationClientSecretErrorCode.UNKNOWN_APPLICATION,
                    "Unknown application.",
                    [nameof(input), nameof(input.ApplicationId).FirstCharToLower()]
                )
            );
        }
        var clientSecret = GenerateClientSecret();
        await applicationManager.UpdateAsync(application, clientSecret, cancellationToken);
        return new ResetOpenIdConnectApplicationClientSecretPayload(application, clientSecret);
    }

    [UseUserManager]
    [Authorize(Policy = AuthConfiguration.WritePolicy)]
    public async Task<UpdateOpenIdConnectApplicationPayload> UpdateOpenIdConnectApplicationAsync(
        UpdateOpenIdConnectApplicationInput input,
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManageApplication(
                claimsPrincipal,
                input.ApplicationId,
                cancellationToken
            ))
        {
            return new UpdateOpenIdConnectApplicationPayload(
                new UpdateOpenIdConnectApplicationError(
                    UpdateOpenIdConnectApplicationErrorCode.UNAUTHORIZED,
                    "You are not authorized to update the application.",
                    [nameof(input), nameof(input.ApplicationId).FirstCharToLower()]
                )
            );
        }
        var application = await applicationManager.FindByIdAsync(input.ApplicationId.ToString(), cancellationToken);
        if (application is null)
        {
            return new UpdateOpenIdConnectApplicationPayload(
                new UpdateOpenIdConnectApplicationError(
                    UpdateOpenIdConnectApplicationErrorCode.UNKNOWN_APPLICATION,
                    "Unknown application.",
                    [nameof(input), nameof(input.ApplicationId).FirstCharToLower()]
                )
            );
        }
        var descriptor = new OpenIddictApplicationDescriptor();
        await applicationManager.PopulateAsync(descriptor, application, cancellationToken);
        UpdateOpenIdConnectApplicationDescriptor(input, descriptor);
        await applicationManager.UpdateAsync(application, descriptor, cancellationToken);
        return new UpdateOpenIdConnectApplicationPayload(application);
    }

    [UseUserManager]
    [Authorize(Policy = AuthConfiguration.WritePolicy)]
    public async Task<DeleteOpenIdConnectApplicationPayload> DeleteOpenIdConnectApplicationAsync(
        DeleteOpenIdConnectApplicationInput input,
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManageApplication(
                claimsPrincipal,
                input.ApplicationId,
                cancellationToken
            ))
        {
            return new DeleteOpenIdConnectApplicationPayload(
                new DeleteOpenIdConnectApplicationError(
                    DeleteOpenIdConnectApplicationErrorCode.UNAUTHORIZED,
                    "You are not authorized to delete the application.",
                    [nameof(input), nameof(input.ApplicationId).FirstCharToLower()]
                )
            );
        }
        var application = await applicationManager.FindByIdAsync(input.ApplicationId.ToString(), cancellationToken);
        if (application is null)
        {
            return new DeleteOpenIdConnectApplicationPayload(
                new DeleteOpenIdConnectApplicationError(
                    DeleteOpenIdConnectApplicationErrorCode.UNKNOWN_APPLICATION,
                    "Unknown application.",
                    [nameof(input), nameof(input.ApplicationId).FirstCharToLower()]
                )
            );
        }
        await applicationManager.DeleteAsync(application, cancellationToken);
        return new DeleteOpenIdConnectApplicationPayload();
    }

    private static void UpdateOpenIdConnectApplicationDescriptor(UpdateOpenIdConnectApplicationInput input, OpenIddictApplicationDescriptor descriptor)
    {
        descriptor.ClientId = input.ClientId;
        descriptor.DisplayName = input.DisplayName;
        descriptor.ConsentType = input.ConsentType.ToStringConsentType();
        descriptor.RedirectUris.Clear();
        if (input.RedirectUri is not null)
        {
            descriptor.RedirectUris.Add(input.RedirectUri);
        }
        descriptor.PostLogoutRedirectUris.Clear();
        if (input.PostLogoutRedirectUri is not null)
        {
            descriptor.PostLogoutRedirectUris.Add(input.PostLogoutRedirectUri);
        }
        descriptor.Permissions.RemoveWhere(permission =>
        {
            try
            {
                permission.ToOpenIdConnectEndpoint();
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        });
        descriptor.Permissions.RemoveWhere(permission =>
        {
            try
            {
                permission.ToOpenIdConnectGrantType();
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        });
        descriptor.Permissions.RemoveWhere(permission =>
        {
            try
            {
                permission.ToOpenIdConnectResponseType();
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        });
        descriptor.Permissions.RemoveWhere(permission =>
        {
            try
            {
                permission.ToOpenIdConnectScope();
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        });
        descriptor.Permissions.UnionWith(
            input.Endpoints.Select(x => x.ToStringEndpoint())
            .Concat(input.GrantTypes.Select(x => x.ToStringGrantType()))
            .Concat(input.ResponseTypes.Select(x => x.ToStringResponseType()))
            .Concat(input.Scopes.Select(x => x.ToStringScope()))
        );
    }
}