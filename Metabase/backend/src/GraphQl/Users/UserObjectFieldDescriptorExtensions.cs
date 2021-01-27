using System.Collections.Generic;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Guid = System.Guid;
using InvalidOperationException = System.InvalidOperationException;
using IServiceProvider = System.IServiceProvider;

namespace Metabase.GraphQl.Users
{
    public static class UserObjectFieldDescriptorExtensions
    {
        private static string GetServiceName<TService>()
        {
            return typeof(TService).FullName ?? typeof(TService).Name;
        }

        public static IObjectFieldDescriptor UseUserManager(
            this IObjectFieldDescriptor descriptor
            )
        {
            // Inspired by https://github.com/ChilliCream/hotchocolate/blob/main/src/HotChocolate/Core/src/Types/Types/Descriptors/Extensions/ScopedServiceObjectFieldDescriptorExtensions.cs
            string userManagerServiceName = GetServiceName<UserManager<Data.User>>();
            return descriptor.Use(next => async context =>
            {
                var services = context.Service<IServiceProvider>();
                var userManager = new UserManager<Data.User>(
                    new UserStore<Data.User, Data.Role, Data.ApplicationDbContext, Guid, Data.UserClaim, Data.UserRole, Data.UserLogin, Data.UserToken, Data.RoleClaim>(
                      context.GetLocalValue<Data.ApplicationDbContext>(GetServiceName<Data.ApplicationDbContext>())
                      ?? throw new InvalidOperationException("Add attribute `[UseDbContext(typeof(Data.ApplicationDbContext))]` before attribute `[UseUserManager]`.")
                      ),
                    services.GetRequiredService<IOptions<IdentityOptions>>(),
                    services.GetRequiredService<IPasswordHasher<Data.User>>(),
                    /* new PasswordHasher<Data.User>( */
                    /*   services.GetRequiredService<IOptions<PasswordHasherOptions>>() */
                    /*   ), */
                    services.GetRequiredService<IEnumerable<IUserValidator<Data.User>>>(),
                    services.GetRequiredService<IEnumerable<IPasswordValidator<Data.User>>>(),
                    services.GetRequiredService<ILookupNormalizer>(),
                    /* new UpperInvariantLookupNormalizer(), */
                    services.GetRequiredService<IdentityErrorDescriber>(),
                    /* new IdentityErrorDescriber(), */
                    services,
                    services.GetRequiredService<ILogger<UserManager<Data.User>>>()
                    );
                try
                {
                    context.SetLocalValue(userManagerServiceName, userManager);
                    await next(context).ConfigureAwait(false);
                }
                finally
                {
                    context.RemoveLocalValue(userManagerServiceName);
                    userManager.Dispose();
                }
            });
        }

        public static IObjectFieldDescriptor UseSignInManager(
            this IObjectFieldDescriptor descriptor
            )
        {
            // Inspired by https://github.com/ChilliCream/hotchocolate/blob/main/src/HotChocolate/Core/src/Types/Types/Descriptors/Extensions/ScopedServiceObjectFieldDescriptorExtensions.cs
            string signInManagerServiceName = GetServiceName<SignInManager<Data.User>>();
            return descriptor.Use(next => async context =>
            {
                var services = context.Service<IServiceProvider>();
                var signInManager = new SignInManager<Data.User>(
                    context.GetLocalValue<UserManager<Data.User>>(GetServiceName<UserManager<Data.User>>())
                    ?? throw new InvalidOperationException("Add attribute `[UseUserManager]` before attribute `[UseSignInManager]`."),
                    services.GetRequiredService<IHttpContextAccessor>(),
                    services.GetRequiredService<IUserClaimsPrincipalFactory<Data.User>>(),
                    services.GetRequiredService<IOptions<IdentityOptions>>(),
                    services.GetRequiredService<ILogger<SignInManager<Data.User>>>(),
                    services.GetRequiredService<IAuthenticationSchemeProvider>(),
                    services.GetRequiredService<IUserConfirmation<Data.User>>()
                    );
                try
                {
                    context.SetLocalValue(signInManagerServiceName, signInManager);
                    await next(context).ConfigureAwait(false);
                }
                finally
                {
                    context.RemoveLocalValue(signInManagerServiceName);
                }
            });
        }
    }
}