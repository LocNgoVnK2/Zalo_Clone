﻿namespace Zalo_Clone.Models
{
    public class MessageContactModel
    {
        public string Sender { get; set; }
        public string ContactId { get; set; }
        public DateTime SendTime { get; set; }
        public int Status { get; set; }
        public int? IdMessageSrc { get; set; } = null;
        public string? Content { get; set; }
        public List<string>? AttachmentByBase64 { get; set; }
        
    }
}