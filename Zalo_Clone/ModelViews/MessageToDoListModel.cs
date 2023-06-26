namespace Zalo_Clone.ModelViews
{
    public class MessageToDoListModel
    {

            public string Sender { get; set; }
            public long TaskId { get; set; }
            public DateTime SendTime { get; set; }
            public int Status { get; set; }
            public int? IdMessageSrc { get; set; }
            public string? Content { get; set; }
            public List<string>? AttachmentByBase64 { get; set; }

    }
}
