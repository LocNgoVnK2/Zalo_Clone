namespace Zalo_Clone.Models
{
    public class MessageSendModel
    {
        public string Sender { get; set; }

        public string ContactId { get; set; }
        public int? IdMessageSrc { get; set; } = null;
        public string? Content { get; set; }
        public List<string>? AttachmentByBase64 { get; set; }
        
    }
}
