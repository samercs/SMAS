using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using OrangeJetpack.Base.Core.Security;
using OrangeJetpack.Base.Core.Security.Exceptions;
using OrangeJetpack.Services.Client.Models;
using SMAS.Services.Identity;
using SMAS.Web.Core.Services;
using SMAS.Web.Features.Account.Models;
using SMAS.Web.Features.Home;
using SMAS.Web.Features.Shared;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SMAS.Web.Features.API.Account
{
    [Route("api/account")]
    public class AccountApiController : ApiBaseController
    {
        private readonly UserService _userService;
        private readonly ILogger _logger;
        private readonly UserManager<Entities.User> _userManager;
        private IRazorViewEngine _razorViewEngine;
        public AccountApiController(IAppServices appServices, 
            UserManager<Entities.User> userManager, 
            RoleManager<IdentityRole<int>> roleManager,
            ILoggerFactory loggerFactory,
            IRazorViewEngine razorViewEngine) : base(appServices)
        {
            _userService = new UserService(userManager, roleManager, appServices.DataContextFactory);
            _logger = loggerFactory.CreateLogger<AccountApiController>();
            _userManager = userManager;
            _razorViewEngine = razorViewEngine;
        }

        [HttpPost("register")]
        public async  Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return ModelStateError(ModelState);
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
                return InvalidArgumentError(string.Join(",", result.Errors.Select(i => i.Description)));
            }
            await EmailNotification.SendNewUserWelcomeEmail(user, AppSettings.SiteTitle, ApplyTemplate);
            _logger.LogInformation(3, "User created a new account with password.");
            return Ok(user);
        }

        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword(NeedPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return ModelStateError(ModelState);
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

            return Ok(new { message = "Email for reset instruction has beeb send successfully." });
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

    }

}
