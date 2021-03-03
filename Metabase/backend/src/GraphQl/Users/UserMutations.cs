using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Array = System.Array;

namespace Metabase.GraphQl.Users
{
    // TODO Mutations in https://github.com/dotnet/Scaffolding/tree/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account

    [ExtendObjectType(Name = nameof(GraphQl.Mutation))]
    public sealed class UserMutations
    {
        /////////////////////
        // LOGGED-OUT USER //
        /////////////////////

        // Inspired by https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.ConfirmEmail.cs.cshtml
        // and https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.RegisterConfirmation.cs.cshtml
        // Despite its name, it is also used to confirm registrations. TODO Should we add another mutation for that?
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public async Task<ConfirmUserEmailPayload> ConfirmUserEmailAsync(
            ConfirmUserEmailInput input,
            [ScopedService] UserManager<Data.User> userManager
            )
        {
            var user = await userManager.FindByEmailAsync(input.Email).ConfigureAwait(false);
            if (user is null)
            {
                return new ConfirmUserEmailPayload(
                    new ConfirmUserEmailError(
                      ConfirmUserEmailErrorCode.UNKNOWN_USER,
                      $"Unable to load user with email address {input.Email}.",
                      new[] { "input", "email" }
                      )
                    );
            }
            var confirmationToken = DecodeCode(input.ConfirmationCode);
            var identityResult = await userManager.ConfirmEmailAsync(user, confirmationToken).ConfigureAwait(false);
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
                            ConfirmUserEmailErrorCode.INVALID_CONFIRMATION_CODE,
                            error.Description,
                            new[] { "input", "confirmationCode" }
                            ),
                            _ =>
                        new ConfirmUserEmailError(
                            ConfirmUserEmailErrorCode.UNKNOWN,
                            error.Description,
                            new[] { "input" }
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
            [ScopedService] SignInManager<Data.User> signInManager
            )
        {
            // TODO This public endpoint can be used to test whether there is a user for the given email address. Is this a problem? In other endpoints like `ResetUserPasswordAsync` do not do that on purpose. Why exactly?
            var user = await userManager.FindByEmailAsync(input.CurrentEmail).ConfigureAwait(false);
            if (user is null)
            {
                return new ConfirmUserEmailChangePayload(
                    new ConfirmUserEmailChangeError(
                      ConfirmUserEmailChangeErrorCode.UNKNOWN_USER,
                      $"Unable to load user with email address {input.CurrentEmail}.",
                      new[] { "input", "currentEmail" }
                      )
                    );
            }
            var changeEmailIdentityResult =
                await userManager.ChangeEmailAsync(
                    user,
                    input.NewEmail,
                    DecodeCode(input.ConfirmationCode)
                    ).ConfigureAwait(false);
            // For us email and user name are one and the same, so when we
            // update the email we need to update the user name.
            var setUserNameIdentityResult = await userManager.SetUserNameAsync(user, input.NewEmail).ConfigureAwait(false);
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
                            new[] { "input", "newEmail" }
                            ),
                            "InvalidToken" =>
                        new ConfirmUserEmailChangeError(
                            ConfirmUserEmailChangeErrorCode.INVALID_CONFIRMATION_CODE,
                            error.Description,
                            new[] { "input", "confirmationCode" }
                            ),
                            _ =>
                        new ConfirmUserEmailChangeError(
                            ConfirmUserEmailChangeErrorCode.UNKNOWN,
                            error.Description,
                            new[] { "input" }
                            )
                        }
                        );
                }
                foreach (var error in setUserNameIdentityResult.Errors)
                {
                    // Ignore `*UserName` errors that have corresponding `*Email` errors because we use `Email` as `UserName`.
                    if (error.Code != "DuplicateUserName")
                    {
                        errors.Add(
                            // List of codes from https://github.com/aspnet/AspNetIdentity/blob/master/src/Microsoft.AspNet.Identity.Core/Resources.resx#L140
                            error.Code switch
                            {
                                _ =>
                          new ConfirmUserEmailChangeError(
                              ConfirmUserEmailChangeErrorCode.UNKNOWN,
                              error.Description,
                              new[] { "input" }
                              )
                            }
                            );
                    }
                }
                return new ConfirmUserEmailChangePayload(errors);
            }
            await signInManager.RefreshSignInAsync(user).ConfigureAwait(false); // TODO Refresh access token?
            return new ConfirmUserEmailChangePayload(user);
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.Login.cs.cshtml
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        [UseSignInManager]
        public async Task<LoginUserPayload> LoginUserAsync(
            LoginUserInput input,
            [ScopedService] UserManager<Data.User> userManager,
            [ScopedService] SignInManager<Data.User> signInManager
            )
        {
            var signInResult = await signInManager.PasswordSignInAsync(
                input.Email,
                input.Password,
                isPersistent: false, // TODO Use `input.RememberMe`?
                lockoutOnFailure: true
                ).ConfigureAwait(false);
            // https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.signinresult?view=aspnetcore-5.0
            if (signInResult.IsLockedOut)
            {
                return new LoginUserPayload(
                    new LoginUserError(
                      LoginUserErrorCode.LOCKED_OUT,
                      "User is locked out.",
                      new[] { "input" }
                      )
                    );
            }
            if (signInResult.IsNotAllowed)
            {
                return new LoginUserPayload(
                    new LoginUserError(
                      LoginUserErrorCode.NOT_ALLOWED,
                      "User is not allowed to login.",
                      new[] { "input" }
                      )
                    );
            }
            if (!signInResult.Succeeded && !signInResult.RequiresTwoFactor)
            {
                return new LoginUserPayload(
                    new LoginUserError(
                      LoginUserErrorCode.INVALID,
                      "Invalid login attempt.",
                      new[] { "input" }
                      )
                    );
            }
            // TODO Only load the user if requested in the GraphQl query. Use resolver in payload and just pass email address.
            var user = await userManager.FindByEmailAsync(input.Email).ConfigureAwait(false);
            if (signInResult.RequiresTwoFactor)
            {
                return new LoginUserPayload(user, requiresTwoFactor: true);
            }
            return new LoginUserPayload(user, requiresTwoFactor: false);
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
        /*     [ScopedService] SignInManager<Data.User> signInManager */
        /*     ) */
        /* { */
        /*     var user = await signInManager.GetTwoFactorAuthenticationUserAsync(); */
        /*     if (user is null) */
        /*     { */
        /*       return new LoginUserWithTwoFactorPayload( */
        /*           new LoginUserWithTwoFactorError( */
        /*             LoginUserWithTwoFactorErrorCode.UNKNOWN_USER, */
        /*             $"Unable to load two-factor authentication user with email address {input.CurrentEmail}.", */
        /*             new [] { "input", "currentEmail" } */
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
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        [UseSignInManager]
        public async Task<LogoutUserPayload> LogoutUserAsync(
            [ScopedService] SignInManager<Data.User> signInManager
            )
        {
            await signInManager.SignOutAsync().ConfigureAwait(false);
            return new LogoutUserPayload();
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.Register.cs.cshtml
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public async Task<RegisterUserPayload> RegisterUserAsync(
            RegisterUserInput input,
            [ScopedService] UserManager<Data.User> userManager,
            [Service] Services.IEmailSender emailSender
            )
        {
            var user = new Data.User
            {
                UserName = input.Email,
                Email = input.Email
            };
            if (input.Password != input.PasswordConfirmation)
            {
                return new RegisterUserPayload(
                    new RegisterUserError(
                      RegisterUserErrorCode.PASSWORD_CONFIRMATION_MISMATCH,
                      "Password and confirmation password do not match.",
                      new[] { "input", "passwordConfirmation" }
                      )
                    );
            }
            var identityResult =
              await userManager.CreateAsync(
                user,
                input.Password
                ).ConfigureAwait(false);
            if (!identityResult.Succeeded)
            {
                var errors = new List<RegisterUserError>();
                foreach (var error in identityResult.Errors)
                {
                    // Ignore `*UserName` errors that have corresponding `*Email` errors because we use `Email` as `UserName`.
                    if (error.Code != "DuplicateUserName"
                        && error.Code != "InvalidUserName")
                    {
                        errors.Add(
                            // List of codes from https://github.com/aspnet/AspNetIdentity/blob/master/src/Microsoft.AspNet.Identity.Core/Resources.resx#L120
                            error.Code switch
                            {
                                "DuplicateEmail" =>
                        new RegisterUserError(
                            RegisterUserErrorCode.DUPLICATE_EMAIL,
                            error.Description,
                            new[] { "input", "email" }
                            ),
                                "InvalidEmail" =>
                        new RegisterUserError(
                            RegisterUserErrorCode.INVALID_EMAIL,
                            error.Description,
                            new[] { "input", "email" }
                            ),
                                "PasswordRequiresDigit" =>
                        new RegisterUserError(
                            RegisterUserErrorCode.PASSWORD_REQUIRES_DIGIT,
                            error.Description,
                            new[] { "input", "password" }
                            ),
                                "PasswordRequiresLower" =>
                          new RegisterUserError(
                              RegisterUserErrorCode.PASSWORD_REQUIRES_LOWER,
                              error.Description,
                              new[] { "input", "password" }
                              ),
                                "PasswordRequiresNonAlphanumeric" =>
                          new RegisterUserError(
                              RegisterUserErrorCode.PASSWORD_REQUIRES_NON_ALPHANUMERIC,
                              error.Description,
                              new[] { "input", "password" }
                              ),
                                "PasswordRequiresUpper" =>
                          new RegisterUserError(
                              RegisterUserErrorCode.PASSWORD_REQUIRES_UPPER,
                              error.Description,
                              new[] { "input", "password" }
                              ),
                                "PasswordTooShort" =>
                          new RegisterUserError(
                              RegisterUserErrorCode.PASSWORD_TOO_SHORT,
                              error.Description,
                              new[] { "input", "password" }
                              ),
                                "PropertyTooShort" =>
                          new RegisterUserError(
                              RegisterUserErrorCode.NULL_OR_EMPTY_EMAIL,
                              error.Description,
                              new[] { "input", "email" }
                              ),
                                _ =>
                          new RegisterUserError(
                              RegisterUserErrorCode.UNKNOWN,
                              $"{error.Description} (error code `{error.Code}`)",
                              new[] { "input" }
                              )
                            }
                        );
                    }
                }
                return new RegisterUserPayload(errors);
            }
            // TODO The confirmation also confirms the registration/account. Should we use another email text then?
            await SendUserEmailConfirmation(
                input.Email,
                await userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false),
                emailSender
                ).ConfigureAwait(false);
            return new RegisterUserPayload(user);
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.ResendEmailConfirmation.cs.cshtml
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public async Task<ResendUserEmailConfirmationPayload> ResendUserEmailConfirmationAsync(
            ResendUserEmailConfirmationInput input,
            [ScopedService] UserManager<Data.User> userManager,
            [Service] Services.IEmailSender emailSender
            )
        {
            var user = await userManager.FindByEmailAsync(input.Email).ConfigureAwait(false);
            // Don't reveal that the user does not exist.
            if (user is not null)
            {
                await SendUserEmailConfirmation(
                    input.Email,
                    await userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false),
                    emailSender
                    ).ConfigureAwait(false);
            }
            return new ResendUserEmailConfirmationPayload();
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.ForgotPassword.cs.cshtml
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public async Task<RequestUserPasswordResetPayload> RequestUserPasswordResetAsync(
            RequestUserPasswordResetInput input,
            [ScopedService] UserManager<Data.User> userManager,
            [Service] Services.IEmailSender emailSender
            )
        {
            var user = await userManager.FindByEmailAsync(input.Email).ConfigureAwait(false);
            // Don't reveal that the user does not exist or is not confirmed
            if (user is not null && await userManager.IsEmailConfirmedAsync(user).ConfigureAwait(false))
            {
                // For more information on how to enable account confirmation and password reset please
                // visit https://docs.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm?view=aspnetcore-5.0
                var resetCode = EncodeToken(
                      await userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false)
                      );
                await emailSender.SendEmailAsync(
                    input.Email,
                    "Reset password",
                    $"Please reset your password with the reset code {resetCode}."
                    ).ConfigureAwait(false);
            }
            return new RequestUserPasswordResetPayload();
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.ResetPassword.cs.cshtml
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public async Task<ResetUserPasswordPayload> ResetUserPasswordAsync(
            ResetUserPasswordInput input,
            [ScopedService] UserManager<Data.User> userManager
            )
        {
            var user = await userManager.FindByEmailAsync(input.Email).ConfigureAwait(false);
            if (input.Password != input.PasswordConfirmation)
            {
                return new ResetUserPasswordPayload(
                    new ResetUserPasswordError(
                      ResetUserPasswordErrorCode.PASSWORD_CONFIRMATION_MISMATCH,
                      "Password and confirmation password do not match.",
                      new[] { "input", "passwordConfirmation" }
                      )
                    );
            }
            // Don't reveal that the user does not exist
            // TODO As said above, do not reveal that the user does or does not exist. However, right now we reveal whether the user exists or not because errors with the password are only reported when the user exists and not otherwise.
            if (user is not null)
            {
                var identityResult = await userManager.ResetPasswordAsync(
                    user,
                    DecodeCode(input.ResetCode),
                    input.Password
                    ).ConfigureAwait(false);
                if (!identityResult.Succeeded)
                {
                    var errors = new List<ResetUserPasswordError>();
                    foreach (var error in identityResult.Errors)
                    {
                        errors.Add(
                            // List of codes from https://github.com/aspnet/AspNetIdentity/blob/master/src/Microsoft.AspNet.Identity.Core/Resources.resx#L120
                            error.Code switch
                            {
                                "InvalidToken" =>
                            new ResetUserPasswordError(
                                ResetUserPasswordErrorCode.INVALID_RESET_CODE,
                                error.Description,
                                new[] { "input", "resetCode" }
                                ),
                                "PasswordRequiresDigit" =>
                        new ResetUserPasswordError(
                            ResetUserPasswordErrorCode.PASSWORD_REQUIRES_DIGIT,
                            error.Description,
                            new[] { "input", "password" }
                            ),
                                "PasswordRequiresLower" =>
                          new ResetUserPasswordError(
                              ResetUserPasswordErrorCode.PASSWORD_REQUIRES_LOWER,
                              error.Description,
                              new[] { "input", "password" }
                              ),
                                "PasswordRequiresNonAlphanumeric" =>
                          new ResetUserPasswordError(
                              ResetUserPasswordErrorCode.PASSWORD_REQUIRES_NON_ALPHANUMERIC,
                              error.Description,
                              new[] { "input", "password" }
                              ),
                                "PasswordRequiresUpper" =>
                          new ResetUserPasswordError(
                              ResetUserPasswordErrorCode.PASSWORD_REQUIRES_UPPER,
                              error.Description,
                              new[] { "input", "password" }
                              ),
                                "PasswordTooShort" =>
                          new ResetUserPasswordError(
                              ResetUserPasswordErrorCode.PASSWORD_TOO_SHORT,
                              error.Description,
                              new[] { "input", "password" }
                              ),
                                _ =>
                          new ResetUserPasswordError(
                              ResetUserPasswordErrorCode.UNKNOWN,
                              $"{error.Description} (error code `{error.Code}`)",
                              new[] { "input" }
                              )
                            }
                        );
                    }
                    return new ResetUserPasswordPayload(errors);
                }
            }
            return new ResetUserPasswordPayload();
        }

        ////////////////////
        // LOGGED-IN USER //
        ////////////////////

        // Inspired by https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Manage/Account.Manage.ChangePassword.cs.cshtml
        [Authorize]
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        [UseSignInManager]
        public async Task<ChangeUserPasswordPayload> ChangeUserPasswordAsync(
            ChangeUserPasswordInput input,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager,
            [ScopedService] SignInManager<Data.User> signInManager
            )
        {
            var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            if (user is null)
            {
                return new ChangeUserPasswordPayload(
                    new ChangeUserPasswordError(
                      ChangeUserPasswordErrorCode.UNKNOWN_USER,
                      $"Unable to load user with identifier {userManager.GetUserId(claimsPrincipal)}.",
                      Array.Empty<string>()
                      )
                    );
            }
            if (!await userManager.HasPasswordAsync(user).ConfigureAwait(false))
            {
                return new ChangeUserPasswordPayload(
                    user,
                    new ChangeUserPasswordError(
                      ChangeUserPasswordErrorCode.NO_PASSWORD,
                      "You do not have a password yet.",
                      Array.Empty<string>()
                      )
                    );
            }
            if (input.NewPassword != input.NewPasswordConfirmation)
            {
                return new ChangeUserPasswordPayload(
                    user,
                    new ChangeUserPasswordError(
                      ChangeUserPasswordErrorCode.PASSWORD_CONFIRMATION_MISMATCH,
                      "Password and confirmation password do not match.",
                      new[] { "input", "passwordConfirmation" }
                      )
                    );
            }
            var identityResult = await userManager.ChangePasswordAsync(user, input.CurrentPassword, input.NewPassword).ConfigureAwait(false);
            if (!identityResult.Succeeded)
            {
                var errors = new List<ChangeUserPasswordError>();
                foreach (var error in identityResult.Errors)
                {
                    errors.Add(
                        // List of codes from https://github.com/aspnet/AspNetIdentity/blob/master/src/Microsoft.AspNet.Identity.Core/Resources.resx#L120
                        error.Code switch
                        {
                            "PasswordRequiresDigit" =>
                      new ChangeUserPasswordError(
                          ChangeUserPasswordErrorCode.PASSWORD_REQUIRES_DIGIT,
                          error.Description,
                          new[] { "input", "password" }
                          ),
                            "PasswordRequiresLower" =>
                      new ChangeUserPasswordError(
                          ChangeUserPasswordErrorCode.PASSWORD_REQUIRES_LOWER,
                          error.Description,
                          new[] { "input", "password" }
                          ),
                            "PasswordRequiresNonAlphanumeric" =>
                      new ChangeUserPasswordError(
                          ChangeUserPasswordErrorCode.PASSWORD_REQUIRES_NON_ALPHANUMERIC,
                          error.Description,
                          new[] { "input", "password" }
                          ),
                            "PasswordRequiresUpper" =>
                        new ChangeUserPasswordError(
                            ChangeUserPasswordErrorCode.PASSWORD_REQUIRES_UPPER,
                            error.Description,
                            new[] { "input", "password" }
                            ),
                            "PasswordTooShort" =>
                        new ChangeUserPasswordError(
                            ChangeUserPasswordErrorCode.PASSWORD_TOO_SHORT,
                            error.Description,
                            new[] { "input", "password" }
                            ),
                            _ =>
                        new ChangeUserPasswordError(
                            ChangeUserPasswordErrorCode.UNKNOWN,
                            $"{error.Description} (error code `{error.Code}`)",
                            new[] { "input" }
                            )
                        }
                    );
                }
                return new ChangeUserPasswordPayload(user, errors);
            }
            await signInManager.RefreshSignInAsync(user).ConfigureAwait(false); // TODO What exactly does this do? Refresh the cookie? Then it is unnecessary for us! https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.signinmanager-1.refreshsigninasync?view=aspnetcore-5.0#Microsoft_AspNetCore_Identity_SignInManager_1_RefreshSignInAsync__0_
            return new ChangeUserPasswordPayload(user);
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Manage/Account.Manage.DeletePersonalData.cs.cshtml
        [Authorize]
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        [UseSignInManager]
        public async Task<DeletePersonalUserDataPayload> DeletePersonalUserDataAsync(
            DeletePersonalUserDataInput input,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager,
            [ScopedService] SignInManager<Data.User> signInManager
            )
        {
            var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            if (user is null)
            {
                return new DeletePersonalUserDataPayload(
                    new DeletePersonalUserDataError(
                      DeletePersonalUserDataErrorCode.UNKNOWN_USER,
                      $"Unable to load user with identifier {userManager.GetUserId(claimsPrincipal)}.",
                      Array.Empty<string>()
                      )
                    );
            }
            if (await userManager.HasPasswordAsync(user).ConfigureAwait(false))
            {
                if (input.Password is null)
                {
                    return new DeletePersonalUserDataPayload(
                        user,
                        new DeletePersonalUserDataError(
                          DeletePersonalUserDataErrorCode.MISSING_PASSWORD,
                          "Missing password.",
                          new[] { "input", "password" }
                          )
                        );
                }
                if (!await userManager.CheckPasswordAsync(user, input.Password).ConfigureAwait(false))
                {
                    return new DeletePersonalUserDataPayload(
                        user,
                        new DeletePersonalUserDataError(
                          DeletePersonalUserDataErrorCode.INCORRECT_PASSWORD,
                          "Incorrect password.",
                          new[] { "input", "password" }
                          )
                        );
                }
            }
            var identityResult = await userManager.DeleteAsync(user).ConfigureAwait(false);
            if (!identityResult.Succeeded)
            {
                var errors = new List<DeletePersonalUserDataError>();
                foreach (var error in identityResult.Errors)
                {
                    errors.Add(
                        // TODO Which errors can occur here?
                        error.Code switch
                        {
                            _ =>
                        new DeletePersonalUserDataError(
                            DeletePersonalUserDataErrorCode.UNKNOWN,
                            $"{error.Description} (error code `{error.Code}`)",
                            new[] { "input" }
                            )
                        }
                    );
                }
                return new DeletePersonalUserDataPayload(user, errors);
            }
            await signInManager.SignOutAsync().ConfigureAwait(false); // TODO Invalidate access tokens here!
            return new DeletePersonalUserDataPayload(user);
        }

        // TODO Inspired by https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Manage/Account.Manage.Disable2fa.cs.cshtml

        // Inspired by https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Manage/Account.Manage.Email.cs.cshtml
        [Authorize]
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public async Task<ChangeUserEmailPayload> ChangeUserEmailAsync(
            ChangeUserEmailInput input,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager,
            [Service] Services.IEmailSender emailSender
            )
        {
            var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            if (user is null)
            {
                return new ChangeUserEmailPayload(
                    new ChangeUserEmailError(
                      ChangeUserEmailErrorCode.UNKNOWN_USER,
                      $"Unable to load user with identifier {userManager.GetUserId(claimsPrincipal)}.",
                      Array.Empty<string>()
                      )
                    );
            }
            var currentEmail = await userManager.GetEmailAsync(user).ConfigureAwait(false);
            if (currentEmail == input.NewEmail)
            {
                return new ChangeUserEmailPayload(
                    user,
                    new ChangeUserEmailError(
                      ChangeUserEmailErrorCode.UNCHANGED_EMAIL,
                      "Your email is unchanged.",
                      new string[] { "input", "newEmail" }
                      )
                    );
            }
            // TODO Check validity of `input.NewEmail` (use error code `INVALID_EMAIL`)
            await SendUserEmailConfirmation(
                input.NewEmail,
                await userManager.GenerateChangeEmailTokenAsync(user, input.NewEmail).ConfigureAwait(false),
                emailSender
                ).ConfigureAwait(false);
            return new ChangeUserEmailPayload(user);
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Manage/Account.Manage.Email.cs.cshtml
        [Authorize]
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public async Task<ResendUserEmailVerificationPayload> ResendUserEmailVerificationAsync(
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager,
            [Service] Services.IEmailSender emailSender
            )
        {
            var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            if (user is null)
            {
                return new ResendUserEmailVerificationPayload(
                    new ResendUserEmailVerificationError(
                      ResendUserEmailVerificationErrorCode.UNKNOWN_USER,
                      $"Unable to load user with identifier {userManager.GetUserId(claimsPrincipal)}.",
                      Array.Empty<string>()
                      )
                    );
            }
            await SendUserEmailConfirmation(
                await userManager.GetEmailAsync(user).ConfigureAwait(false),
                await userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false),
                emailSender
                ).ConfigureAwait(false);
            return new ResendUserEmailVerificationPayload(user);
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Manage/Account.Manage.GenerateRecoveryCodes.cs.cshtml
        [Authorize]
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public async Task<GenerateUserTwoFactorRecoveryCodesPayload> GenerateUserTwoFactorRecoveryCodesAsync(
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager
            )
        {
            var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            if (user is null)
            {
                return new GenerateUserTwoFactorRecoveryCodesPayload(
                    new GenerateUserTwoFactorRecoveryCodesError(
                      GenerateUserTwoFactorRecoveryCodesErrorCode.UNKNOWN_USER,
                      $"Unable to load user with identifier {userManager.GetUserId(claimsPrincipal)}.",
                      Array.Empty<string>()
                      )
                    );
            }
            if (!await userManager.GetTwoFactorEnabledAsync(user).ConfigureAwait(false))
            {
                return new GenerateUserTwoFactorRecoveryCodesPayload(
                    user,
                    new GenerateUserTwoFactorRecoveryCodesError(
                      GenerateUserTwoFactorRecoveryCodesErrorCode.TWO_FACTOR_AUTHENTICATION_DISABLED,
                      "You have disabled two-factor authentication.",
                      Array.Empty<string>()
                      )
                    );
            }
            return new GenerateUserTwoFactorRecoveryCodesPayload(
                user,
                (await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10).ConfigureAwait(false)).ToList().AsReadOnly()
                );
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Manage/Account.Manage.Index.cs.cshtml
        [Authorize]
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public async Task<SetUserPhoneNumberPayload> SetUserPhoneNumberAsync(
            SetUserPhoneNumberInput input,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager,
            [ScopedService] SignInManager<Data.User> signInManager
            )
        {
            var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            if (user is null)
            {
                return new SetUserPhoneNumberPayload(
                    new SetUserPhoneNumberError(
                      SetUserPhoneNumberErrorCode.UNKNOWN_USER,
                      $"Unable to load user with identifier {userManager.GetUserId(claimsPrincipal)}.",
                      Array.Empty<string>()
                      )
                    );
            }
            var currentPhoneNumber = await userManager.GetPhoneNumberAsync(user).ConfigureAwait(false);
            if (currentPhoneNumber == input.PhoneNumber)
            {
                return new SetUserPhoneNumberPayload(
                    user,
                    new SetUserPhoneNumberError(
                      SetUserPhoneNumberErrorCode.UNCHANGED_PHONE_NUMBER,
                      "Your phone number is unchanged.",
                      new string[] { "input", "phoneNumber" }
                      )
                    );
            }
            var identityResult = await userManager.SetPhoneNumberAsync(user, input.PhoneNumber).ConfigureAwait(false);
            if (!identityResult.Succeeded)
            {
                var errors = new List<SetUserPhoneNumberError>();
                foreach (var error in identityResult.Errors)
                {
                    errors.Add(
                        // TODO Which errors can occur?
                        error.Code switch
                        {
                            _ =>
                        new SetUserPhoneNumberError(
                            SetUserPhoneNumberErrorCode.UNKNOWN,
                            $"{error.Description} (error code `{error.Code}`)",
                            new[] { "input" }
                            )
                        }
                    );
                }
                return new SetUserPhoneNumberPayload(user, errors);
            }
            await signInManager.RefreshSignInAsync(user).ConfigureAwait(false); // TODO Refresh access token?
            return new SetUserPhoneNumberPayload(user);
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Manage/Account.Manage.SetPassword.cs.cshtml
        [Authorize]
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        [UseSignInManager]
        public async Task<SetUserPasswordPayload> SetUserPasswordAsync(
            SetUserPasswordInput input,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager,
            [ScopedService] SignInManager<Data.User> signInManager
            )
        {
            var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            if (user is null)
            {
                return new SetUserPasswordPayload(
                    new SetUserPasswordError(
                      SetUserPasswordErrorCode.UNKNOWN_USER,
                      $"Unable to load user with identifier {userManager.GetUserId(claimsPrincipal)}.",
                      Array.Empty<string>()
                      )
                    );
            }
            if (await userManager.HasPasswordAsync(user).ConfigureAwait(false))
            {
                return new SetUserPasswordPayload(
                    user,
                    new SetUserPasswordError(
                      SetUserPasswordErrorCode.EXISTING_PASSWORD,
                      "You already have a password.",
                      Array.Empty<string>()
                      )
                    );
            }
            if (input.Password != input.PasswordConfirmation)
            {
                return new SetUserPasswordPayload(
                    user,
                    new SetUserPasswordError(
                      SetUserPasswordErrorCode.PASSWORD_CONFIRMATION_MISMATCH,
                      "Password and confirmation password do not match.",
                      new[] { "input", "passwordConfirmation" }
                      )
                    );
            }
            var identityResult = await userManager.AddPasswordAsync(user, input.Password).ConfigureAwait(false);
            if (!identityResult.Succeeded)
            {
                var errors = new List<SetUserPasswordError>();
                foreach (var error in identityResult.Errors)
                {
                    errors.Add(
                        // List of codes from https://github.com/aspnet/AspNetIdentity/blob/master/src/Microsoft.AspNet.Identity.Core/Resources.resx#L120
                        error.Code switch
                        {
                            "PasswordRequiresDigit" =>
                      new SetUserPasswordError(
                          SetUserPasswordErrorCode.PASSWORD_REQUIRES_DIGIT,
                          error.Description,
                          new[] { "input", "password" }
                          ),
                            "PasswordRequiresLower" =>
                      new SetUserPasswordError(
                          SetUserPasswordErrorCode.PASSWORD_REQUIRES_LOWER,
                          error.Description,
                          new[] { "input", "password" }
                          ),
                            "PasswordRequiresNonAlphanumeric" =>
                      new SetUserPasswordError(
                          SetUserPasswordErrorCode.PASSWORD_REQUIRES_NON_ALPHANUMERIC,
                          error.Description,
                          new[] { "input", "password" }
                          ),
                            "PasswordRequiresUpper" =>
                        new SetUserPasswordError(
                            SetUserPasswordErrorCode.PASSWORD_REQUIRES_UPPER,
                            error.Description,
                            new[] { "input", "password" }
                            ),
                            "PasswordTooShort" =>
                        new SetUserPasswordError(
                            SetUserPasswordErrorCode.PASSWORD_TOO_SHORT,
                            error.Description,
                            new[] { "input", "password" }
                            ),
                            _ =>
                        new SetUserPasswordError(
                            SetUserPasswordErrorCode.UNKNOWN,
                            $"{error.Description} (error code `{error.Code}`)",
                            new[] { "input" }
                            )
                        }
                    );
                }
                return new SetUserPasswordPayload(user, errors);
            }
            await signInManager.RefreshSignInAsync(user).ConfigureAwait(false); // TODO What exactly does this do? Refresh the cookie? Then it is unnecessary for us! https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.signinmanager-1.refreshsigninasync?view=aspnetcore-5.0#Microsoft_AspNetCore_Identity_SignInManager_1_RefreshSignInAsync__0_
            return new SetUserPasswordPayload(user);
        }

        private static async Task SendUserEmailConfirmation(
            string email,
            string confirmationToken,
            Services.IEmailSender emailSender
            )
        {
            var confirmationCode = EncodeToken(confirmationToken);
            await emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your email address by clicking the link https://TODO/user/confirm-email?email={email}&confirmationCode={confirmationCode}.")
                .ConfigureAwait(false);
        }

        private static string EncodeToken(string token)
        {
            return
              WebEncoders.Base64UrlEncode(
                  Encoding.UTF8.GetBytes(
                    token
                    )
                  );
        }

        private static string DecodeCode(string code)
        {
            return
              Encoding.UTF8.GetString(
                WebEncoders.Base64UrlDecode(
                  code
                  )
                );
        }
    }
}