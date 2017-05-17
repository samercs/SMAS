using SMAS.Data;
using SMAS.Entities;
using SMAS.Entities.Enum;
using SMAS.Services;
using OrangeJetpack.Services.Client.Messaging;
using OrangeJetpack.Services.Client.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMAS.Notifications
{
    public class EmailNotification
    {
        private readonly EmailTemplateService _emailTemplateService;
        private readonly IMessageService _messageService;

        public EmailNotification(IDataContextFactory dataContextFactory, IMessageService messageService)
        {
            _emailTemplateService = new EmailTemplateService(dataContextFactory);
            _messageService = messageService;
        }

        public async Task SendNewUserWelcomeEmail(User user, string siteTitle, Func<Email, Email> applyTemplate)
        {
            var template = await _emailTemplateService.GetByTemplateType(EmailTemplateType.AccountRegistration);

            var email = new Email
            {
                ToAddress = user.Email,
                Subject = template.Subject.ReplaceParameters(new Dictionary<string, string>
                {
                    {"{SiteTitle}", siteTitle}
                }),
                Message = template.Message.ReplaceParameters(new Dictionary<string, string>
                {
                    {"{SiteTitle}", siteTitle},
                    {"{User.FirstName}", user.FirstName }
                })
            };

            applyTemplate(email);
            await Send(email);
        }

        public async Task SendEmailChangedEmail(User user, string siteTitle, Func<Email, Email> applyTemplate)
        {
            var template = await _emailTemplateService.GetByTemplateType(EmailTemplateType.EmailChanged);

            var email = new Email
            {
                ToAddress = user.Email,
                Subject = template.Subject,
                Message = template.Message.ReplaceParameters(new Dictionary<string, string>
                {
                    {"{User.FirstName}", user.FirstName},
                    {"{SiteTitle}", siteTitle}
                })
            };

            applyTemplate(email);
            await Send(email);
        }

        public async Task SendNoAccountEmail(string emailAddress, Func<Email, Email> applyTemplate)
        {
            var template = await _emailTemplateService.GetByTemplateType(EmailTemplateType.NoAccount);

            var email = new Email
            {
                ToAddress = emailAddress,
                Subject = template.Subject,
                Message = template.Message.ReplaceParameters(new Dictionary<string, string>
                {
                    {"{EmailAddress}", emailAddress}
                })
            };

            applyTemplate(email);
            await Send(email);
        }

        public async Task SendPasswordResetNotification(string emailAddress, string callBackUrl, Func<Email, Email> applyTemplate)
        {
            var template = await _emailTemplateService.GetByTemplateType(EmailTemplateType.PasswordReset);

            var email = new Email
            {
                ToAddress = emailAddress,
                Subject = template.Subject,
                Message = template.Message.ReplaceParameters(new Dictionary<string, string>
                {
                    {"{User.EmailAddress}", emailAddress},
                    {"{ResetPasswordUrl}", callBackUrl}
                })
            };

            applyTemplate?.Invoke(email);

            await Send(email);
        }

        public async Task SendPasswordChangedEmail(User user, string siteTitle, Func<Email,Email> applyTemplate )
        {
            var template = await _emailTemplateService.GetByTemplateType(EmailTemplateType.PasswordChanged);

            var email = new Email
            {
                ToAddress = user.Email,
                Subject = template.Subject,
                Message = template.Message.ReplaceParameters(new Dictionary<string, string>
                {
                    {"{User.FirstName}", user.FirstName},
                    {"{SiteTitle}", siteTitle}
                })
            };

            applyTemplate(email);
            await Send(email);
        }

        private async Task Send(Email email)
        {
            try
            {
                await _messageService.Send(email);
            }
            catch (Exception)
            {
                //todo Add logger
                throw;
            }
        }
    }
}
