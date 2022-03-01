using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Array = System.Array;

// Note that `SignInManager` relies on cookies, see https://github.com/aspnet/Identity/issues/1421. For its source code see https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Core/src/SignInManager.cs
namespace Metabase.GraphQl.Users
{
    [ExtendObjectType(nameof(GraphQl.Mutation))]
    public sealed class UserMutations
    {
        // Key Uri Format https://github.com/google/google-authenticator/wiki/Key-Uri-Format
        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        /////////////////////
        // LOGGED-OUT USER //
        /////////////////////

        // Inspired by https://github.com/dotnet/Scaffolding/blob/main/src/Scaffolding/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.ConfirmEmail.cs.cshtml
        // and https://github.com/dotnet/Scaffolding/blob/main/src/Scaffolding/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.RegisterConfirmation.cs.cshtml
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
                      new[] { nameof(input), nameof(input.Email).FirstCharToLower() }
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
                        // List of codes from https://github.com/aspnet/AspNetIdentity/blob/main/src/Microsoft.AspNet.Identity.Core/Resources.resx#L140
                        error.Code switch
                        {
                            "InvalidToken" =>
                        new ConfirmUserEmailError(
                            ConfirmUserEmailErrorCode.INVALID_CONFIRMATION_CODE,
                            error.Description,
                            new[] { nameof(input), nameof(input.ConfirmationCode).FirstCharToLower() }
                            ),
                            _ =>
                        new ConfirmUserEmailError(
                            ConfirmUserEmailErrorCode.UNKNOWN,
                            error.Description,
                            new[] { nameof(input) }
                            )
                        }
                        );
                }
                return new ConfirmUserEmailPayload(errors);
            }
            return new ConfirmUserEmailPayload(user);
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/main/src/Scaffolding/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.ConfirmEmailChange.cs.cshtml
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
                      new[] { nameof(input), nameof(input.CurrentEmail).FirstCharToLower() }
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
                        // List of codes from https://github.com/aspnet/AspNetIdentity/blob/main/src/Microsoft.AspNet.Identity.Core/Resources.resx#L140
                        error.Code switch
                        {
                            "DuplicateEmail" =>
                        new ConfirmUserEmailChangeError(
                            ConfirmUserEmailChangeErrorCode.DUPLICATE_EMAIL,
                            error.Description,
                            new[] { nameof(input), nameof(input.NewEmail).FirstCharToLower() }
                            ),
                            "InvalidToken" =>
                        new ConfirmUserEmailChangeError(
                            ConfirmUserEmailChangeErrorCode.INVALID_CONFIRMATION_CODE,
                            error.Description,
                            new[] { nameof(input), nameof(input.ConfirmationCode).FirstCharToLower() }
                            ),
                            _ =>
                        new ConfirmUserEmailChangeError(
                            ConfirmUserEmailChangeErrorCode.UNKNOWN,
                            error.Description,
                            new[] { nameof(input) }
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
                            // List of codes from https://github.com/aspnet/AspNetIdentity/blob/main/src/Microsoft.AspNet.Identity.Core/Resources.resx#L140
                            error.Code switch
                            {
                                _ =>
                          new ConfirmUserEmailChangeError(
                              ConfirmUserEmailChangeErrorCode.UNKNOWN,
                              error.Description,
                              new[] { nameof(input) }
                              )
                            }
                            );
                    }
                }
                return new ConfirmUserEmailChangePayload(errors);
            }
            await signInManager.RefreshSignInAsync(user).ConfigureAwait(false);
            return new ConfirmUserEmailChangePayload(user);
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/main/src/Scaffolding/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.Login.cs.cshtml
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
                isPersistent: input.RememberMe,
                lockoutOnFailure: true
                ).ConfigureAwait(false);
            // https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.signinresult?view=aspnetcore-5.0
            if (signInResult.IsLockedOut)
            {
                return new LoginUserPayload(
                    new LoginUserError(
                      LoginUserErrorCode.LOCKED_OUT,
                      "User is locked out.",
                      new[] { nameof(input) }
                      )
                    );
            }
            if (signInResult.IsNotAllowed)
            {
                return new LoginUserPayload(
                    new LoginUserError(
                      LoginUserErrorCode.NOT_ALLOWED,
                      "User is not allowed to login.",
                      new[] { nameof(input) }
                      )
                    );
            }
            if (!signInResult.Succeeded && !signInResult.RequiresTwoFactor)
            {
                return new LoginUserPayload(
                    new LoginUserError(
                      LoginUserErrorCode.INVALID,
                      "Invalid login attempt.",
                      new[] { nameof(input) }
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

        // Inspired by https://github.com/dotnet/Scaffolding/blob/main/src/Scaffolding/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.LoginWith2fa.cs.cshtml
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        [UseSignInManager]
        public async Task<LoginUserWithTwoFactorCodePayload> LoginUserWithTwoFactorCodeAsync(
            LoginUserWithTwoFactorCodeInput input,
            [ScopedService] SignInManager<Data.User> signInManager
            )
        {
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync().ConfigureAwait(false);
            if (user is null)
            {
                return new LoginUserWithTwoFactorCodePayload(
                    new LoginUserWithTwoFactorCodeError(
                      LoginUserWithTwoFactorCodeErrorCode.UNKNOWN_USER,
                      "Unable to load two-factor authentication user.",
                      Array.Empty<string>()
                      )
                    );
            }
            var authenticatorCode =
                input.AuthenticatorCode
                .Replace(" ", string.Empty)
                .Replace("-", string.Empty);
            var signInResult =
                await signInManager.TwoFactorAuthenticatorSignInAsync(
                    authenticatorCode,
                    isPersistent: input.RememberMe,
                    rememberClient: input.RememberMachine
                ).ConfigureAwait(false);
            if (signInResult.IsLockedOut)
            {
                return new LoginUserWithTwoFactorCodePayload(
                    new LoginUserWithTwoFactorCodeError(
                      LoginUserWithTwoFactorCodeErrorCode.LOCKED_OUT,
                      "User is locked out.",
                      new[] { nameof(input) }
                      )
                    );
            }
            if (signInResult.IsNotAllowed)
            {
                return new LoginUserWithTwoFactorCodePayload(
                    new LoginUserWithTwoFactorCodeError(
                      LoginUserWithTwoFactorCodeErrorCode.NOT_ALLOWED,
                      "User is not allowed to login.",
                      new[] { nameof(input) }
                      )
                    );
            }
            if (!signInResult.Succeeded)
            {
                return new LoginUserWithTwoFactorCodePayload(
                    new LoginUserWithTwoFactorCodeError(
                      LoginUserWithTwoFactorCodeErrorCode.INVALID_AUTHENTICATOR_CODE,
                      "Invalid authenticator code.",
                      new[] { nameof(input), nameof(input.AuthenticatorCode).FirstCharToLower() }
                      )
                    );
            }
            return new LoginUserWithTwoFactorCodePayload(user);
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/main/src/Scaffolding/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.LoginWithRecoveryCode.cs.cshtml
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        [UseSignInManager]
        public async Task<LoginUserWithRecoveryCodePayload> LoginUserWithRecoveryCodeAsync(
            LoginUserWithRecoveryCodeInput input,
            [ScopedService] SignInManager<Data.User> signInManager
            )
        {
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync().ConfigureAwait(false);
            if (user is null)
            {
                return new LoginUserWithRecoveryCodePayload(
                    new LoginUserWithRecoveryCodeError(
                      LoginUserWithRecoveryCodeErrorCode.UNKNOWN_USER,
                      "Unable to load two-factor authentication user.",
                      Array.Empty<string>()
                      )
                    );
            }
            var recoveryCode =
                input.RecoveryCode
                .Replace(" ", string.Empty);
            var signInResult =
                await signInManager.TwoFactorRecoveryCodeSignInAsync(
                    recoveryCode
                ).ConfigureAwait(false);
            if (signInResult.IsLockedOut)
            {
                return new LoginUserWithRecoveryCodePayload(
                    new LoginUserWithRecoveryCodeError(
                      LoginUserWithRecoveryCodeErrorCode.LOCKED_OUT,
                      "User is locked out.",
                      new[] { nameof(input) }
                      )
                    );
            }
            if (signInResult.IsNotAllowed)
            {
                return new LoginUserWithRecoveryCodePayload(
                    new LoginUserWithRecoveryCodeError(
                      LoginUserWithRecoveryCodeErrorCode.NOT_ALLOWED,
                      "User is not allowed to login.",
                      new[] { nameof(input) }
                      )
                    );
            }
            if (!signInResult.Succeeded)
            {
                return new LoginUserWithRecoveryCodePayload(
                    new LoginUserWithRecoveryCodeError(
                      LoginUserWithRecoveryCodeErrorCode.INVALID_RECOVERY_CODE,
                      "Invalid recovery code.",
                      new[] { nameof(input), nameof(input.RecoveryCode).FirstCharToLower() }
                      )
                    );
            }
            return new LoginUserWithRecoveryCodePayload(user);
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/main/src/Scaffolding/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.Register.cs.cshtml
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public async Task<RegisterUserPayload> RegisterUserAsync(
            RegisterUserInput input,
            [ScopedService] UserManager<Data.User> userManager,
            [Service] Services.IEmailSender emailSender,
            [Service] AppSettings appSettings
            )
        {
            var user = new Data.User(
                name: input.Name,
                email: input.Email,
                postalAddress: null,
                websiteLocator: null
            );
            if (input.Password != input.PasswordConfirmation)
            {
                return new RegisterUserPayload(
                    new RegisterUserError(
                      RegisterUserErrorCode.PASSWORD_CONFIRMATION_MISMATCH,
                      "Password and confirmation password do not match.",
                      new[] { nameof(input), nameof(input.PasswordConfirmation).FirstCharToLower() }
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
                            // List of codes from https://github.com/aspnet/AspNetIdentity/blob/main/src/Microsoft.AspNet.Identity.Core/Resources.resx#L120
                            error.Code switch
                            {
                                "DuplicateEmail" =>
                        new RegisterUserError(
                            RegisterUserErrorCode.DUPLICATE_EMAIL,
                            error.Description,
                            new[] { nameof(input), nameof(input.Email).FirstCharToLower() }
                            ),
                                "InvalidEmail" =>
                        new RegisterUserError(
                            RegisterUserErrorCode.INVALID_EMAIL,
                            error.Description,
                            new[] { nameof(input), nameof(input.Email).FirstCharToLower() }
                            ),
                                "PasswordRequiresDigit" =>
                        new RegisterUserError(
                            RegisterUserErrorCode.PASSWORD_REQUIRES_DIGIT,
                            error.Description,
                            new[] { nameof(input), nameof(input.Password).FirstCharToLower() }
                            ),
                                "PasswordRequiresLower" =>
                          new RegisterUserError(
                              RegisterUserErrorCode.PASSWORD_REQUIRES_LOWER,
                              error.Description,
                              new[] { nameof(input), nameof(input.Password).FirstCharToLower() }
                              ),
                                "PasswordRequiresNonAlphanumeric" =>
                          new RegisterUserError(
                              RegisterUserErrorCode.PASSWORD_REQUIRES_NON_ALPHANUMERIC,
                              error.Description,
                              new[] { nameof(input), nameof(input.Password).FirstCharToLower() }
                              ),
                                "PasswordRequiresUpper" =>
                          new RegisterUserError(
                              RegisterUserErrorCode.PASSWORD_REQUIRES_UPPER,
                              error.Description,
                              new[] { nameof(input), nameof(input.Password).FirstCharToLower() }
                              ),
                                "PasswordTooShort" =>
                          new RegisterUserError(
                              RegisterUserErrorCode.PASSWORD_TOO_SHORT,
                              error.Description,
                              new[] { nameof(input), nameof(input.Password).FirstCharToLower() }
                              ),
                                "PropertyTooShort" =>
                          new RegisterUserError(
                              RegisterUserErrorCode.NULL_OR_EMPTY_EMAIL,
                              error.Description,
                              new[] { nameof(input), nameof(input.Email).FirstCharToLower() }
                              ),
                                _ =>
                          new RegisterUserError(
                              RegisterUserErrorCode.UNKNOWN,
                              $"{error.Description} (error code `{error.Code}`)",
                              new[] { nameof(input) }
                              )
                            }
                        );
                    }
                }
                return new RegisterUserPayload(errors);
            }
            // TODO The confirmation also confirms the registration/account. Should we use another email text then?
            await SendUserEmailConfirmation(
                (user.Name, input.Email),
                await userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false),
                emailSender,
                appSettings.Host
                ).ConfigureAwait(false);
            return new RegisterUserPayload(user);
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/main/src/Scaffolding/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.ResendEmailConfirmation.cs.cshtml
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public async Task<ResendUserEmailConfirmationPayload> ResendUserEmailConfirmationAsync(
            ResendUserEmailConfirmationInput input,
            [ScopedService] UserManager<Data.User> userManager,
            [Service] Services.IEmailSender emailSender,
            [Service] AppSettings appSettings
            )
        {
            var user = await userManager.FindByEmailAsync(input.Email).ConfigureAwait(false);
            // Don't reveal that the user does not exist.
            if (user is not null)
            {
                await SendUserEmailConfirmation(
                    (user.Name, input.Email),
                    await userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false),
                    emailSender,
                    appSettings.Host
                    ).ConfigureAwait(false);
            }
            return new ResendUserEmailConfirmationPayload();
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/main/src/Scaffolding/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.ForgotPassword.cs.cshtml
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public async Task<RequestUserPasswordResetPayload> RequestUserPasswordResetAsync(
            RequestUserPasswordResetInput input,
            [ScopedService] UserManager<Data.User> userManager,
            [Service] Services.IEmailSender emailSender,
            [Service] AppSettings appSettings
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
                await emailSender.SendAsync(
                    (user.Name, input.Email),
                    "Reset password",
                    $"Please reset your password by following the link {appSettings.Host}/users/reset-password?resetCode={resetCode}."
                    ).ConfigureAwait(false);
            }
            return new RequestUserPasswordResetPayload();
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/main/src/Scaffolding/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.ResetPassword.cs.cshtml
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
                      new[] { nameof(input), nameof(input.PasswordConfirmation).FirstCharToLower() }
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
                            // List of codes from https://github.com/aspnet/AspNetIdentity/blob/main/src/Microsoft.AspNet.Identity.Core/Resources.resx#L120
                            error.Code switch
                            {
                                "InvalidToken" =>
                            new ResetUserPasswordError(
                                ResetUserPasswordErrorCode.INVALID_RESET_CODE,
                                error.Description,
                                new[] { nameof(input), nameof(input.ResetCode).FirstCharToLower() }
                                ),
                                "PasswordRequiresDigit" =>
                        new ResetUserPasswordError(
                            ResetUserPasswordErrorCode.PASSWORD_REQUIRES_DIGIT,
                            error.Description,
                            new[] { nameof(input), nameof(input.Password).FirstCharToLower() }
                            ),
                                "PasswordRequiresLower" =>
                          new ResetUserPasswordError(
                              ResetUserPasswordErrorCode.PASSWORD_REQUIRES_LOWER,
                              error.Description,
                              new[] { nameof(input), nameof(input.Password).FirstCharToLower() }
                              ),
                                "PasswordRequiresNonAlphanumeric" =>
                          new ResetUserPasswordError(
                              ResetUserPasswordErrorCode.PASSWORD_REQUIRES_NON_ALPHANUMERIC,
                              error.Description,
                              new[] { nameof(input), nameof(input.Password).FirstCharToLower() }
                              ),
                                "PasswordRequiresUpper" =>
                          new ResetUserPasswordError(
                              ResetUserPasswordErrorCode.PASSWORD_REQUIRES_UPPER,
                              error.Description,
                              new[] { nameof(input), nameof(input.Password).FirstCharToLower() }
                              ),
                                "PasswordTooShort" =>
                          new ResetUserPasswordError(
                              ResetUserPasswordErrorCode.PASSWORD_TOO_SHORT,
                              error.Description,
                              new[] { nameof(input), nameof(input.Password).FirstCharToLower() }
                              ),
                                _ =>
                          new ResetUserPasswordError(
                              ResetUserPasswordErrorCode.UNKNOWN,
                              $"{error.Description} (error code `{error.Code}`)",
                              new[] { nameof(input) }
                              )
                            }
                        );
                    }
                    return new ResetUserPasswordPayload(errors);
                }
            }
            return new ResetUserPasswordPayload();
        }

        [Authorize(Policy = Configuration.AuthConfiguration.ManageUserPolicy)]
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public async Task<DeleteUserPayload> DeleteUserAsync(
            DeleteUserInput input,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager
            )
        {
            if (!await UserAuthorization.IsAuthorizedToDeleteUsers(
                    claimsPrincipal,
                    userManager
                ).ConfigureAwait(false)
            )
            {
                return new DeleteUserPayload(
                    new DeleteUserError(
                      DeleteUserErrorCode.UNAUTHORIZED,
                      $"You are not authorized to delete user with identifier {input.UserId}.",
                      new[] { nameof(input), nameof(input.UserId).FirstCharToLower() }
                      )
                    );
            }
            var user =
                await userManager.Users.SingleOrDefaultAsync(_ =>
                    _.Id == input.UserId
                ).ConfigureAwait(false);
            if (user is null)
            {
                return new DeleteUserPayload(
                    new DeleteUserError(
                      DeleteUserErrorCode.UNKNOWN_USER,
                      $"Unable to load user with identifier {input.UserId}.",
                      new[] { nameof(input), nameof(input.UserId).FirstCharToLower() }
                      )
                    );
            }
            var identityResult = await userManager.DeleteAsync(user).ConfigureAwait(false);
            if (!identityResult.Succeeded)
            {
                var errors = new List<DeleteUserError>();
                foreach (var error in identityResult.Errors)
                {
                    errors.Add(
                        // TODO Which errors can occur here?
                        error.Code switch
                        {
                            _ =>
                        new DeleteUserError(
                            DeleteUserErrorCode.UNKNOWN,
                            $"{error.Description} (error code `{error.Code}`)",
                            new[] { nameof(input) }
                            )
                        }
                    );
                }
                return new DeleteUserPayload(user, errors);
            }
            return new DeleteUserPayload(user);
        }

        ////////////////////
        // LOGGED-IN USER //
        ////////////////////

        // Inspired by https://github.com/dotnet/Scaffolding/blob/main/src/Scaffolding/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Account.Logout.cs.cshtml
        [Authorize(Policy = Configuration.AuthConfiguration.ManageUserPolicy)]
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

        // Inspired by https://github.com/dotnet/Scaffolding/blob/main/src/Scaffolding/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Manage/Account.Manage.ChangePassword.cs.cshtml
        [Authorize(Policy = Configuration.AuthConfiguration.ManageUserPolicy)]
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
                      new[] { nameof(input), nameof(input.NewPasswordConfirmation).FirstCharToLower() }
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
                        // List of codes from https://github.com/aspnet/AspNetIdentity/blob/main/src/Microsoft.AspNet.Identity.Core/Resources.resx#L120
                        error.Code switch
                        {
                            "PasswordRequiresDigit" =>
                      new ChangeUserPasswordError(
                          ChangeUserPasswordErrorCode.PASSWORD_REQUIRES_DIGIT,
                          error.Description,
                          new[] { nameof(input), nameof(input.NewPassword).FirstCharToLower() }
                          ),
                            "PasswordRequiresLower" =>
                      new ChangeUserPasswordError(
                          ChangeUserPasswordErrorCode.PASSWORD_REQUIRES_LOWER,
                          error.Description,
                          new[] { nameof(input), nameof(input.NewPassword).FirstCharToLower() }
                          ),
                            "PasswordRequiresNonAlphanumeric" =>
                      new ChangeUserPasswordError(
                          ChangeUserPasswordErrorCode.PASSWORD_REQUIRES_NON_ALPHANUMERIC,
                          error.Description,
                          new[] { nameof(input), nameof(input.NewPassword).FirstCharToLower() }
                          ),
                            "PasswordRequiresUpper" =>
                        new ChangeUserPasswordError(
                            ChangeUserPasswordErrorCode.PASSWORD_REQUIRES_UPPER,
                            error.Description,
                            new[] { nameof(input), nameof(input.NewPassword).FirstCharToLower() }
                            ),
                            "PasswordTooShort" =>
                        new ChangeUserPasswordError(
                            ChangeUserPasswordErrorCode.PASSWORD_TOO_SHORT,
                            error.Description,
                            new[] { nameof(input), nameof(input.NewPassword).FirstCharToLower() }
                            ),
                            _ =>
                        new ChangeUserPasswordError(
                            ChangeUserPasswordErrorCode.UNKNOWN,
                            $"{error.Description} (error code `{error.Code}`)",
                            new[] { nameof(input) }
                            )
                        }
                    );
                }
                return new ChangeUserPasswordPayload(user, errors);
            }
            await signInManager.RefreshSignInAsync(user).ConfigureAwait(false);
            return new ChangeUserPasswordPayload(user);
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/main/src/Scaffolding/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Manage/Account.Manage.DeletePersonalData.cs.cshtml
        [Authorize(Policy = Configuration.AuthConfiguration.ManageUserPolicy)]
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
                          new[] { nameof(input), nameof(input.Password).FirstCharToLower() }
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
                          new[] { nameof(input), nameof(input.Password).FirstCharToLower() }
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
                            new[] { nameof(input) }
                            )
                        }
                    );
                }
                return new DeletePersonalUserDataPayload(user, errors);
            }
            await signInManager.SignOutAsync().ConfigureAwait(false);
            return new DeletePersonalUserDataPayload(user);
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/main/src/Scaffolding/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Manage/Account.Manage.Disable2fa.cs.cshtml
        [Authorize(Policy = Configuration.AuthConfiguration.ManageUserPolicy)]
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public async Task<DisableUserTwoFactorAuthenticationPayload> DisableUserTwoFactorAuthenticationAsync(
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager
            )
        {
            var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            if (user is null)
            {
                return new DisableUserTwoFactorAuthenticationPayload(
                    new DisableUserTwoFactorAuthenticationError(
                      DisableUserTwoFactorAuthenticationErrorCode.UNKNOWN_USER,
                      $"Unable to load user with identifier {userManager.GetUserId(claimsPrincipal)}.",
                      Array.Empty<string>()
                      )
                    );
            }
            var disableResult = await userManager.SetTwoFactorEnabledAsync(user, false).ConfigureAwait(false);
            if (!disableResult.Succeeded)
            {
                return new DisableUserTwoFactorAuthenticationPayload(
                    new DisableUserTwoFactorAuthenticationError(
                      DisableUserTwoFactorAuthenticationErrorCode.UNKNOWN,
                      "Unknown error.",
                      Array.Empty<string>()
                      )
                    );
            }
            return new DisableUserTwoFactorAuthenticationPayload(user);
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/main/src/Scaffolding/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Manage/Account.Manage.TwoFactorAuthentication.cs.cshtml
        [Authorize(Policy = Configuration.AuthConfiguration.ManageUserPolicy)]
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        [UseSignInManager]
        public async Task<ForgetUserTwoFactorAuthenticationClientPayload> ForgetUserTwoFactorAuthenticationClientAsync(
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager,
            [ScopedService] SignInManager<Data.User> signInManager
            )
        {
            var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            if (user is null)
            {
                return new ForgetUserTwoFactorAuthenticationClientPayload(
                    new ForgetUserTwoFactorAuthenticationClientError(
                      ForgetUserTwoFactorAuthenticationClientErrorCode.UNKNOWN_USER,
                      $"Unable to load user with identifier {userManager.GetUserId(claimsPrincipal)}.",
                      Array.Empty<string>()
                      )
                    );
            }
            await signInManager.ForgetTwoFactorClientAsync().ConfigureAwait(false);
            return new ForgetUserTwoFactorAuthenticationClientPayload(user);
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/main/src/Scaffolding/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Manage/Account.Manage.EnableAuthenticator.cs.cshtml
        [Authorize(Policy = Configuration.AuthConfiguration.ManageUserPolicy)]
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public async Task<GenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriPayload> GenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriAsync(
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager,
            [Service] UrlEncoder urlEncoder
            )
        {
            var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            if (user is null)
            {
                return new GenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriPayload(
                    new GenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriError(
                      GenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriErrorCode.UNKNOWN_USER,
                      $"Unable to load user with identifier {userManager.GetUserId(claimsPrincipal)}.",
                      Array.Empty<string>()
                      )
                    );
            }
            var (sharedKey, authenticatorUri) = await LoadSharedKeyAndQrCodeUriAsync(userManager, urlEncoder, user).ConfigureAwait(false);
            return new GenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriPayload(
                user,
                sharedKey,
                authenticatorUri
                );
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/main/src/Scaffolding/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Manage/Account.Manage.EnableAuthenticator.cs.cshtml
        [Authorize(Policy = Configuration.AuthConfiguration.ManageUserPolicy)]
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public async Task<EnableUserTwoFactorAuthenticatorPayload> EnableUserTwoFactorAuthenticatorAsync(
            EnableUserTwoFactorAuthenticatorInput input,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager,
            [Service] UrlEncoder urlEncoder
            )
        {
            var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            if (user is null)
            {
                return new EnableUserTwoFactorAuthenticatorPayload(
                    new EnableUserTwoFactorAuthenticatorError(
                      EnableUserTwoFactorAuthenticatorErrorCode.UNKNOWN_USER,
                      $"Unable to load user with identifier {userManager.GetUserId(claimsPrincipal)}.",
                      Array.Empty<string>()
                      )
                    );
            }
            var verificationToken =
                input.VerificationCode
                .Replace(" ", string.Empty)
                .Replace("-", string.Empty);
            var isTokenValid =
                await userManager.VerifyTwoFactorTokenAsync(
                    user,
                    userManager.Options.Tokens.AuthenticatorTokenProvider,
                    verificationToken
                    )
                    .ConfigureAwait(false);
            if (!isTokenValid)
            {
                var (sharedKey, authenticatorUri) = await LoadSharedKeyAndQrCodeUriAsync(userManager, urlEncoder, user).ConfigureAwait(false);
                return new EnableUserTwoFactorAuthenticatorPayload(
                    new EnableUserTwoFactorAuthenticatorError(
                      EnableUserTwoFactorAuthenticatorErrorCode.INVALID_VERIFICATION_CODE,
                      "Verification code is invalid.",
                      new[] { nameof(input), nameof(input.VerificationCode).FirstCharToLower() }
                      ),
                    sharedKey,
                    authenticatorUri
                    );
            }
            var enableResult = await userManager.SetTwoFactorEnabledAsync(user, true).ConfigureAwait(false);
            if (!enableResult.Succeeded)
            {
                var (sharedKey, authenticatorUri) = await LoadSharedKeyAndQrCodeUriAsync(userManager, urlEncoder, user).ConfigureAwait(false);
                return new EnableUserTwoFactorAuthenticatorPayload(
                    new EnableUserTwoFactorAuthenticatorError(
                      EnableUserTwoFactorAuthenticatorErrorCode.ENABLING_FAILED,
                      "Unknown error enabling.",
                      Array.Empty<string>()
                      ),
                    sharedKey,
                    authenticatorUri
                    );
            }
            if (await userManager.CountRecoveryCodesAsync(user).ConfigureAwait(false) == 0)
            {
                var recoveryCodes = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10).ConfigureAwait(false);
                return new EnableUserTwoFactorAuthenticatorPayload(user, recoveryCodes.ToList().AsReadOnly());
            }
            else
            {
                return new EnableUserTwoFactorAuthenticatorPayload(user);
            }
        }

        private static async Task<(string sharedKey, string authenticatorUri)> LoadSharedKeyAndQrCodeUriAsync(UserManager<Data.User> userManager, UrlEncoder urlEncoder, Data.User user)
        {
            // Load the authenticator key & QR code URI to display on the form
            var unformattedKey = await userManager.GetAuthenticatorKeyAsync(user).ConfigureAwait(false);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await userManager.ResetAuthenticatorKeyAsync(user).ConfigureAwait(false);
                unformattedKey = await userManager.GetAuthenticatorKeyAsync(user).ConfigureAwait(false);
            }
            var sharedKey = FormatKey(unformattedKey);
            var email = await userManager.GetEmailAsync(user).ConfigureAwait(false);
            var authenticatorUri = GenerateQrCodeUri(urlEncoder, email, unformattedKey);
            return (sharedKey, authenticatorUri);
        }

        private static string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            var currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey, currentPosition, 4).Append(' ');
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey, currentPosition, unformattedKey.Length - currentPosition);
            }
            return result.ToString().ToLowerInvariant();
        }

        private static string GenerateQrCodeUri(UrlEncoder urlEncoder, string email, string unformattedKey)
        {
            return string.Format(
                AuthenticatorUriFormat,
                urlEncoder.Encode("buildingenvelopedata.org"), // issuer
                urlEncoder.Encode(email), // account name
                unformattedKey // secret
                );
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/main/src/Scaffolding/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Manage/Account.Manage.ResetAuthenticator.cs.cshtml
        [Authorize(Policy = Configuration.AuthConfiguration.ManageUserPolicy)]
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        [UseSignInManager]
        public async Task<ResetUserTwoFactorAuthenticatorPayload> ResetUserTwoFactorAuthenticatorAsync(
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager,
            [ScopedService] SignInManager<Data.User> signInManager
            )
        {
            var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            if (user is null)
            {
                return new ResetUserTwoFactorAuthenticatorPayload(
                    new ResetUserTwoFactorAuthenticatorError(
                      ResetUserTwoFactorAuthenticatorErrorCode.UNKNOWN_USER,
                      $"Unable to load user with identifier {userManager.GetUserId(claimsPrincipal)}.",
                      Array.Empty<string>()
                      )
                    );
            }
            var disableResult = await userManager.SetTwoFactorEnabledAsync(user, false).ConfigureAwait(false);
            if (!disableResult.Succeeded)
            {
                return new ResetUserTwoFactorAuthenticatorPayload(
                    new ResetUserTwoFactorAuthenticatorError(
                      ResetUserTwoFactorAuthenticatorErrorCode.DISABLING_FAILED,
                      "Unknown error disabling.",
                      Array.Empty<string>()
                      )
                    );
            }
            var resetResult = await userManager.ResetAuthenticatorKeyAsync(user).ConfigureAwait(false);
            if (!resetResult.Succeeded)
            {
                return new ResetUserTwoFactorAuthenticatorPayload(
                    new ResetUserTwoFactorAuthenticatorError(
                      ResetUserTwoFactorAuthenticatorErrorCode.RESETTING_FAILED,
                      "Unknown error resetting.",
                      Array.Empty<string>()
                      )
                    );
            }
            await signInManager.RefreshSignInAsync(user).ConfigureAwait(false);
            return new ResetUserTwoFactorAuthenticatorPayload(user);
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/main/src/Scaffolding/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Manage/Account.Manage.Email.cs.cshtml
        [Authorize(Policy = Configuration.AuthConfiguration.ManageUserPolicy)]
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public async Task<ChangeUserEmailPayload> ChangeUserEmailAsync(
            ChangeUserEmailInput input,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager,
            [Service] Services.IEmailSender emailSender,
            [Service] AppSettings appSettings
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
                      new string[] { nameof(input), nameof(input.NewEmail).FirstCharToLower() }
                      )
                    );
            }
            // TODO Check validity of `input.NewEmail` (use error code `INVALID_EMAIL`)
            await SendChangeUserEmailConfirmation(
                user.Name,
                currentEmail,
                input.NewEmail,
                await userManager.GenerateChangeEmailTokenAsync(user, input.NewEmail).ConfigureAwait(false),
                emailSender,
                appSettings.Host
                ).ConfigureAwait(false);
            return new ChangeUserEmailPayload(user);
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/main/src/Scaffolding/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Manage/Account.Manage.Email.cs.cshtml
        [Authorize(Policy = Configuration.AuthConfiguration.ManageUserPolicy)]
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public async Task<ResendUserEmailVerificationPayload> ResendUserEmailVerificationAsync(
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager,
            [Service] Services.IEmailSender emailSender,
            [Service] AppSettings appSettings
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
                (user.Name, await userManager.GetEmailAsync(user).ConfigureAwait(false)),
                await userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false),
                emailSender,
                appSettings.Host
                ).ConfigureAwait(false);
            return new ResendUserEmailVerificationPayload(user);
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/main/src/Scaffolding/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Manage/Account.Manage.GenerateRecoveryCodes.cs.cshtml
        [Authorize(Policy = Configuration.AuthConfiguration.ManageUserPolicy)]
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

        // Inspired by https://github.com/dotnet/Scaffolding/blob/main/src/Scaffolding/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Manage/Account.Manage.Index.cs.cshtml
        [Authorize(Policy = Configuration.AuthConfiguration.ManageUserPolicy)]
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        [UseSignInManager]
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
                      new string[] { nameof(input), nameof(input.PhoneNumber).FirstCharToLower() }
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
                            new[] { nameof(input) }
                            )
                        }
                    );
                }
                return new SetUserPhoneNumberPayload(user, errors);
            }
            await signInManager.RefreshSignInAsync(user).ConfigureAwait(false);
            return new SetUserPhoneNumberPayload(user);
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/main/src/Scaffolding/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Manage/Account.Manage.SetPassword.cs.cshtml
        [Authorize(Policy = Configuration.AuthConfiguration.ManageUserPolicy)]
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
                      new[] { nameof(input), nameof(input.PasswordConfirmation).FirstCharToLower() }
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
                        // List of codes from https://github.com/aspnet/AspNetIdentity/blob/main/src/Microsoft.AspNet.Identity.Core/Resources.resx#L120
                        error.Code switch
                        {
                            "PasswordRequiresDigit" =>
                      new SetUserPasswordError(
                          SetUserPasswordErrorCode.PASSWORD_REQUIRES_DIGIT,
                          error.Description,
                          new[] { nameof(input), nameof(input.Password).FirstCharToLower() }
                          ),
                            "PasswordRequiresLower" =>
                      new SetUserPasswordError(
                          SetUserPasswordErrorCode.PASSWORD_REQUIRES_LOWER,
                          error.Description,
                          new[] { nameof(input), nameof(input.Password).FirstCharToLower() }
                          ),
                            "PasswordRequiresNonAlphanumeric" =>
                      new SetUserPasswordError(
                          SetUserPasswordErrorCode.PASSWORD_REQUIRES_NON_ALPHANUMERIC,
                          error.Description,
                          new[] { nameof(input), nameof(input.Password).FirstCharToLower() }
                          ),
                            "PasswordRequiresUpper" =>
                        new SetUserPasswordError(
                            SetUserPasswordErrorCode.PASSWORD_REQUIRES_UPPER,
                            error.Description,
                            new[] { nameof(input), nameof(input.Password).FirstCharToLower() }
                            ),
                            "PasswordTooShort" =>
                        new SetUserPasswordError(
                            SetUserPasswordErrorCode.PASSWORD_TOO_SHORT,
                            error.Description,
                            new[] { nameof(input), nameof(input.Password).FirstCharToLower() }
                            ),
                            _ =>
                        new SetUserPasswordError(
                            SetUserPasswordErrorCode.UNKNOWN,
                            $"{error.Description} (error code `{error.Code}`)",
                            new[] { nameof(input) }
                            )
                        }
                    );
                }
                return new SetUserPasswordPayload(user, errors);
            }
            await signInManager.RefreshSignInAsync(user).ConfigureAwait(false);
            return new SetUserPasswordPayload(user);
        }

        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public async Task<AddUserRolePayload> AddUserRoleAsync(
            AddUserRoleInput input,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager,
            [ScopedService] Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            if (!await UserAuthorization.IsAuthorizedToAddOrRemoveRole(claimsPrincipal, input.Role, userManager).ConfigureAwait(false))
            {
                return new AddUserRolePayload(
                    new AddUserRoleError(
                      AddUserRoleErrorCode.UNAUTHORIZED,
                      $"You are not authorized to add role {input.Role}.",
                      Array.Empty<string>()
                    )
                );
            }
            var user = await context.Users.AsQueryable()
                .SingleOrDefaultAsync(
                    x => x.Id == input.UserId,
                    cancellationToken
                ).ConfigureAwait(false);
            if (user is null)
            {
                return new AddUserRolePayload(
                    new AddUserRoleError(
                      AddUserRoleErrorCode.UNKNOWN_USER,
                      "Unknown user.",
                      new[] { nameof(input), nameof(input.UserId).FirstCharToLower() }
                      )
                    );
            }
            var identityResult = await userManager.AddToRoleAsync(user, Data.Role.EnumToName(input.Role)).ConfigureAwait(false);
            if (!identityResult.Succeeded)
            {
                var errors = new List<AddUserRoleError>();
                foreach (var error in identityResult.Errors)
                {
                    errors.Add(
                        // TODO Which error codes occur here? When known, translate the properly into `AddUserRoleErrorCode`.
                        error.Code switch
                        {
                            _ =>
                                new AddUserRoleError(
                                    AddUserRoleErrorCode.UNKNOWN,
                                    $"{error.Description} (error code `{error.Code}`)",
                                    new[] { nameof(input) }
                                    )
                        }
                    );
                }
                return new AddUserRolePayload(user, errors);
            }
            return new AddUserRolePayload(user);
        }

        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public async Task<RemoveUserRolePayload> RemoveUserRoleAsync(
            RemoveUserRoleInput input,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager,
            [ScopedService] Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            if (!await UserAuthorization.IsAuthorizedToAddOrRemoveRole(claimsPrincipal, input.Role, userManager).ConfigureAwait(false))
            {
                return new RemoveUserRolePayload(
                    new RemoveUserRoleError(
                      RemoveUserRoleErrorCode.UNAUTHORIZED,
                      $"You are not authorized to remove role {input.Role}.",
                      Array.Empty<string>()
                    )
                );
            }
            var user = await context.Users.AsQueryable()
                .SingleOrDefaultAsync(
                    x => x.Id == input.UserId,
                    cancellationToken
                ).ConfigureAwait(false);
            if (user is null)
            {
                return new RemoveUserRolePayload(
                    new RemoveUserRoleError(
                      RemoveUserRoleErrorCode.UNKNOWN_USER,
                      "Unknown user.",
                      new[] { nameof(input), nameof(input.UserId).FirstCharToLower() }
                      )
                    );
            }
            var identityResult = await userManager.RemoveFromRoleAsync(user, Data.Role.EnumToName(input.Role)).ConfigureAwait(false);
            if (!identityResult.Succeeded)
            {
                var errors = new List<RemoveUserRoleError>();
                foreach (var error in identityResult.Errors)
                {
                    errors.Remove(
                        // TODO Which error codes occur here? When known, translate the properly into `RemoveUserRoleErrorCode`.
                        error.Code switch
                        {
                            _ =>
                                new RemoveUserRoleError(
                                    RemoveUserRoleErrorCode.UNKNOWN,
                                    $"{error.Description} (error code `{error.Code}`)",
                                    new[] { nameof(input) }
                                    )
                        }
                    );
                }
                return new RemoveUserRolePayload(user, errors);
            }
            return new RemoveUserRolePayload(user);
        }

        private static async Task SendUserEmailConfirmation(
            (string name, string address) to,
            string confirmationToken,
            Services.IEmailSender emailSender,
            string host
            )
        {
            var confirmationCode = EncodeToken(confirmationToken);
            await emailSender.SendAsync(
                to,
                "Confirm your email",
                $"Please confirm your email address by following the link {host}/users/confirm-email?email={to.address}&confirmationCode={confirmationCode}")
                .ConfigureAwait(false);
        }

        private static async Task SendChangeUserEmailConfirmation(
            string name,
            string currentEmail,
            string newEmail,
            string confirmationToken,
            Services.IEmailSender emailSender,
            string host
        )
        {
            var confirmationCode = EncodeToken(confirmationToken);
            await emailSender.SendAsync(
                (name, newEmail),
                "Confirm your email change",
                $"Please confirm your email address change by following the link {host}/users/confirm-email-change?currentEmail={currentEmail}&newEmail={newEmail}&confirmationCode={confirmationCode}")
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