namespace SMAS.Services.PushNotification
{
    public class Message : Registration
    {
        public Message()
        {
        }

        public Message(Registration registration)
            : base(registration)
        {
        }


        public string Text { get; set; }
    }
}
