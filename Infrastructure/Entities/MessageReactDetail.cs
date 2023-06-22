using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    [Table("MESSAGE_REACT_DETAIL")]
    [PrimaryKey(nameof(ReactId),nameof(MessageId),nameof(UserReact))]
    public class MessageReactDetail
    {

        public int ReactId { get; set; }
        public long MessageId { get; set; }
        public string UserReact { get; set; }


    }
}
