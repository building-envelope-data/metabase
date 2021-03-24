using System.Security.Claims;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Metabase.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Users
{
    public sealed class UserType
      : EntityType<Data.User, UserByIdDataLoader>
    {
        protected override void Configure(
            IObjectTypeDescriptor<Data.User> descriptor
            )
        {
            descriptor.BindFieldsExplicitly();
            // TODO Protect fields like `Email` and `PhoneNumber` from being publicly available!
            base.Configure(descriptor);
            descriptor
              .Field(t => t.Email);
            descriptor
              .Field(t => t.EmailConfirmed);
            descriptor
              .Field(t => t.Name);
            descriptor
              .Field(t => t.PhoneNumber);
            descriptor
              .Field(t => t.WebsiteLocator);
            descriptor
              .Field(t => t.DevelopedMethods);
            descriptor
              .Field(t => t.DevelopedMethods)
              .Type<NonNullType<ObjectType<UserDevelopedMethodConnection>>>()
              .Resolve(context =>
                  new UserDevelopedMethodConnection(
                      context.Parent<Data.User>()
                  )
              );
            descriptor
              .Field(t => t.RepresentedInstitutions)
              .Type<NonNullType<ObjectType<UserRepresentedInstitutionConnection>>>()
              .Resolve(context =>
                  new UserRepresentedInstitutionConnection(
                      context.Parent<Data.User>()
                  )
              );
            descriptor
              .Field("twoFactorAuthentication")
              .ResolveWith<UserResolvers>(t => t.GetTwoFactorAuthenticationAsync(default!, default!, default!, default!))
              .UseDbContext<Data.ApplicationDbContext>()
              .UseUserManager()
              .UseSignInManager();
            descriptor
              .Field("hasPassword")
              .ResolveWith<UserResolvers>(t => t.GetHasPasswordAsync(default!, default!, default!))
              .UseDbContext<Data.ApplicationDbContext>()
              .UseUserManager();
        }

        private sealed class UserResolvers
        {
            // Inspired by https://github.com/dotnet/Scaffolding/blob/main/src/Scaffolding/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Manage/Account.Manage.TwoFactorAuthentication.cs.cshtml
            public async Task<TwoFactorAuthentication?> GetTwoFactorAuthenticationAsync(
              Data.User user,
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

            public async Task<bool?> GetHasPasswordAsync(
              Data.User user,
              [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
              [ScopedService] UserManager<Data.User> userManager
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
              return await userManager.HasPasswordAsync(user).ConfigureAwait(false);
            }
        }
    }
}