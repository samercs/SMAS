namespace OrangeJetpack.Core.Web.UI
{
    public class StatusMessage
    {
        public string Message { get; set; }
        public StatusMessageType Type { get; set; }
        
        public StatusMessage(string message, StatusMessageType type = StatusMessageType.Success)
        {
            Message = message;
            Type = type;
        }
    }
}
