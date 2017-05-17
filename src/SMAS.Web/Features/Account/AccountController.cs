using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SMAS.Web.Core.Configuration;
using SMAS.Web.Core.Services;
using SMAS.Web.Features.Account.Models;
using SMAS.Web.Features.Home;
using SMAS.Web.Features.Shared;
using OrangeJetpack.Core.Web.UI;
using OrangeJetpack.Core.Web.Utilities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SMAS.Services.Identity;
using OrangeJetpack.Core.Security;
using OrangeJetpack.Core.Security.Exceptions;
using OrangeJetpack.Regionalization;
using OrangeJetpack.Services.Client.Messaging;
using OrangeJetpack.Services.Client.Models;

namespace SMAS.Web.Features.Account
{
    [Route("[controller]")]
    public class AccountController : AppBaseController
    {
        private readonly UserService _userService;
        private readonly IMessageService _messageService;
        private readonly UserManager<Entities.User> _userManager;
        private readonly SignInManager<Entities.User> _signInManager;
        
        private readonly ILogger _logger;

        public AccountController(
            IAppServices appServices,
            IOptions<AppSettings> appSettings,
            UserManager<Entities.User> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            SignInManager<Entities.User> signInManager,
            ILoggerFactory loggerFactory,
            ViewRender viewRender) : base(appServices)
        {
            _userService = new UserService(userManager, roleManager, appServices.DataContextFactory);

            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _messageService = appServices.MessageService;
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            var coutries = Countries.GetUpsCountries();
            var model = new RegisterViewModel()
            {
                PhoneNumberViewModel = new PhoneNumberViewModel
                {
                    Countries = coutries,
                    PhoneCountryCode = "+965"
                }
            };

            return View(model);
        }

        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, IFormCollection formCollection)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new Entities.User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneCountryCode = model.PhoneNumberViewModel.PhoneCountryCode,
                PhoneLocalNumber = model.PhoneNumberViewModel.PhoneLocalNumber
            };

            var result = await _userService.CreateUser(user, model.Password);
            if (!result.Succeeded)
            {
                AddErrors(result);
                return View(model);
            }

            await _signInManager.SignInAsync(user, false);
            await EmailNotification.SendNewUserWelcomeEmail(user, AppSettings.SiteTitle, ApplyTemplate);

            _logger.LogInformation(3, "User created a new account with password.");
            return RedirectToLocal(model.ReturnUrl);
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            var model = new LoginViewModel();

            return View(model);
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, IFormCollection formCollection)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result =
                await
                    _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                _logger.LogInformation(1, "User logged in.");
                return RedirectToLocal(model.ReturnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToAction(nameof(SendCode), new { model.ReturnUrl, model.RememberMe });
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning(2, "User account locked out.");
                return View("Lockout");
            }

            SetStatusMessage("We're sorry, the email address or password entered is incorrect.", StatusMessageType.Error);
            return View(model);
        }

        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(Login));
        }

        [Route("edit-account")]
        public async Task<IActionResult> EditAccount()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return RedirectToAction(nameof(Login));
            }
            var model = new EditAccountViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumberViewModel = new PhoneNumberViewModel
                {
                    Countries = Countries.GetAllCountries(),
                    PhoneLocalNumber = user.PhoneLocalNumber,
                    PhoneCountryCode = user.PhoneCountryCode
                }
            };

            return View(model);
        }

        [Route("edit-account")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAccount(EditAccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.PhoneNumberViewModel.Countries = Countries.GetAllCountries();
                return View(model);
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            var userNameChanged = user.UserName != model.Email;
            var originalEmail = user.Email;

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.PhoneLocalNumber = model.PhoneNumberViewModel.PhoneLocalNumber;
            user.PhoneCountryCode = model.PhoneNumberViewModel.PhoneCountryCode;

            await _userManager.UpdateAsync(user);
            if (userNameChanged)
            {
                user.Email += ";" + originalEmail;
                await EmailNotification.SendEmailChangedEmail(user, AppSettings.SiteTitle, ApplyTemplate);
                await _signInManager.SignOutAsync();
                await _signInManager.SignInAsync(user, false);
            }

            SetStatusMessage("Your information have successfully been saved.");
            return RedirectToAction(nameof(EditAccount));
        }

        [Route("need-password")]
        public IActionResult ForgotPassword()
        {
            var model = new NeedPasswordViewModel();
            return View(model);
        }

        [Route("need-password")]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(NeedPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(new NeedPasswordViewModel());
            }

            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                await EmailNotification.SendNoAccountEmail(model.Email, ApplyTemplate);
            }
            else
            {
                await EmailNotification.SendPasswordResetNotification(model.Email, GetPasswordResetUrl(model.Email), ApplyTemplate);
            }


            SetStatusMessage($"A password reset email has been sent to &lt;strong&gt;{model.Email}&lt;/strong&gt;, simply follow the instructions in the email to complete the reset.");
            return RedirectToAction("Login", "Account");
        }

        [Route("reset-password")]
        public IActionResult ResetPassword(UrlTokenParameters tokenParameters)
        {
            string errorMessage;
            if (!ValidateTokenParameters(tokenParameters, out errorMessage))
            {
                return StatusMessage(errorMessage, StatusMessageType.Error);
            }

            var model = new ResetPasswordViewModel
            {
                TokenParameters = tokenParameters
            };

            return View(model);
        }

        [Route("reset-password")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            string errorMessage;
            if (!ValidateTokenParameters(model.TokenParameters, out errorMessage))
            {
                return Error(errorMessage);
            }

            var user = await _userManager.FindByNameAsync(model.TokenParameters.Email);
            if (user == null)
            {
                return Error("We're sorry, we cannot find a user account for this password reset attempt.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, token, model.Password);
            await _signInManager.SignInAsync(user, false);

            SetStatusMessage("Password successfully changed.");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("change-password")]
        [Authorize]
        public IActionResult ChangePassword()
        {
            var model = new ChangePasswordViewModel();
            return View(model);
        }

        [HttpPost("change-password")]
        [Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                SetStatusMessage("We're sorry, but we were unable to change your password. Please be sure you enter your current password correctly and try again.",StatusMessageType.Error);
                return View(model);
            }

            await EmailNotification.SendPasswordChangedEmail(user, AppSettings.SiteTitle, ApplyTemplate);
            SetStatusMessage("Password successfully changed.");
            return RedirectToAction("Index", "Home");
        }


        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet("forgot-password-confirmation")]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet("reset-password-confirmation")]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet("send-code")]
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl = null, bool rememberMe = false)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(user);
            var factorOptions =
                userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return
                View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpPost("send-code")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }

            // Generate the token and send it
            var code = await _userManager.GenerateTwoFactorTokenAsync(user, model.SelectedProvider);
            if (string.IsNullOrWhiteSpace(code))
            {
                return View("Error");
            }

            var message = "Your security code is: " + code;
            if (model.SelectedProvider == "Email")
            {
                //TODO send email activation
                //await _emailSender.SendEmailAsync(await _userManager.GetEmailAsync(user), "Security Code", message);
            }
            else if (model.SelectedProvider == "Phone")
            {
                await SendSms(user, message);
            }

            return RedirectToAction(nameof(VerifyCode),
                new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        [HttpGet("verify-code")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyCode(string provider, bool rememberMe, string returnUrl = null)
        {
            // Require that the user has already logged in via username/password or external login
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpPost("verify-code")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes.
            // If a user enters incorrect codes for a specified amount of time then the user account
            // will be locked out for a specified amount of time.
            var result =
                await
                    _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe,
                        model.RememberBrowser);
            if (result.Succeeded)
            {
                return RedirectToLocal(model.ReturnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning(7, "User account locked out.");
                return View("Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid code.");
                return View(model);
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            //if (Url.IsLocalUrl(returnUrl))
            //{
            //    return Redirect(returnUrl);
            //}

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private string GetPasswordResetUrl(string email)
        {
            var baseUrl = GetBaseUrl("ResetPassword");
            return PasswordUtilities.GenerateResetPasswordUrl(baseUrl, email);
        }

        private string GetBaseUrl(string action)
        {
            var urlHelper = new UrlHelper(ControllerContext);
            return urlHelper.Action(action, "Account", null, "https");
        }

        private static bool ValidateTokenParameters(UrlTokenParameters urlTokenParameters, out string errorMessage)
        {
            try
            {
                PasswordUtilities.ValidateResetPasswordParameters(urlTokenParameters);
            }
            catch (MissingParametersException)
            {
                errorMessage = "We're sorry, the URL that you are attempting to use is missing one or more parameters.";
                return false;
            }
            catch (ExpiredTimestampException)
            {
                errorMessage = "We're sorry, the URL that you are attempting to use to reset your password has expired.";
                return false;
            }
            catch (InvalidTokenException)
            {
                errorMessage =
                    "We're sorry, the URL that you are attempting to use to reset your password is invaluserId.";
                return false;
            }
            errorMessage = null;
            return true;
        }

        private async Task SendSms(Entities.User user, string message)
        {
            var sms = new Sms
            {
                CountryCode = user.PhoneCountryCode,
                LocalNumber = user.PhoneLocalNumber,
                Message = message
            };
            await _messageService.Send(sms);
        }

    }
}
