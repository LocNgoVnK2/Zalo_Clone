namespace Zalo_Clone.Models
{
    public class MessageReceipentModel
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public DateTime SendTime { get; set; }
        public int Status { get; set; }
        public int? IdMessageSrc { get; set; } = null;
        public string? Content { get; set; }
        public List<string>? AttachmentByBase64 { get; set; }
        
    }
}
