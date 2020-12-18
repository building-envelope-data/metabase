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
        // Inspired by https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.ConfirmEmail.cs.cshtml
        // and https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.RegisterConfirmation.cs.cshtml
        // Despite its name, it is also used to confirm registrations. TODO Should we add another mutation for that?
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
                        error.Description,
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

        // Inspired by https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.ConfirmEmailChange.cs.cshtml
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        [UseSignInManager]
        public async Task<ConfirmUserEmailChangePayload> ConfirmUserEmailChangeAsync(
            ConfirmUserEmailChangeInput input,
            [ScopedService] UserManager<Data.User> userManager,
            [ScopedService] SignInManager<Data.User> signInManager,
            CancellationToken cancellationToken
            )
        {
            var user = await userManager.FindByEmailAsync(input.OldEmail);
            if (user is null)
            {
              return new ConfirmUserEmailChangePayload(
                  new ConfirmUserEmailChangeError(
                    ConfirmUserEmailChangeErrorCode.USER_NOT_FOUND,
                    $"Unable to load user with email address {input.OldEmail}.",
                    new [] { "input", "oldEmail" }
                    )
                  );
            }
            var confirmationToken =
              Encoding.UTF8.GetString(
                  WebEncoders.Base64UrlDecode(
                    input.ConfirmationCode
                    )
                );
            var changeEmailIdentityResult = await userManager.ChangeEmailAsync(user, input.NewEmail, confirmationToken);
            // For us email and user name are one and the same, so when we
            // update the email we need to update the user name.
            var setUserNameIdentityResult = await userManager.SetUserNameAsync(user, input.NewEmail);
            if (!(changeEmailIdentityResult.Succeeded && setUserNameIdentityResult.Succeeded))
            {
              var errors = new List<ConfirmUserEmailChangeError>();
              foreach (var error in changeEmailIdentityResult.Errors)
              {
                errors.Add(
                    // List of codes from https://github.com/aspnet/AspNetIdentity/blob/master/src/Microsoft.AspNet.Identity.Core/Resources.resx#L140
                    error.Code switch
                    {
                    "DuplicateEmail" =>
                    new ConfirmUserEmailChangeError(
                        ConfirmUserEmailChangeErrorCode.DUPLICATE_EMAIL,
                        error.Description,
                        new [] { "input", "newEmail" }
                        ),
                    "InvalidToken" =>
                    new ConfirmUserEmailChangeError(
                        ConfirmUserEmailChangeErrorCode.INVALID_CONFIRMATION_TOKEN,
                        $"Invalid confirmation token.",
                        new [] { "input", "confirmationToken" }
                        ),
                    _ =>
                    new ConfirmUserEmailChangeError(
                        ConfirmUserEmailChangeErrorCode.UNKNOWN,
                        error.Description,
                        new [] { "input" }
                        )
                    }
                    );
              }
              foreach (var error in setUserNameIdentityResult.Errors)
              {
                errors.Add(
                    // List of codes from https://github.com/aspnet/AspNetIdentity/blob/master/src/Microsoft.AspNet.Identity.Core/Resources.resx#L140
                    error.Code switch
                    {
                    "DuplicateUserName" =>
                    new ConfirmUserEmailChangeError(
                        ConfirmUserEmailChangeErrorCode.DUPLICATE_EMAIL,
                        error.Description,
                        new [] { "input", "newEmail" }
                        ),
                    _ =>
                    new ConfirmUserEmailChangeError(
                        ConfirmUserEmailChangeErrorCode.UNKNOWN,
                        error.Description,
                        new [] { "input" }
                        )
                    }
                    );
              }
              return new ConfirmUserEmailChangePayload(errors);
            }
            await signInManager.RefreshSignInAsync(user);
            return new ConfirmUserEmailChangePayload(user);
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
          if (!signInResult.Succeeded && !signInResult.RequiresTwoFactor)
          {
            return new LoginUserPayload(
                new LoginUserError(
                  LoginUserErrorCode.INVALID,
                  "Invalid login attempt.",
                  new [] { "input" }
                  )
                );
          }
          // TODO Only load the user if requested in the GraphQl query. Use resolver in payload and just pass email address.
          var user = await userManager.FindByEmailAsync(input.Email);
          if (signInResult.RequiresTwoFactor)
          {
            return new LoginUserPayload(user);
          }
          return new LoginUserPayload("TODO Generate JWT Access Token", user);
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.LoginWith2fa.cs.cshtml
        // TODO Using `SignInManager`'s two-factor authentication methods does not work because they rely on cookies, see https://github.com/aspnet/Identity/issues/1421 . We have to do manually what is done in https://github.com/dotnet/aspnetcore/blob/master/src/Identity/Core/src/SignInManager.cs#L504
        // TODO https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.LoginWithRecoveryCode.cs.cshtml
        /* [UseDbContext(typeof(Data.ApplicationDbContext))] */
        /* [UseUserManager] */
        /* [UseSignInManager] */
        /* public async Task<LoginUserWithTwoFactorPayload> LoginUserWithTwoFactorAsync( */
        /*     LoginUserWithTwoFactorInput input, */
        /*     [ScopedService] UserManager<Data.User> userManager, */
        /*     [ScopedService] SignInManager<Data.User> signInManager, */
        /*     CancellationToken cancellationToken */
        /*     ) */
        /* { */
        /*     var user = await signInManager.GetTwoFactorAuthenticationUserAsync(); */
        /*     if (user is null) */
        /*     { */
        /*       return new LoginUserWithTwoFactorPayload( */
        /*           new LoginUserWithTwoFactorError( */
        /*             LoginUserWithTwoFactorErrorCode.USER_NOT_FOUND, */
        /*             $"Unable to load two-factor authentication user with email address {input.OldEmail}.", */
        /*             new [] { "input", "oldEmail" } */
        /*             ) */
        /*           ); */
        /*     } */
        /*     if (user == null) */
        /*     { */
        /*         throw new InvalidOperationException($"Unable to load two-factor authentication user."); */
        /*     } */
        /*     var authenticatorCode = Input.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty); */
        /*     var result = await signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, Input.RememberMachine); */
        /*     if (result.Succeeded) */
        /*     { */
        /*         _logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id); */
        /*         return LocalRedirect(returnUrl); */
        /*     } */
        /*     else if (result.IsLockedOut) */
        /*     { */
        /*         _logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id); */
        /*         return RedirectToPage("./Lockout"); */
        /*     } */
        /*     else */
        /*     { */
        /*         _logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", user.Id); */
        /*         ModelState.AddModelError(string.Empty, "Invalid authenticator code."); */
        /*         return Page(); */
        /*     } */
        /* } */

        // Inspired by https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.Logout.cs.cshtml
        /* [UseDbContext(typeof(Data.ApplicationDbContext))] */
        /* [UseUserManager] */
        /* [UseSignInManager] */
        /* public async Task<LogOutUserPayload> LogoutUserAsync( */
        /*     LogoutUserInput input, */
        /*     [ScopedService] UserManager<Data.User> userManager, */
        /*     [ScopedService] SignInManager<Data.User> signInManager, */
        /*     CancellationToken cancellationToken */
        /*     ) */
        /* { */
        /*   // TODO Invalidate JWT token? */
        /* } */

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
                new RegisterUserError(
                  RegisterUserErrorCode.PASSWORD_CONFIRMATION_MISMATCH,
                  "The password and confirmation password do not match.",
                  new [] { "input", "passwordConfirmation" }
                  )
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
          // TODO The confirmation also confirms the registration/account. Should we use another email text then?
          await SendUserEmailConfirmation(user, userManager);
          return new RegisterUserPayload(user);
        }

        private async Task SendUserEmailConfirmation(
            Data.User user,
            UserManager<Data.User> userManager
            )
        {
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
        }

        /// Inspired by https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.ResendEmailConfirmation.cs.cshtml
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public async Task<ResendUserEmailConfirmationPayload> ResendUserEmailConfirmationAsync(
            ResendUserEmailConfirmationInput input,
            [ScopedService] UserManager<Data.User> userManager,
            CancellationToken cancellationToken
            )
        {
          var user = await userManager.FindByEmailAsync(input.Email);
          // Don't reveal that the user does not exist.
          if (user is not null)
          {
            await SendUserEmailConfirmation(user, userManager);
          }
          return new ResendUserEmailConfirmationPayload();
        }

        /// Inspired by https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.ForgotPassword.cs.cshtml
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public async Task<RequestUserPasswordResetPayload> RequestUserPasswordResetAsync(
            RequestUserPasswordResetInput input,
            [ScopedService] UserManager<Data.User> userManager,
            CancellationToken cancellationToken
            )
        {
          var user = await userManager.FindByEmailAsync(input.Email);
          // Don't reveal that the user does not exist or is not confirmed
          if (user is not null && await userManager.IsEmailConfirmedAsync(user))
          {
            // For more information on how to enable account confirmation and password reset please
            // visit https://docs.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm?view=aspnetcore-5.0
            var resetCode =
              WebEncoders.Base64UrlEncode(
                  Encoding.UTF8.GetBytes(
                    await userManager.GeneratePasswordResetTokenAsync(user)
                    )
                  );
            /* TODO await _emailSender.SendEmailAsync( */
            /*     input.Email, */
            /*     "Reset Password", */
            /*     $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."); */
            System.Console.WriteLine($"Reset Code: {resetCode}");
          }
          return new RequestUserPasswordResetPayload();
        }

        /// Inspired by https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.ResetPassword.cs.cshtml
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public async Task<ResetUserPasswordPayload> ResetUserPasswordAsync(
            ResetUserPasswordInput input,
            [ScopedService] UserManager<Data.User> userManager,
            CancellationToken cancellationToken
            )
        {
          var user = await userManager.FindByEmailAsync(input.Email);
          if (input.Password != input.PasswordConfirmation)
          {
            return new ResetUserPasswordPayload(
                new ResetUserPasswordError(
                  ResetUserPasswordErrorCode.PASSWORD_CONFIRMATION_MISMATCH,
                  "The password and confirmation password do not match.",
                  new [] { "input", "passwordConfirmation" }
                  )
                );
          }
          // Don't reveal that the user does not exist
          if (user is not null)
          {
            var identityResult = await userManager.ResetPasswordAsync(user, input.ResetCode, input.Password);
            if (!identityResult.Succeeded)
            {
              var errors = new List<ResetUserPasswordError>();
              foreach (var error in identityResult.Errors)
              {
                errors.Add(
                    // List of codes from https://github.com/aspnet/AspNetIdentity/blob/master/src/Microsoft.AspNet.Identity.Core/Resources.resx#L120
                    error.Code switch
                    {
                    "PasswordRequiresDigit" =>
                    new ResetUserPasswordError(
                        ResetUserPasswordErrorCode.PASSWORD_REQUIRES_DIGIT,
                        error.Description,
                        new [] { "input", "password" }
                        ),
                    "PasswordRequiresLower" =>
                      new ResetUserPasswordError(
                          ResetUserPasswordErrorCode.PASSWORD_REQUIRES_LOWER,
                          error.Description,
                          new [] { "input", "password" }
                          ),
                    "PasswordRequiresNonAlphanumeric" =>
                      new ResetUserPasswordError(
                          ResetUserPasswordErrorCode.PASSWORD_REQUIRES_NON_ALPHANUMERIC,
                          error.Description,
                          new [] { "input", "password" }
                          ),
                    "PasswordRequiresUpper" =>
                      new ResetUserPasswordError(
                          ResetUserPasswordErrorCode.PASSWORD_REQUIRES_UPPER,
                          error.Description,
                          new [] { "input", "password" }
                          ),
                    "PasswordTooShort" =>
                      new ResetUserPasswordError(
                          ResetUserPasswordErrorCode.PASSWORD_TOO_SHORT,
                          error.Description,
                          new [] { "input", "password" }
                          ),
                    _ =>
                      new ResetUserPasswordError(
                          ResetUserPasswordErrorCode.UNKNOWN,
                          $"{error.Description} (error code `{error.Code}`)",
                          new [] { "input" }
                          )
                    }
                );
              }
              return new ResetUserPasswordPayload(errors);
            }
          }
          return new ResetUserPasswordPayload();
        }
    }
}
