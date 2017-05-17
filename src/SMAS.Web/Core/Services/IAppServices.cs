using SMAS.Data;
using SMAS.Web.Core.Configuration;
using OrangeJetpack.Core.Web.Utilities;
using OrangeJetpack.Services.Client.Messaging;

namespace SMAS.Web.Core.Services
{
    public interface IAppServices
    {
        IDataContextFactory DataContextFactory { get; }

        AppSettings AppSettings { get; }
        
        IMessageService MessageService { get; }
        ViewRender ViewRender { get; }
    }
}
