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
using Metabase.Extensions;
using Microsoft.AspNetCore.Identity;

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
          Func<Data.User, T?> getValue
          )
          where T : class
        {
            var user = context.Parent<Data.User>();
            var claimsPrincipal =
              context.GetGlobalValue<ClaimsPrincipal>(nameof(ClaimsPrincipal))
              ?? throw new Exception("Claims principal must not be null.");
            var userManager =
              context.GetLocalValue<UserManager<Data.User>>(GetServiceName<UserManager<Data.User>>())
              ?? throw new Exception("User manager must not be null.");
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
          Func<Data.User, T?> getValue
          )
          where T : struct
        {
            var user = context.Parent<Data.User>();
            var claimsPrincipal =
              context.GetGlobalValue<ClaimsPrincipal>(nameof(ClaimsPrincipal))
              ?? throw new Exception("Claims principal must not be null.");
            var userManager =
              context.GetLocalValue<UserManager<Data.User>>(GetServiceName<UserManager<Data.User>>())
              ?? throw new Exception("User manager must not be null.");
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
          Func<Data.User, UserManager<Data.User>, Task<T?>> getValue
          )
          where T : class
        {
            var user = context.Parent<Data.User>();
            var claimsPrincipal =
              context.GetGlobalValue<ClaimsPrincipal>(nameof(ClaimsPrincipal))
              ?? throw new Exception("Claims principal must not be null.");
            var userManager =
              context.GetLocalValue<UserManager<Data.User>>(GetServiceName<UserManager<Data.User>>())
              ?? throw new Exception("User manager must not be null.");
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
          Func<Data.User, UserManager<Data.User>, Task<T?>> getValue
          )
          where T : struct
        {
            var user = context.Parent<Data.User>();
            var claimsPrincipal =
              context.GetGlobalValue<ClaimsPrincipal>(nameof(ClaimsPrincipal))
              ?? throw new Exception("Claims principal must not be null.");
            var userManager =
              context.GetLocalValue<UserManager<Data.User>>(GetServiceName<UserManager<Data.User>>())
              ?? throw new Exception("User manager must not be null.");
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
            descriptor
              .Field(t => t.Name);
            descriptor
              .Field(t => t.Email)
              .Resolve(context =>
                Authorize(context, user => user.Email)
              )
              .UseDbContext<Data.ApplicationDbContext>()
              .UseUserManager();
            descriptor
              .Field(t => t.EmailConfirmed)
              .Name("isEmailConfirmed")
              .Type<BooleanType>()
              .Resolve(context =>
                Authorize<bool>(context, user => user.EmailConfirmed)
              )
              .UseDbContext<Data.ApplicationDbContext>()
              .UseUserManager();
            descriptor
              .Field(t => t.PhoneNumber)
              .Resolve(context =>
                Authorize(context, user => user.PhoneNumber)
              )
              .UseDbContext<Data.ApplicationDbContext>()
              .UseUserManager();
            descriptor
              .Field(t => t.PhoneNumberConfirmed)
              .Name("isPhoneNumberConfirmed")
              .Type<BooleanType>()
              .Resolve(context =>
                Authorize<bool>(context, user => user.PhoneNumberConfirmed)
              )
              .UseDbContext<Data.ApplicationDbContext>()
              .UseUserManager();
            descriptor
              .Field(t => t.WebsiteLocator);
            descriptor
              .Field("twoFactorAuthentication")
              .ResolveWith<UserResolvers>(t => t.GetTwoFactorAuthenticationAsync(default!, default!, default!, default!))
              .UseDbContext<Data.ApplicationDbContext>()
              .UseUserManager()
              .UseSignInManager();
            descriptor
              .Field("hasPassword")
              .Type<BooleanType>()
              .Resolve(context =>
                AuthorizeAsync<bool>(context, async (user, userManager) =>
                  await userManager.HasPasswordAsync(user).ConfigureAwait(false)
                  )
              )
              .UseDbContext<Data.ApplicationDbContext>()
              .UseUserManager();
            descriptor
              .Field("roles")
              .ResolveWith<UserResolvers>(x => x.GetRolesAsync(default!, default!))
              .UseDbContext<Data.ApplicationDbContext>()
              .UseUserManager();
            descriptor
              .Field("rolesCurrentUserCanAdd")
              .ResolveWith<UserResolvers>(x => x.GetRolesCurrentUserCanAddAsync(default!, default!, default!))
              .UseDbContext<Data.ApplicationDbContext>()
              .UseUserManager();
            descriptor
              .Field("rolesCurrentUserCanRemove")
              .ResolveWith<UserResolvers>(x => x.GetRolesCurrentUserCanRemoveAsync(default!, default!, default!))
              .UseDbContext<Data.ApplicationDbContext>()
              .UseUserManager();
            descriptor
              .Field("canCurrentUserDeleteUser")
              .ResolveWith<UserResolvers>(x => x.GetCanCurrentUserDeleteUserAsync(default!, default!))
              .UseDbContext<Data.ApplicationDbContext>()
              .UseUserManager();
            descriptor
              .Field(t => t.DevelopedMethods)
              .Argument(nameof(Data.UserMethodDeveloper.Pending).FirstCharToLower(), _ => _.Type<NonNullType<BooleanType>>().DefaultValue(false))
              .Type<NonNullType<ObjectType<UserDevelopedMethodConnection>>>()
              .Resolve(context =>
                  new UserDevelopedMethodConnection(
                      context.Parent<Data.User>(),
                      context.ArgumentValue<bool>(nameof(Data.UserMethodDeveloper.Pending).FirstCharToLower())
                  )
              );
            descriptor
              .Field(t => t.RepresentedInstitutions)
              .Argument(nameof(Data.InstitutionRepresentative.Pending).FirstCharToLower(), _ => _.Type<NonNullType<BooleanType>>().DefaultValue(false))
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
            public async Task<TwoFactorAuthentication?> GetTwoFactorAuthenticationAsync(
              [Parent] Data.User user,
              [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
              [ScopedService] UserManager<Data.User> userManager,
              [ScopedService] SignInManager<Data.User> signInManager
            )
            {
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
                    isMachineRemembered: await signInManager.IsTwoFactorClientRememberedAsync(user).ConfigureAwait(false),
                    recoveryCodesLeftCount: await userManager.CountRecoveryCodesAsync(user).ConfigureAwait(false)
                    );
            }

            public Task<bool> GetCanCurrentUserDeleteUserAsync(
              [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
              [ScopedService] UserManager<Data.User> userManager
            )
            {
                return UserAuthorization.IsAuthorizedToDeleteUsers(claimsPrincipal, userManager);
            }

            public async Task<IEnumerable<Enumerations.UserRole>> GetRolesAsync(
              [Parent] Data.User user,
              [ScopedService] UserManager<Data.User> userManager
            )
            {
                return (await userManager.GetRolesAsync(user).ConfigureAwait(false))
                  .Select(Data.Role.EnumFromName);
            }

            public async Task<IList<Enumerations.UserRole>> GetRolesCurrentUserCanAddAsync(
              [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
              [ScopedService] UserManager<Data.User> userManager,
              CancellationToken cancellationToken
            )
            {
                return await GetRolesCurrentUserCanAddOrRemoveAsync(claimsPrincipal, userManager)
                  .ToListAsync(cancellationToken)
                  .ConfigureAwait(false);
            }

            public async Task<IList<Enumerations.UserRole>> GetRolesCurrentUserCanRemoveAsync(
              [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
              [ScopedService] UserManager<Data.User> userManager,
              CancellationToken cancellationToken
            )
            {
                return await GetRolesCurrentUserCanAddOrRemoveAsync(claimsPrincipal, userManager)
                  .ToListAsync(cancellationToken)
                  .ConfigureAwait(false);
            }

            private async IAsyncEnumerable<Enumerations.UserRole> GetRolesCurrentUserCanAddOrRemoveAsync(
              [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
              [ScopedService] UserManager<Data.User> userManager
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