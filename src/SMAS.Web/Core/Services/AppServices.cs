using Microsoft.Extensions.Options;
using SMAS.Data;
using SMAS.Web.Core.Configuration;
using OrangeJetpack.Core.Web.Utilities;
using OrangeJetpack.Services.Client.Messaging;

namespace SMAS.Web.Core.Services
{
    public class AppServices : IAppServices
    {
        public IDataContextFactory DataContextFactory { get; }
        public ViewRender ViewRender { get; }

        public AppSettings AppSettings { get; }

        public IMessageService MessageService { get; }

        public AppServices()
        {
            
        }

        public AppServices(
            IOptions<AppSettings> appSettings,
            IDataContextFactory dataContextFactory,
            IMessageService messageService,
            ViewRender viewRender)
        {
            AppSettings = appSettings.Value;
            DataContextFactory = dataContextFactory;
            MessageService = messageService;
            ViewRender = viewRender;
        }

        
    }
}
