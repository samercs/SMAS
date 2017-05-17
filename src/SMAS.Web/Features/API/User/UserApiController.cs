using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrangeJetpack.Localization;
using SMAS.Core.Identity;
using SMAS.Entities;
using SMAS.Models;
using SMAS.Services;
using SMAS.Services.Identity;
using SMAS.Web.Core.Services;
using SMAS.Web.Features.Account.Models;
using SMAS.Web.Features.API.User.Model;
using SMAS.Web.Features.Shared;
using ChangePasswordViewModel = SMAS.Web.Models.ManageViewModels.ChangePasswordViewModel;

namespace SMAS.Web.Features.API.User
{
    [Route("api/user")]
    public class UserApiController : ApiBaseController
    {
        private readonly UserManager<Entities.User> _userManager;
        private readonly UserService _userService;
        private readonly ILogger _logger;
        public UserApiController(IAppServices appServices, UserManager<Entities.User> userManager, RoleManager<IdentityRole<int>> roleManager, ILoggerFactory loggerFactory) : base(appServices)
        {
            _userManager = userManager;
            _userService = new UserService(userManager, roleManager, DataContextFactory);
            _logger = loggerFactory.CreateLogger<UserApiController>();
        }

        [HttpGet("current/profile")]
        [Authorize(ActiveAuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetProfile()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return Ok(UserViewModel.Create(user));
        }

        

        [HttpPut("current/profile")]
        [Authorize(ActiveAuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateProfile(EditAccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return ModelStateError(ModelState);
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var userNameChanged = user.UserName != model.Email;
            var originalEmail = user.Email;

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneLocalNumber = model.PhoneNumberViewModel.PhoneLocalNumber;
            user.PhoneCountryCode = model.PhoneNumberViewModel.PhoneCountryCode;
            user.Email = model.Email;
            user.UserName = model.Email;

            await _userManager.UpdateAsync(user);
            if (userNameChanged)
            {
                user.Email += ";" + originalEmail;
                await EmailNotification.SendEmailChangedEmail(user, AppSettings.SiteTitle, ApplyTemplate);
            }

            return Ok("User profile has been updated successfully.");
        }

        [HttpPut("current/change-password")]
        [Authorize(ActiveAuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return ModelStateError(ModelState);
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    
                    _logger.LogInformation(3, "User changed their password successfully.");
                    return Ok("Change Password Success");
                }
                return InvalidArgumentError("Can't chage user password. Please check the old password.");
            }
            return Unauthorized();

        }
    }
}
