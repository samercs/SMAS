using OrangeJetpack.Base.Core.Formatting;

namespace OrangeJetpack.Services.Client.Models
{
    public class Sms
    {
        public string CountryCode { get; set; }
        public string LocalNumber { get; set; }
        public string Message { get; set; }

        public string PhoneNumber
        {
            get
            {
                var phoneNumber = StringFormatter.StripNonDigits(CountryCode + LocalNumber);
                if (!phoneNumber.StartsWith("+"))
                {
                    phoneNumber = "+" + phoneNumber;
                }

                return phoneNumber;
            }
        }
    }
}
