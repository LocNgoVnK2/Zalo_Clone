using System.ComponentModel.DataAnnotations.Schema;

namespace Zalo_Clone.Models
{
    public class FriendRequestModel
    {

            [Column("user_1")]
            public string User1 { get; set; }

            [Column("user_2")]
            public string User2 { get; set; }
            public string? UserName { get; set; }
            public string? Email { get; set; }

            public string? Avatar { get; set; }

            [Column("requestDate")]
            public DateTime RequestDate { get; set; }

            [Column("acceptDate")]
            public DateTime? AcceptDate { get; set; }
        
    }
}
