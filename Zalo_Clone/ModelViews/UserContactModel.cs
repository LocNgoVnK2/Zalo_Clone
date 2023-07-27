namespace Zalo_Clone.Models
{
    public class UserContactModel
    {
       public string Id { get; set; }
        public string ContactName { get; set; }
        public byte[]? Avatar { get; set; }
        public string? LastMessageContent { get; set; }

    }
}
