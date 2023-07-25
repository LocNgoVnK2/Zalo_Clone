using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
namespace Infrastructure.Entities
{
    public class EmailMessage
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; } 
        public string Content { get; set; }
        public EmailMessage(IEnumerable<string> to, string subject, string content) {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x=> new MailboxAddress("Zalo Clone",x)));
            Subject = subject;
            Content = content;
        }
            public EmailMessage(string to, string subject, string content) {
            To = new List<MailboxAddress>
            {
                new MailboxAddress("Zalo Clone", to)
            };
            Subject = subject;
            Content = content;
        }
    }
}
