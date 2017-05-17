using OrangeJetpack.Services.Client.Messaging;

namespace SMAS.Web.Core.Configuration
{
    public class AppSettings
    {
        public string SiteTitle { get; set; }

        public string ProjectKey { get; set; }

        public string ProjectToken { get; set; }

        public string EmailSender { get; set; }
        public string ApiKey { get; set; }
        public string NotificationHubName { get; set; }
        public string NotificationHubConnectionString { get; set; }


        public EmailSettings EmailSettings => new EmailSettings
        {
            ProjectKey = ProjectKey,
            ProjectToken = ProjectToken,
            SenderAddress = EmailSender,
            SenderName = SiteTitle
        };
    }
}
