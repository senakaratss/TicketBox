using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBox.Application.DTOs
{
    public class EmailAttachmentDto
    {
        public byte[] Content { get; set; }
        public string FileName { get; set; }
    }
}
