using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using OrangeJetpack.Services.Client.Models;
using Email = OrangeJetpack.Services.Client.Models.Email;
using EmailResponse = OrangeJetpack.Services.Client.Models.EmailResponse;

namespace OrangeJetpack.Services.Client.Messaging
{
    public class MessageService : IMessageService
    {
        private readonly HttpClient _httpClient;
        private readonly string _senderAddress;
        private readonly string _senderName;

        public MessageService(string projectKey, string projectToken, string senderAddress = null, string senderName = null)
        {
            _httpClient = HttpClientFactory.Create(projectKey, projectToken);
            _senderAddress = senderAddress;
            _senderName = senderName;
        }

        public MessageService(EmailSettings emailSettings)
        {
            _httpClient = HttpClientFactory.Create(emailSettings.ProjectKey, emailSettings.ProjectToken);
            _senderAddress = emailSettings.SenderAddress;
            _senderName = emailSettings.SenderName;
        }

        public async Task<EmailResponse> Send(Email email)
        {
            email.FromAddress = email.FromAddress ?? _senderAddress;
            email.FromName = email.FromName ?? _senderName;

            var response = await _httpClient.PostAsync("messages/email", new JsonContent(email));
            var stream = await response.Content.ReadAsStreamAsync();
            var serializer = new DataContractJsonSerializer(typeof(List<EmailResponse>));
            var result = (List<EmailResponse>)serializer.ReadObject(stream);
            return result != null && result.Any() ? result.First() : null;
        }

        public async Task<SmsResponse> Send(Sms sms)
        {
            var response = await _httpClient.PostAsync("messages/sms", new JsonContent(sms));
            return new SmsResponse
            {
                IsSuccess = response.IsSuccessStatusCode
            };
        }
    }
}
