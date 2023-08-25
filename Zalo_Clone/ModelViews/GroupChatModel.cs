using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zalo_Clone.Models
{
    public class GroupChatModel
    {
            public string? idGroup { get; set; }=null;
            public string Name { get; set; }
            [NotMapped]
            public string? imageByBase64 { get; set; }
            public string Leader { get; set; }

    }
}
