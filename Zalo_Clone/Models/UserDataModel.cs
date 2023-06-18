namespace Zalo_Clone.Models
{
    public class UserDataModel
    {
        public int Id { get; set; }

        public string Email { get; set; }
        public string Username { get; set; }
        public byte[] Avatar { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] Background { get; set; }
    }
}
