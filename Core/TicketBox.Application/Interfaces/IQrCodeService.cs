using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBox.Application.Interfaces
{
    public interface IQrCodeService
    {
        string GenerateQrCode(string content);
    }
}
