using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Metabase.Extensions;
using Metabase.GraphQl.Entities;
using Metabase.GraphQl.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;
using UserRole = Metabase.Enumerations.UserRole;

namespace Metabase.GraphQl.Users;

public sealed class UserType
    : EntityType<User, UserByIdDataLoader>
{
    private static async Task<T?> Authorize<T>(
        IResolverContext context,
        Func<User, T?> getValue,
        string? scope = null
    )
        where T : class
    {
        var claimsPrincipal =
            context.GetGlobalStateOrDefault<ClaimsPrincipal>(nameof(ClaimsPrincipal))
            ?? throw new ArgumentException("Claims principal must not be null.");
        var authorization = context.Service<UserAuthorization>();
        if (scope is not null && !claimsPrincipal.HasScope(scope))
        {
            return null;
        }

        var user = context.Parent<User>();
        if (!await authorization.IsAuthorizedToManageUser(claimsPrincipal, user.Id, context.RequestAborted))
        {
            return null;
        }

        return getValue(user);
    }

    private static async Task<T?> Authorize<T>(
        IResolverContext context,
        Func<User, T?> getValue,
        string? scope = null
    )
        where T : struct
    {
        var claimsPrincipal =
            context.GetGlobalStateOrDefault<ClaimsPrincipal>(nameof(ClaimsPrincipal))
            ?? throw new ArgumentException("Claims principal must not be null.");
        var authorization = context.Service<UserAuthorization>();
        if (scope is not null && !claimsPrincipal.HasScope(scope))
        {
            return null;
        }

        var user = context.Parent<User>();
        if (!await authorization.IsAuthorizedToManageUser(claimsPrincipal, user.Id, context.RequestAborted))
        {
            return null;
        }

        return getValue(user);
    }

    private static async Task<T?> AuthorizeAsync<T>(
        IResolverContext context,
        Func<User, UserAuthorization, Task<T?>> getValue,
        string? scope = null
    )
        where T : class
    {
        var claimsPrincipal =
            context.GetGlobalStateOrDefault<ClaimsPrincipal>(nameof(ClaimsPrincipal))
            ?? throw new ArgumentException("Claims principal must not be null.");
        var authorization = context.Service<UserAuthorization>();
        if (scope is not null && !claimsPrincipal.HasScope(scope))
        {
            return null;
        }

        var user = context.Parent<User>();
        if (!await authorization.IsAuthorizedToManageUser(claimsPrincipal, user.Id, context.RequestAborted))
        {
            return null;
        }

        return await getValue(user, authorization);
    }

    private static async Task<T?> AuthorizeAsync<T>(
        IResolverContext context,
        Func<User, UserAuthorization, Task<T?>> getValue,
        string? scope = null
    )
        where T : struct
    {
        var claimsPrincipal =
            context.GetGlobalStateOrDefault<ClaimsPrincipal>(nameof(ClaimsPrincipal))
            ?? throw new ArgumentException("Claims principal must not be null.");
        var authorization = context.Service<UserAuthorization>();
        if (scope is not null && !claimsPrincipal.HasScope(scope))
        {
            return null;
        }

        var user = context.Parent<User>();
        if (!await authorization.IsAuthorizedToManageUser(claimsPrincipal, user.Id, context.RequestAborted))
        {
            return null;
        }

        return await getValue(user, authorization);
    }

    protected override void Configure(
        IObjectTypeDescriptor<User> descriptor
    )
    {
        descriptor.BindFieldsExplicitly();
        base.Configure(descriptor);
        // Keep authorization scopes in sync with `UserinfoController`.
        descriptor
            .Field(t => t.Name)
            // .Type<NonNullType<StringType>>()
            .Resolve(async context =>
                // Instead of returning `null`, we return a string because otherwise the
                // corresponding GraphQL field would need to be nullable and because the type `User`
                // implements `IStakeholder`, the stakeholder name would also need to be nullable.
                await Authorize(context, user => user.Name, Scopes.Profile) ??
                "<redacted>"
            )
            .UseUserManager();
        descriptor
            .Field("contact")
            .Type<NonNullType<ObjectType<ContactInformation>>>()
            .Resolve(async context =>
                new ContactInformation(
                    await Authorize(context, user => user.PhoneNumber, Scopes.Phone),
                    await Authorize<bool>(context, user => user.PhoneNumberConfirmed, Scopes.Phone) ?? false,
                    await Authorize(context, user => user.PostalAddress, Scopes.Address),
                    await Authorize(context, user => user.Email, Scopes.Email),
                    await Authorize<bool>(context, user => user.EmailConfirmed, Scopes.Email) ?? false,
                    await Authorize(context, user => user.WebsiteLocator, Scopes.Profile)
                )
            )
            .UseUserManager();
        descriptor
            .Field(t => t.Email)
            .Deprecated("Moved to `contact.emailAddress")
            .Resolve(context =>
                Authorize(context, user => user.Email, Scopes.Email)
            )
            .UseUserManager();
        descriptor
            .Field(t => t.EmailConfirmed)
            .Deprecated("Moved to `contact.isEmailAddress")
            .Name($"is{nameof(User.EmailConfirmed)}")
            .Type<BooleanType>()
            .Resolve(context =>
                Authorize<bool>(context, user => user.EmailConfirmed, Scopes.Email)
            )
            .UseUserManager();
        descriptor
            .Field(t => t.PostalAddress)
            .Deprecated("Moved to `contact.postalAddress")
            .Resolve(context =>
                Authorize(context, user => user.PostalAddress, Scopes.Address)
            )
            .UseUserManager();
        descriptor
            .Field(t => t.PhoneNumber)
            .Deprecated("Moved to `contact.phoneNumber")
            .Resolve(context =>
                Authorize(context, user => user.PhoneNumber, Scopes.Phone)
            )
            .UseUserManager();
        descriptor
            .Field(t => t.PhoneNumberConfirmed)
            .Deprecated("Moved to `contact.isPhoneNumberConfirmed")
            .Name($"is{nameof(User.PhoneNumberConfirmed)}")
            .Type<BooleanType>()
            .Resolve(context =>
                Authorize<bool>(context, user => user.PhoneNumberConfirmed, Scopes.Phone)
            )
            .UseUserManager();
        descriptor
            .Field(t => t.WebsiteLocator)
            .Deprecated("Moved to `contact.websiteLocator")
            .Resolve(context =>
                Authorize(context, user => user.WebsiteLocator, Scopes.Profile)
            )
            .UseUserManager();
        descriptor
            .Field("twoFactorAuthentication")
            .ResolveWith<UserResolvers>(t =>
                UserResolvers.GetTwoFactorAuthenticationAsync(default!, default!, default!, default!, default!, default!))
            .UseUserManager()
            .UseSignInManager();
        descriptor
            .Field("hasPassword")
            .Type<BooleanType>()
            .Resolve(context =>
                AuthorizeAsync<bool>(
                    context,
                    async (user, authorization) => await authorization.HasPasswordAsync(user),
                    OpenIdConnectScope.ManageUserApiScope
                )
            )
            .UseUserManager();
        descriptor
            .Field("roles")
            .Resolve(context =>
                AuthorizeAsync(
                    context,
                    async (user, authorization) => await authorization.GetRolesAsync(user),
                    Scopes.Roles
                )
            )
            .UseUserManager();
        descriptor
            .Field("rolesCurrentUserCanAdd")
            .ResolveWith<UserResolvers>(x =>
                UserResolvers.GetRolesCurrentUserCanAddOrRemoveAsync(default!, default!, default!))
            .UseUserManager();
        descriptor
            .Field("rolesCurrentUserCanRemove")
            .ResolveWith<UserResolvers>(x =>
                UserResolvers.GetRolesCurrentUserCanAddOrRemoveAsync(default!, default!, default!))
            .UseUserManager();
        descriptor
            .Field("isAuthorizedToDeleteUser")
            .ResolveWith<UserResolvers>(x => UserResolvers.IsAuthorizedToDeleteUserAsync(default!, default!, default!))
            .UseUserManager();
        descriptor
            .Field("isAuthorizedToManageOpenIdConnect")
            .ResolveWith<UserResolvers>(x => UserResolvers.IsAuthorizedToManageOpenIdConnect(default!, default!, default!))
            .UseUserManager();
        descriptor
            .Field("isAuthorizedToAddApprovals")
            .ResolveWith<UserResolvers>(x => UserResolvers.IsAuthorizedToAddApprovals(default!, default!, default!))
            .UseUserManager();
        descriptor
            .Field(t => t.DevelopedMethods)
            .Type<NonNullType<ObjectType<UserDevelopedMethodConnection>>>()
            .UseFiltering<UserDevelopedMethodFilterType>()
            .Resolve(context =>
                new UserDevelopedMethodConnection(
                    context.Parent<User>(),
                    context.GetQueryContext<UserMethodDeveloper>()
                )
            );
        descriptor
            .Field($"{GraphQlConstants.PendingPrefix}{nameof(User.DevelopedMethods)}")
            .Type<ObjectType<PendingUserDevelopedMethodConnection>>()
            .Authorize(AuthorizationPolicies.WriteScopePolicy)
            .UseFiltering<UserDevelopedMethodFilterType>()
            .Resolve(context =>
                new PendingUserDevelopedMethodConnection(
                    context.Parent<User>(),
                    context.GetQueryContext<UserMethodDeveloper>()
                )
            );
        descriptor
            .Field(t => t.RepresentedInstitutions)
            .Type<NonNullType<ObjectType<UserRepresentedInstitutionConnection>>>()
            .UseFiltering<UserRepresentedInstitutionFilterType>()
            .Resolve(context =>
                new UserRepresentedInstitutionConnection(
                    context.Parent<User>(),
                    context.GetQueryContext<InstitutionRepresentative>()
                )
            );
        descriptor
            .Field($"{GraphQlConstants.PendingPrefix}{nameof(User.RepresentedInstitutions)}")
            .Type<ObjectType<PendingUserRepresentedInstitutionConnection>>()
            .Authorize(AuthorizationPolicies.WriteScopePolicy)
            .UseFiltering<UserRepresentedInstitutionFilterType>()
            .Resolve(context =>
                new PendingUserRepresentedInstitutionConnection(
                    context.Parent<User>(),
                    context.GetQueryContext<InstitutionRepresentative>()
                )
            );
        descriptor
            .Field(t => t.GnuPgKeyFingerprints)
            .Type<NonNullType<ObjectType<UserGnuPgKeyFingerprintConnection>>>()
            .UseFiltering<UserGnuPgKeyFingerprintFilterType>()
            .Resolve(context =>
                new UserGnuPgKeyFingerprintConnection(
                    context.Parent<User>(),
                    context.GetQueryContext<GnuPgKeyFingerprint>()
                )
            );
        descriptor
            .Field("has" + nameof(GnuPgKeyFingerprint))
            .UseFiltering<UserGnuPgKeyFingerprintFilterType>()
            .ResolveWith<UserResolvers>(x =>
                UserResolvers.HasGnuPgKeyFingerprintsAsync(default!, default!, default!, default!));
    }

    private sealed class UserResolvers
    {
        public static Task<bool> HasGnuPgKeyFingerprintsAsync(
            [Parent] User user,
            ApplicationDbContext context,
            IResolverContext resolverContext,
            CancellationToken cancellationToken
        )
        {
            return context.GnuPgKeyFingerprints.AsNoTracking()
                .Filter(resolverContext)
                .Where(f => f.UserId == user.Id)
                .AnyAsync(cancellationToken);
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/main/src/Scaffolding/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Manage/Account.Manage.TwoFactorAuthentication.cs.cshtml
        public static async Task<TwoFactorAuthentication?> GetTwoFactorAuthenticationAsync(
            [Parent] User user,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ClaimsPrincipal claimsPrincipal,
            UserAuthorization authorization,
            CancellationToken cancellationToken
        )
        {
            if (!claimsPrincipal.HasScope(OpenIdConnectScope.ManageUserApiScope))
            {
                return null;
            }

            if (!await authorization.IsAuthorizedToManageUser(claimsPrincipal, user.Id, cancellationToken))
            {
                return null;
            }

            return new TwoFactorAuthentication(
                await userManager.GetAuthenticatorKeyAsync(user) is not null,
                await userManager.GetTwoFactorEnabledAsync(user),
                await signInManager.IsTwoFactorClientRememberedAsync(user),
                await userManager.CountRecoveryCodesAsync(user)
            );
        }

        public static Task<bool> IsAuthorizedToManageOpenIdConnect(
            ClaimsPrincipal claimsPrincipal,
            Authorization.OpenIdConnectAuthorization authorization,
            CancellationToken cancellationToken
        )
        {
            return authorization.IsAuthorizedToManageOpenIdConnect(claimsPrincipal, cancellationToken);
        }

        public static Task<bool> IsAuthorizedToAddApprovals(
            ClaimsPrincipal claimsPrincipal,
            ApprovalAuthorization authorization,
            CancellationToken cancellationToken
        )
        {
            return authorization.IsAuthorizedToAddApprovals(claimsPrincipal, cancellationToken);
        }

        public static Task<bool> IsAuthorizedToDeleteUserAsync(
            ClaimsPrincipal claimsPrincipal,
            UserAuthorization authorization,
            CancellationToken cancellationToken
        )
        {
            return authorization.IsAuthorizedToDeleteUsers(claimsPrincipal, cancellationToken);
        }

        public static async IAsyncEnumerable<UserRole> GetRolesCurrentUserCanAddOrRemoveAsync(
            ClaimsPrincipal claimsPrincipal,
            UserAuthorization authorization,
            [EnumeratorCancellation] CancellationToken cancellationToken
        )
        {
            foreach (var role in Role.AllEnum)
            {
                if (await authorization.IsAuthorizedToAddOrRemoveRole(claimsPrincipal, role, cancellationToken))
                {
                    yield return role;
                }
            }
        }
    }
}