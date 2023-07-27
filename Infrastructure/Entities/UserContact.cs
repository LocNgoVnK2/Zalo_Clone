using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities
{
    [Table("USER_CONTACT")]
    [PrimaryKey(nameof(UserId), nameof(ContactId))]
    public class UserContact
    {
        public string UserId { get; set; }
        public string ContactId { get; set; }
        [NotMapped]

        public Message? LastMessage { get; set; }
    }
}
