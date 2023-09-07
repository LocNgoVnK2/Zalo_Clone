using Infrastructure.Entities;

namespace Zalo_Clone.Models
{
    public class MessageSendModel
    {
        public string Sender { get; set; }

        public string ContactId { get; set; }
        public int? IdMessageSrc { get; set; } = null;
        public string? Content { get; set; }
        public List<MessageAttachmentModel>? MessageAttachments { get; set; }
        
    }
}
