using System;
using System.Collections.Generic;
using System.Text;

namespace SMAS.Services.PushNotification
{
    public class Notification
    {
        public Notification(string title, string message, string additionalData)
        {
            var tick = Math.Floor(DateTime.UtcNow.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds);
            Data = new Data
            {
                Title = title,
                Message = message,
                ExtraData = additionalData
            };
        }

        public Data Data { get; set; }

        public string RegistrationType { get; set; }
        public string Tags { get; set; }

    }

    public class Data
    {
        public string Message { get; set; }
        public string Title { get; set; }
        public string ExtraData { get; set; }
    }
}
