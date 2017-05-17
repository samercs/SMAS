using System;
using System.Text;
using Newtonsoft.Json;

namespace SMAS.Services.PushNotification
{
    public class Registration
    {
        public Registration()
        { }

        public Registration(Registration other)
        {
            RegistrationType = other.RegistrationType;
            Tags = other.Tags;
        }

        public string RegistrationId { get; set; }
        public string OldRegistrationId { get; set; }

        public string RegistrationType { get; set; }


        public string Tags { get; set; }


    }

    public enum RegistrationType
    {
        Windows,
        Apple,
        Gcm,
    }
}
