namespace Zalo_Clone.Models
{
    public class UserDataModel
    {
        public string Id { get; set; }

        public byte[]? Avatar { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public byte[]? Background { get; set; }
    }
}
