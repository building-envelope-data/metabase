using System.Threading;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using HotChocolate.AspNetCore.Authorization;
using NpgsqlTypes;
using DateTime = System.DateTime;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Users
{
    // TODO Mutations in https://github.com/dotnet/Scaffolding/tree/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account

    [ExtendObjectType(Name = nameof(GraphQl.Mutation))]
    public sealed class UserMutations
    {
        // Inspired by https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.Register.cs.cshtml
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public async Task<RegisterUserPayload> RegisterUserAsync(
            RegisterUserInput input,
            [ScopedService] UserManager<Data.User> userManager,
            CancellationToken cancellationToken
            )
        {
          var user = new Data.User {
            UserName = input.Email,
            Email = input.Email
          };
          if (input.Password != input.PasswordConfirmation)
          {
            return new RegisterUserPayload(
                new [] {
                  new RegisterUserError(
                      RegisterUserErrorCode.PASSWORD_CONFIRMATION_MISMATCH,
                      "The password and confirmation password do not match.",
                      new [] { "input", "passwordConfirmation" }
                      ),
                  }
                  );
          }
          var identityResult =
            await userManager.CreateAsync(
              user,
              input.Password
              );

          if (!identityResult.Succeeded)
          {
            var errors = new List<RegisterUserError>();
            foreach (var error in identityResult.Errors)
            {
              errors.Add(
                  // List of codes from https://github.com/aspnet/AspNetIdentity/blob/master/src/Microsoft.AspNet.Identity.Core/Resources.resx#L120
                  error.Code switch
                  {
                  "DuplicateEmail" or "DuplicateUserName" =>
                  new RegisterUserError(
                      RegisterUserErrorCode.DUPLICATE_EMAIL,
                      error.Description,
                      new [] { "input", "email" }
                      ),
                  "InvalidEmail" or "InvalidUserName" =>
                  new RegisterUserError(
                      RegisterUserErrorCode.INVALID_EMAIL,
                      error.Description,
                      new [] { "input", "email" }
                      ),
                  "PasswordRequiresDigit" =>
                  new RegisterUserError(
                      RegisterUserErrorCode.PASSWORD_REQUIRES_DIGIT,
                      error.Description,
                      new [] { "input", "password" }
                      ),
                  "PasswordRequiresLower" =>
                    new RegisterUserError(
                        RegisterUserErrorCode.PASSWORD_REQUIRES_LOWER,
                        error.Description,
                        new [] { "input", "password" }
                        ),
                  "PasswordRequiresNonAlphanumeric" =>
                    new RegisterUserError(
                        RegisterUserErrorCode.PASSWORD_REQUIRES_NON_ALPHANUMERIC,
                        error.Description,
                        new [] { "input", "password" }
                        ),
                  "PasswordRequiresUpper" =>
                    new RegisterUserError(
                        RegisterUserErrorCode.PASSWORD_REQUIRES_UPPER,
                        error.Description,
                        new [] { "input", "password" }
                        ),
                  "PasswordTooShort" =>
                    new RegisterUserError(
                        RegisterUserErrorCode.PASSWORD_TOO_SHORT,
                        error.Description,
                        new [] { "input", "password" }
                        ),
                  "PropertyTooShort" =>
                    new RegisterUserError(
                        RegisterUserErrorCode.NULL_OR_EMPTY_EMAIL,
                        error.Description,
                        new [] { "input", "email" }
                        ),
                  _ =>
                    new RegisterUserError(
                        RegisterUserErrorCode.UNKNOWN,
                        $"{error.Description} (error code `{error.Code}`)",
                        new [] { "input" }
                        )
                  }
              );
            }
            return new RegisterUserPayload(errors);
          }
          var confirmationCode =
            WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(
                  await userManager.GenerateEmailConfirmationTokenAsync(user)
                  )
                );
          /* TODO await _emailSender.SendEmailAsync( */
          /*     input.Email, */
          /*     "Confirm your email", */
          /*     $"Please confirm your email address with the confirmation code {confirmationCode}."); */
          System.Console.WriteLine($"Confirmation Code: {confirmationCode}");
          return new RegisterUserPayload(user);
        }

        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public async Task<ConfirmUserEmailPayload> ConfirmUserEmailAsync(
            ConfirmUserEmailInput input,
            [ScopedService] UserManager<Data.User> userManager,
            CancellationToken cancellationToken
            )
        {
            var user = await userManager.FindByEmailAsync(input.Email);
            if (user is null)
            {
              return new ConfirmUserEmailPayload(
                  new ConfirmUserEmailError(
                    ConfirmUserEmailErrorCode.USER_NOT_FOUND,
                    $"Unable to load user with email address {input.Email}.",
                    new [] { "input", "email" }
                    )
                  );
            }
            var confirmationToken = Encoding.UTF8.GetString(
                WebEncoders.Base64UrlDecode(
                  input.ConfirmationCode
                  )
                );
            var identityResult = await userManager.ConfirmEmailAsync(user, confirmationToken);
            if (!identityResult.Succeeded)
            {
              var errors = new List<ConfirmUserEmailError>();
              foreach (var error in identityResult.Errors)
              {
                errors.Add(
                    // List of codes from https://github.com/aspnet/AspNetIdentity/blob/master/src/Microsoft.AspNet.Identity.Core/Resources.resx#L140
                    error.Code switch
                    {
                    "InvalidToken" =>
                    new ConfirmUserEmailError(
                        ConfirmUserEmailErrorCode.INVALID_CONFIRMATION_TOKEN,
                        $"Invalid confirmation token.",
                        new [] { "input", "confirmationToken" }
                        ),
                    _ =>
                    new ConfirmUserEmailError(
                        ConfirmUserEmailErrorCode.UNKNOWN,
                        error.Description,
                        new [] { "input" }
                        )
                    }
                    );
              }
              return new ConfirmUserEmailPayload(errors);
            }
            return new ConfirmUserEmailPayload(user);
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.Login.cs.cshtml
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        [UseSignInManager]
        public async Task<LoginUserPayload> LoginUserAsync(
            LoginUserInput input,
            [ScopedService] UserManager<Data.User> userManager,
            [ScopedService] SignInManager<Data.User> signInManager,
            CancellationToken cancellationToken
            )
        {
          var signInResult = await signInManager.PasswordSignInAsync(
              input.Email,
              input.Password,
              isPersistent: false,
              lockoutOnFailure: true
              );
          // https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.signinresult?view=aspnetcore-5.0
          if (signInResult.IsLockedOut)
          {
            return new LoginUserPayload(
                new LoginUserError(
                  LoginUserErrorCode.LOCKED_OUT,
                  "User is locked out.",
                  new [] { "input" }
                  )
                );
          }
          if (signInResult.IsNotAllowed)
          {
            return new LoginUserPayload(
                new LoginUserError(
                  LoginUserErrorCode.NOT_ALLOWED,
                  "User is not allowed to login.",
                  new [] { "input" }
                  )
                );
          }
          if (signInResult.RequiresTwoFactor)
          {
            return new LoginUserPayload(
                new LoginUserError(
                  LoginUserErrorCode.REQUIRES_TWO_FACTOR,
                  "User requires two-factor authentication to login.",
                  new [] { "input" }
                  )
                );
          }
          if (!signInResult.Succeeded)
          {
            return new LoginUserPayload(
                new LoginUserError(
                  LoginUserErrorCode.INVALID,
                  "Invalid login attempt.",
                  new [] { "input" }
                  )
                );
          }
          // TODO Only load the user if requested. Use resolver in payload and just pass email address.
          var user = await userManager.FindByEmailAsync(input.Email);
          return new LoginUserPayload("TODO Generate JWT Access Token", user);
        }
    }
}
