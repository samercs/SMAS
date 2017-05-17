using System.Threading.Tasks;
using OrangeJetpack.Services.Client.Models;
using Email = OrangeJetpack.Services.Client.Models.Email;
using EmailResponse = OrangeJetpack.Services.Client.Models.EmailResponse;

namespace OrangeJetpack.Services.Client.Messaging
{
    public interface IMessageService
    {
        Task<EmailResponse> Send(Email email);
        Task<SmsResponse> Send(Sms sms);
    }
}
