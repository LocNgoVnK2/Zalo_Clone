
using Zalo_Clone.Models;
namespace Zalo_Clone.Models
{
    public class MessageContactModel
    {
        public long Id {get ; set;}
        public string Sender { get; set; }
        public string SenderName { get; set; }

        public string ContactId { get; set; }
        public string ContactName { get; set; }
        public DateTime SendTime { get; set; }
        public int Status { get; set; }
        public int? IdMessageSrc { get; set; } = null;
        public string? Content { get; set; }
        public List<MessageAttachmentModel>? MessageAttachments { get; set; }
        
    }
}
