using System;
using System.Threading;
using System.Globalization;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SMAS.Data;
using SMAS.Notifications;
using SMAS.Services;
using SMAS.Services.Identity;
using SMAS.Web.Core.Configuration;
using SMAS.Web.Core.Services;
using OrangeJetpack.Core.Web.UI;
using OrangeJetpack.Core.Web.Utilities;
using OrangeJetpack.Services.Client.Messaging;
using OrangeJetpack.Services.Client.Models;

namespace SMAS.Web.Features.Shared
{
    public class AppBaseController : Controller
    {
        protected string LanguageCode { get; set; }
        protected readonly IAppServices AppServices;
        protected readonly ViewRender ViewRender;

        protected readonly IDataContextFactory DataContextFactory;
        protected readonly IMessageService MessageService;

        protected readonly AppSettings AppSettings;
        protected readonly EmailTemplateService EmailTemplateService;
        protected readonly EmailNotification EmailNotification;

        protected AppBaseController(IAppServices appServices)
        {
            AppServices = appServices;
            ViewRender = appServices.ViewRender;
            DataContextFactory = appServices.DataContextFactory;
            
            AppSettings = appServices.AppSettings;
            MessageService = appServices.MessageService;

            EmailTemplateService = new EmailTemplateService(DataContextFactory);
            EmailNotification = new EmailNotification(appServices.DataContextFactory,MessageService);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LanguageCode = ViewBag.LanguageCode = GetLanguageCode();
            base.OnActionExecuting(filterContext);
        }

        private static string GetLanguageCode()
        {
            var languageCode = "en";
            if (CultureInfo.CurrentCulture.Name.StartsWith("ar", StringComparison.OrdinalIgnoreCase))
            {
                languageCode = "ar";
            }

            return languageCode;
        }

        /// <summary>
        /// Sets a view status message for display.
        /// </summary>
        /// <param name="message">The message text to display.</param>
        /// <param name="statusMessageType">The <see cref="StatusMessageType"/> to use.</param>
        protected void SetStatusMessage(string message, StatusMessageType statusMessageType = StatusMessageType.Success)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            TempData["StatusMessage.Message"] = message;
            TempData["StatusMessage.Type"] = statusMessageType;
        }

        /// <summary>
        /// Gets a status message if one is set.
        /// </summary>
        protected StatusMessage GetStatusMessage()
        {
            return TempData["StatusMessage"] as StatusMessage;
        }

        /// <summary>
        /// Gets a blank view with a status message.
        /// </summary>
        /// <param name="message">The message text to display.</param>
        /// <param name="statusMessageType">The <see cref="StatusMessageType"/> to use.</param>
        protected ActionResult StatusMessage(string message, StatusMessageType statusMessageType = StatusMessageType.Success)
        {
            SetStatusMessage(message, statusMessageType);
            return View("Blank");
        }

        /// <summary>
        /// Gets a ViewResult for Error with the supplied error message applied.
        /// </summary>
        protected ActionResult Error(string errorMessage)
        {
            SetStatusMessage(errorMessage, StatusMessageType.Error);
            return View("Blank");
        }

        protected Email ApplyTemplate(Email email)
        {
            ViewRender.ActionContext = ControllerContext;
            var messagWithTemplate = ViewRender.GetPartialViewAsString("EmailTemplate", email);
            email.Message = messagWithTemplate;

            return email;
        }

        protected IActionResult Confirmation(string pageTitle, string confirmationMessage)
        {
            return View("Confirmation", new ConfirmationViewModel
            {
                PageTitle = pageTitle,
                ConfirmationMessage = confirmationMessage
            });
        }

        protected JsonResult JsonError(string error, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            Response.StatusCode = (int)statusCode;
            return Json(new
            {
                success = false,
                error
            });
        }

        protected JsonResult KendoJson(object data)
        {
            return Json(data, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None
            });
        }
    }
}
