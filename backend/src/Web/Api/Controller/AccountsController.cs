using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Icon.Web.Api.Controller
{
    /* [Route("api/components")] */
    /* [ApiController] */
    /* [ApiConventionType(typeof(DefaultApiConventions))] */
    /* [Authorize(LocalApi.PolicyName)] */
    /* public class AccountsController : ControllerBase */
    /* { */
    /*     private readonly UserManager<User> _userManager; */
    /*     private readonly SignInManager<User> _signInManager; */
    /*     private readonly ILogger<LoginModel> _logger; */
    /*     private readonly IEmailSender _emailSender; */

    /*     public LoginModel(SignInManager<User> signInManager, */
    /*         ILogger<LoginModel> logger, */
    /*         UserManager<User> userManager, */
    /*         IEmailSender emailSender) */
    /*     { */
    /*         _userManager = userManager; */
    /*         _signInManager = signInManager; */
    /*         _emailSender = emailSender; */
    /*         _logger = logger; */
    /*     } */

    /*     public class SignInInputModel */
    /*     { */
    /*         [Required] */
    /*         [EmailAddress] */
    /*         public string EmailAddress { get; set; } */

    /*         [Required] */
    /*         [DataType(DataType.Password)] */
    /*         public string Password { get; set; } */

    /*         [Display(Name = "Remember me?")] */
    /*         public bool RememberMe { get; set; } */
    /*     } */

    /*     [HttpPost("sign-in")] */
    /*     [AllowAnonymous] */
    /*     public async Task<IActionResult> SignIn([FromBody] SignInInputModel) */
    /*     { */
    /*       var result = await _signInManager.PasswordSignInAsync(Input.EmailAddress, Input.Password, Input.RememberMe, lockoutOnFailure: false); */
    /*       if (result.Succeeded) */
    /*       { */
    /*         _logger.LogInformation("User logged in."); */
    /*         return LocalRedirect(returnUrl); */
    /*       } */
    /*       if (result.RequiresTwoFactor) */
    /*       { */
    /*         return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe }); */
    /*       } */
    /*       if (result.IsLockedOut) */
    /*       { */
    /*         _logger.LogWarning("User account locked out."); */
    /*         return RedirectToPage("./Lockout"); */
    /*       } */
    /*       else */
    /*       { */
    /*         ModelState.AddModelError(string.Empty, "Invalid login attempt."); */
    /*         return Page(); */
    /*       } */
    /*     } */
    /* } */

    /* /1* public async Task<IActionResult> OnPostSendVerificationEmailAsync() *1/ */
    /* /1* { *1/ */
    /* /1*     if (!ModelState.IsValid) *1/ */
    /* /1*     { *1/ */
    /* /1*         return Page(); *1/ */
    /* /1*     } *1/ */

    /* /1*     var user = await _userManager.FindByEmailAsync(Input.Email); *1/ */
    /* /1*     if (user is null) *1/ */
    /* /1*     { *1/ */
    /* /1*         ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email."); *1/ */
    /* /1*     } *1/ */

    /* /1*     var userId = await _userManager.GetUserIdAsync(user); *1/ */
    /* /1*     var code = await _userManager.GenerateEmailConfirmationTokenAsync(user); *1/ */
    /* /1*     var callbackUrl = Url.Page( *1/ */
    /* /1*         "/Account/ConfirmEmail", *1/ */
    /* /1*         pageHandler: null, *1/ */
    /* /1*         values: new { userId = userId, code = code }, *1/ */
    /* /1*         protocol: Request.Scheme); *1/ */
    /* /1*     await _emailSender.SendEmailAsync( *1/ */
    /* /1*         Input.Email, *1/ */
    /* /1*         "Confirm your email", *1/ */
    /* /1*         $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."); *1/ */

    /* /1*     ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email."); *1/ */
    /* /1*     return Page(); *1/ */
    /* /1* } *1/ */
}