using System.Collections.Generic;
using HotChocolate;
using HotChocolate.Types;
using Metabase.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using InvalidOperationException = System.InvalidOperationException;
using IServiceProvider = System.IServiceProvider;

namespace Metabase.GraphQl.Users;

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
        var userManagerServiceName = GetServiceName<UserManager<User>>();
        return descriptor.Use(next => async context =>
        {
            var services = context.Service<IServiceProvider>();
            var userManager = new UserManager<User>(
                new ApplicationUserStore(services.GetRequiredService<ApplicationDbContext>()),
                services.GetRequiredService<IOptions<IdentityOptions>>(),
                services.GetRequiredService<IPasswordHasher<User>>(),
                /* new PasswordHasher<Data.User>( */
                /*   services.GetRequiredService<IOptions<PasswordHasherOptions>>() */
                /*   ), */
                services.GetRequiredService<IEnumerable<IUserValidator<User>>>(),
                services.GetRequiredService<IEnumerable<IPasswordValidator<User>>>(),
                services.GetRequiredService<ILookupNormalizer>(),
                /* new UpperInvariantLookupNormalizer(), */
                services.GetRequiredService<IdentityErrorDescriber>(),
                /* new IdentityErrorDescriber(), */
                services,
                services.GetRequiredService<ILogger<UserManager<User>>>()
            );
            try
            {
                context.SetLocalState(userManagerServiceName, userManager);
                await next(context).ConfigureAwait(false);
            }
            finally
            {
                context.RemoveLocalState(userManagerServiceName);
                userManager.Dispose();
            }
        });
    }

    public static IObjectFieldDescriptor UseSignInManager(
        this IObjectFieldDescriptor descriptor
    )
    {
        // Inspired by https://github.com/ChilliCream/hotchocolate/blob/main/src/HotChocolate/Core/src/Types/Types/Descriptors/Extensions/ScopedServiceObjectFieldDescriptorExtensions.cs
        var signInManagerServiceName = GetServiceName<SignInManager<User>>();
        return descriptor.Use(next => async context =>
        {
            var services = context.Service<IServiceProvider>();
            var signInManager = new SignInManager<User>(
                context.GetLocalStateOrDefault<UserManager<User>>(GetServiceName<UserManager<User>>())
                ?? throw new InvalidOperationException(
                    "Add attribute `[UseUserManager]` before attribute `[UseSignInManager]`."),
                services.GetRequiredService<IHttpContextAccessor>(),
                services.GetRequiredService<IUserClaimsPrincipalFactory<User>>(),
                services.GetRequiredService<IOptions<IdentityOptions>>(),
                services.GetRequiredService<ILogger<SignInManager<User>>>(),
                services.GetRequiredService<IAuthenticationSchemeProvider>(),
                services.GetRequiredService<IUserConfirmation<User>>()
            );
            try
            {
                context.SetLocalState(signInManagerServiceName, signInManager);
                await next(context).ConfigureAwait(false);
            }
            finally
            {
                context.RemoveLocalState(signInManagerServiceName);
            }
        });
    }
}