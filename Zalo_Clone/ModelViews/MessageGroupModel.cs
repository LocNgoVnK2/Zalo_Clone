namespace Zalo_Clone.Models
{
    public class MessageGroupModel
    {
        public string Sender { get; set; }
        public string GroupReceive { get; set; }
        public DateTime SendTime { get; set; }
        public int Status { get; set; }
        public int? IdMessageSrc { get; set; }
        public string? Content { get; set; }
        public List<string>? AttachmentByBase64 { get; set; }
        
    }
}
