namespace Zalo_Clone.Models
{
    public class UserInformationModel
    {
        public string Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; } 
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public string? Avatar { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Background { get; set; }
    }
}
