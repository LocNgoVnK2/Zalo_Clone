using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities
{
    [Table("USER_CONTACT")]
    [PrimaryKey(nameof(UserId), nameof(OtherUserId))]
    public class UserContact
    {
        public string UserId { get; set; }
        public string? OtherUserId { get; set; }

        public long LastMessageId { get; set; }
        [NotMapped]
        public string? LastMessageContent { get; set; }
         [NotMapped]
        public DateTime? LastMessageTime { get; set; }
         [NotMapped]
        public string? GroupContactId { get; set; } 
    }
}
