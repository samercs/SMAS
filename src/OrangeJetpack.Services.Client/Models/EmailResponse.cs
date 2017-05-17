using System;
using System.Collections.Generic;
using ResponseBase = OrangeJetpack.Services.Client.Messaging.ResponseBase;

namespace OrangeJetpack.Services.Client.Models
{
    public class EmailResponse : ResponseBase
    {
        public DateTime SendDate { get; set; }
        public List<string> ErrorEmails { get; set; }
        public List<string> ErrorMessages { get; set; }

        public EmailResponse()
        {
            ErrorEmails = new List<string>();
            ErrorMessages = new List<string>();
        }
    }
}
