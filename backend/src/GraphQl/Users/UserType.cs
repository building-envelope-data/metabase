using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Configuration;
using Metabase.Extensions;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Metabase.GraphQl.Users
{
    public sealed class UserType
        : EntityType<Data.User, UserByIdDataLoader>
    {
        private static string GetServiceName<TService>()
        {
            return typeof(TService).FullName ?? typeof(TService).Name;
        }

        private static async Task<T?> Authorize<T>(
            IResolverContext context,
            Func<Data.User, T?> getValue,
            string? scope = null
        )
            where T : class
        {
            var claimsPrincipal =
                context.GetGlobalStateOrDefault<ClaimsPrincipal>(nameof(ClaimsPrincipal))
                ?? throw new ArgumentException("Claims principal must not be null.");
            if (scope is not null && !claimsPrincipal.HasScope(scope))
            {
                return null;
            }

            var user = context.Parent<Data.User>();
            var userManager =
                context.GetLocalStateOrDefault<UserManager<Data.User>>(GetServiceName<UserManager<Data.User>>())
                ?? throw new ArgumentException("User manager must not be null.");
            if (!await UserAuthorization.IsAuthorizedToManageUser(
                    claimsPrincipal,
                    user.Id,
                    userManager
                ).ConfigureAwait(false))
            {
                return null;
            }

            return getValue(user);
        }

        private static async Task<T?> Authorize<T>(
            IResolverContext context,
            Func<Data.User, T?> getValue,
            string? scope = null
        )
            where T : struct
        {
            var claimsPrincipal =
                context.GetGlobalStateOrDefault<ClaimsPrincipal>(nameof(ClaimsPrincipal))
                ?? throw new ArgumentException("Claims principal must not be null.");
            if (scope is not null && !claimsPrincipal.HasScope(scope))
            {
                return null;
            }

            var user = context.Parent<Data.User>();
            var userManager =
                context.GetLocalStateOrDefault<UserManager<Data.User>>(GetServiceName<UserManager<Data.User>>())
                ?? throw new ArgumentException("User manager must not be null.");
            if (!await UserAuthorization.IsAuthorizedToManageUser(
                    claimsPrincipal,
                    user.Id,
                    userManager
                ).ConfigureAwait(false))
            {
                return null;
            }

            return getValue(user);
        }

        private static async Task<T?> AuthorizeAsync<T>(
            IResolverContext context,
            Func<Data.User, UserManager<Data.User>, Task<T?>> getValue,
            string? scope = null
        )
            where T : class
        {
            var claimsPrincipal =
                context.GetGlobalStateOrDefault<ClaimsPrincipal>(nameof(ClaimsPrincipal))
                ?? throw new ArgumentException("Claims principal must not be null.");
            if (scope is not null && !claimsPrincipal.HasScope(scope))
            {
                return null;
            }

            var user = context.Parent<Data.User>();
            var userManager =
                context.GetLocalStateOrDefault<UserManager<Data.User>>(GetServiceName<UserManager<Data.User>>())
                ?? throw new ArgumentException("User manager must not be null.");
            if (!await UserAuthorization.IsAuthorizedToManageUser(
                    claimsPrincipal,
                    user.Id,
                    userManager
                ).ConfigureAwait(false))
            {
                return null;
            }

            return await getValue(user, userManager).ConfigureAwait(false);
        }

        private static async Task<T?> AuthorizeAsync<T>(
            IResolverContext context,
            Func<Data.User, UserManager<Data.User>, Task<T?>> getValue,
            string? scope = null
        )
            where T : struct
        {
            var claimsPrincipal =
                context.GetGlobalStateOrDefault<ClaimsPrincipal>(nameof(ClaimsPrincipal))
                ?? throw new ArgumentException("Claims principal must not be null.");
            if (scope is not null && !claimsPrincipal.HasScope(scope))
            {
                return null;
            }

            var user = context.Parent<Data.User>();
            var userManager =
                context.GetLocalStateOrDefault<UserManager<Data.User>>(GetServiceName<UserManager<Data.User>>())
                ?? throw new ArgumentException("User manager must not be null.");
            if (!await UserAuthorization.IsAuthorizedToManageUser(
                    claimsPrincipal,
                    user.Id,
                    userManager
                ).ConfigureAwait(false))
            {
                return null;
            }

            return await getValue(user, userManager).ConfigureAwait(false);
        }

        protected override void Configure(
            IObjectTypeDescriptor<Data.User> descriptor
        )
        {
            descriptor.BindFieldsExplicitly();
            base.Configure(descriptor);
            // Keep authorization scopes in sync with `UserinfoController`.
            descriptor
                .Field(t => t.Name)
                // .Type<NonNullType<StringType>>()
                .Resolve(async context =>
                    // Instead of returning `null`, we return a string because
                    // otherwise the corresponding GraphQL field would need to be
                    // nullable and because the type `User` implements
                    // `IStakeholder`, the stakeholder name would also need to be
                    // nullable.
                    await Authorize(context, user => user.Name, Scopes.Profile) ??
                    "<redacted>"
                )
                .UseUserManager();
            descriptor
                .Field(t => t.Email)
                .Resolve(context =>
                    Authorize(context, user => user.Email, Scopes.Email)
                )
                .UseUserManager();
            descriptor
                .Field(t => t.EmailConfirmed)
                .Name("isEmailConfirmed")
                .Type<BooleanType>()
                .Resolve(context =>
                    Authorize<bool>(context, user => user.EmailConfirmed, Scopes.Email)
                )
                .UseUserManager();
            descriptor
                .Field(t => t.PostalAddress)
                .Resolve(context =>
                    Authorize(context, user => user.PostalAddress, Scopes.Address)
                )
                .UseUserManager();
            descriptor
                .Field(t => t.PhoneNumber)
                .Resolve(context =>
                    Authorize(context, user => user.PhoneNumber, Scopes.Phone)
                )
                .UseUserManager();
            descriptor
                .Field(t => t.PhoneNumberConfirmed)
                .Name("isPhoneNumberConfirmed")
                .Type<BooleanType>()
                .Resolve(context =>
                    Authorize<bool>(context, user => user.PhoneNumberConfirmed, Scopes.Phone)
                )
                .UseUserManager();
            descriptor
                .Field(t => t.WebsiteLocator)
                .Resolve(context =>
                    Authorize(context, user => user.WebsiteLocator, Scopes.Profile)
                )
                .UseUserManager();
            descriptor
                .Field("twoFactorAuthentication")
                .ResolveWith<UserResolvers>(t =>
                    UserResolvers.GetTwoFactorAuthenticationAsync(default!, default!, default!, default!))
                .UseUserManager()
                .UseSignInManager();
            descriptor
                .Field("hasPassword")
                .Type<BooleanType>()
                .Resolve(context =>
                    AuthorizeAsync<bool>(
                        context,
                        async (user, userManager) =>
                            await userManager.HasPasswordAsync(user).ConfigureAwait(false),
                        AuthConfiguration.ManageUserApiScope
                    )
                )
                .UseUserManager();
            descriptor
                .Field("roles")
                .Resolve(context =>
                    AuthorizeAsync(
                        context,
                        async (user, userManager) =>
                            (await userManager.GetRolesAsync(user).ConfigureAwait(false))
                            .Select(Data.Role.EnumFromName),
                        Scopes.Roles
                    )
                )
                .UseUserManager();
            descriptor
                .Field("rolesCurrentUserCanAdd")
                .ResolveWith<UserResolvers>(x =>
                    UserResolvers.GetRolesCurrentUserCanAddAsync(default!, default!, default!))
                .UseUserManager();
            descriptor
                .Field("rolesCurrentUserCanRemove")
                .ResolveWith<UserResolvers>(x =>
                    UserResolvers.GetRolesCurrentUserCanRemoveAsync(default!, default!, default!))
                .UseUserManager();
            descriptor
                .Field("canCurrentUserDeleteUser")
                .ResolveWith<UserResolvers>(x => UserResolvers.GetCanCurrentUserDeleteUserAsync(default!, default!))
                .UseUserManager();
            descriptor
                .Field(t => t.DevelopedMethods)
                .Argument(nameof(Data.UserMethodDeveloper.Pending).FirstCharToLower(),
                    _ => _.Type<NonNullType<BooleanType>>().DefaultValue(false))
                .Type<NonNullType<ObjectType<UserDevelopedMethodConnection>>>()
                .Resolve(context =>
                    new UserDevelopedMethodConnection(
                        context.Parent<Data.User>(),
                        context.ArgumentValue<bool>(nameof(Data.UserMethodDeveloper.Pending).FirstCharToLower())
                    )
                );
            descriptor
                .Field(t => t.RepresentedInstitutions)
                .Argument(nameof(Data.InstitutionRepresentative.Pending).FirstCharToLower(),
                    _ => _.Type<NonNullType<BooleanType>>().DefaultValue(false))
                .Type<NonNullType<ObjectType<UserRepresentedInstitutionConnection>>>()
                .Resolve(context =>
                    new UserRepresentedInstitutionConnection(
                        context.Parent<Data.User>(),
                        context.ArgumentValue<bool>(nameof(Data.InstitutionRepresentative.Pending).FirstCharToLower())
                    )
                );
        }

        private sealed class UserResolvers
        {
            // Inspired by https://github.com/dotnet/Scaffolding/blob/main/src/Scaffolding/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Manage/Account.Manage.TwoFactorAuthentication.cs.cshtml
            public static async Task<TwoFactorAuthentication?> GetTwoFactorAuthenticationAsync(
                [Parent] Data.User user,
                ClaimsPrincipal claimsPrincipal,
                [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
                [Service(ServiceKind.Resolver)] SignInManager<Data.User> signInManager
            )
            {
                if (!claimsPrincipal.HasScope(AuthConfiguration.ManageUserApiScope))
                {
                    return null;
                }

                if (!await UserAuthorization.IsAuthorizedToManageUser(
                        claimsPrincipal,
                        user.Id,
                        userManager
                    ).ConfigureAwait(false))
                {
                    return null;
                }

                return new TwoFactorAuthentication(
                    hasAuthenticator: await userManager.GetAuthenticatorKeyAsync(user).ConfigureAwait(false) != null,
                    isEnabled: await userManager.GetTwoFactorEnabledAsync(user).ConfigureAwait(false),
                    isMachineRemembered: await signInManager.IsTwoFactorClientRememberedAsync(user)
                        .ConfigureAwait(false),
                    recoveryCodesLeftCount: await userManager.CountRecoveryCodesAsync(user).ConfigureAwait(false)
                );
            }

            public static Task<bool> GetCanCurrentUserDeleteUserAsync(
                ClaimsPrincipal claimsPrincipal,
                [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager
            )
            {
                return UserAuthorization.IsAuthorizedToDeleteUsers(claimsPrincipal, userManager);
            }

            public static async Task<IList<Enumerations.UserRole>> GetRolesCurrentUserCanAddAsync(
                ClaimsPrincipal claimsPrincipal,
                [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
                CancellationToken cancellationToken
            )
            {
                return await GetRolesCurrentUserCanAddOrRemoveAsync(claimsPrincipal, userManager)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

            public static async Task<IList<Enumerations.UserRole>> GetRolesCurrentUserCanRemoveAsync(
                ClaimsPrincipal claimsPrincipal,
                [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
                CancellationToken cancellationToken
            )
            {
                return await GetRolesCurrentUserCanAddOrRemoveAsync(claimsPrincipal, userManager)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

            private static async IAsyncEnumerable<Enumerations.UserRole> GetRolesCurrentUserCanAddOrRemoveAsync(
                ClaimsPrincipal claimsPrincipal,
                [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager
            )
            {
                foreach (var role in Data.Role.AllEnum)
                {
                    if (await UserAuthorization.IsAuthorizedToAddOrRemoveRole(
                            claimsPrincipal,
                            role,
                            userManager
                        ).ConfigureAwait(false))
                    {
                        yield return role;
                    }
                }
            }
        }
    }
}