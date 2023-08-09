using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    [Table("SEARCH_LOG")]
    public class SearchLog
    {
        [Key]

        [Column("Id")]

        public long? Id { get; set; }

        [Column("UserSrc")]
        public string UserSrcId { get; set; }

        [Column("UserDes")]
        public string UserDesId { get; set; }
        [Column("AtTime")]
        public DateTime AtTime { get; set; }



    }
}
