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

namespace Metabase.GraphQl.Users;

public sealed class UserType
    : EntityType<User, IUserByIdDataLoader>
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
            .Cost(0)
            .Resolve(async context =>
                // Instead of returning `null`, we return a string because otherwise the
                // corresponding GraphQL field would need to be nullable and because the type `User`
                // implements `IStakeholder`, the stakeholder name would also need to be nullable.
                await Authorize(context, user => user.Name, Scopes.Profile)
                ?? context.Parent<User>().Name.Split(null as char[], StringSplitOptions.RemoveEmptyEntries).GetFirstOrDefault()
                ?? "<redacted>"
            )
            .UseUserManager();
        descriptor
            .Field("contact")
            .Type<NonNullType<ObjectType<ContactInformation>>>()
            .Cost(0)
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
            .Field("twoFactorAuthentication")
            .Cost(0)
            .ResolveWith<UserResolvers>(t =>
                UserResolvers.GetTwoFactorAuthenticationAsync(default!, default!, default!, default!, default!, default!))
            .UseUserManager()
            .UseSignInManager();
        descriptor
            .Field("hasPassword")
            .Type<BooleanType>()
            .Cost(0)
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
            .Cost(0)
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
            .Cost(1)
            .ResolveWith<UserResolvers>(x =>
                UserResolvers.GetRolesCurrentUserCanAddOrRemoveAsync(default!, default!, default!))
            .UseUserManager();
        descriptor
            .Field("rolesCurrentUserCanRemove")
            .Cost(1)
            .ResolveWith<UserResolvers>(x =>
                UserResolvers.GetRolesCurrentUserCanAddOrRemoveAsync(default!, default!, default!))
            .UseUserManager();
        descriptor
            .Field("isAuthorizedToDeleteUser")
            .Cost(1)
            .ResolveWith<UserResolvers>(x => UserResolvers.IsAuthorizedToDeleteUserAsync(default!, default!, default!))
            .UseUserManager();
        descriptor
            .Field("isAuthorizedToManageOpenIdConnect")
            .Cost(1)
            .ResolveWith<UserResolvers>(x => UserResolvers.IsAuthorizedToManageOpenIdConnect(default!, default!, default!))
            .UseUserManager();
        descriptor
            .Field("isAuthorizedToAddApprovals")
            .Cost(1)
            .ResolveWith<UserResolvers>(x => UserResolvers.IsAuthorizedToAddApprovals(default!, default!, default!))
            .UseUserManager();
        descriptor
            .Field(t => t.DevelopedMethods)
            .Type<NonNullType<ObjectType<UserDevelopedMethodConnection>>>()
            .AddPagingArguments()
            .UseFiltering<UserDevelopedMethodFilterType>()
            .UseSorting<UserDevelopedMethodSortType>()
            .Resolve(context =>
                new UserDevelopedMethodConnection(
                    context.Parent<User>(),
                    context.GetPagingArguments(),
                    context.GetQueryContext<UserMethodDeveloper>()
                )
            );
        descriptor
            .Field($"{GraphQlConstants.PendingPrefix}{nameof(User.DevelopedMethods)}")
            .Type<ObjectType<PendingUserDevelopedMethodConnection>>()
            .Authorize(AuthorizationPolicies.WriteScopePolicy)
            .AddPagingArguments()
            .UseFiltering<UserDevelopedMethodFilterType>()
            .UseSorting<UserDevelopedMethodSortType>()
            .Resolve(context =>
                new PendingUserDevelopedMethodConnection(
                    context.Parent<User>(),
                    context.GetPagingArguments(),
                    context.GetQueryContext<UserMethodDeveloper>()
                )
            );
        descriptor
            .Field(t => t.RepresentedInstitutions)
            .Type<NonNullType<ObjectType<UserRepresentedInstitutionConnection>>>()
            .UseFiltering<UserRepresentedInstitutionFilterType>()
            .UseSorting<UserRepresentedInstitutionSortType>()
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
            .UseSorting<UserRepresentedInstitutionSortType>()
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
            .UseSorting<UserGnuPgKeyFingerprintSortType>()
            .Resolve(context =>
                new UserGnuPgKeyFingerprintConnection(
                    context.Parent<User>(),
                    context.GetQueryContext<GnuPgKeyFingerprint>()
                )
            );
        descriptor
            .Field("has" + nameof(GnuPgKeyFingerprint))
            .UseFiltering<UserGnuPgKeyFingerprintFilterType>()
            .UseSorting<UserGnuPgKeyFingerprintSortType>()
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

        public static async IAsyncEnumerable<Metabase.Enumerations.UserRole> GetRolesCurrentUserCanAddOrRemoveAsync(
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