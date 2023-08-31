using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    [Table("MESSAGE_ATTACHMENT")]
    public class MessageAttachment
    {
        [Key]
        public long Id { get; set; }
        public long IdMessage { get; set; }
        public byte[] Attachment { get; set; }
        public string FileName { get; set; }
        public string FileType {get ; set;}
        [NotMapped]
        public string AttachmentByBase64 { get; set; }
    }
}
