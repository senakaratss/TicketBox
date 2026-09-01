using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.Interfaces;

namespace TicketBox.Persistence.Repositories
{
    public class QrCodeService : IQrCodeService
    {
        public string GenerateQrCode(string content)
        {
            using var qrGenerator = new QRCodeGenerator();

            using var qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);

            using var qrCode = new PngByteQRCode(qrCodeData);

            byte[] qrCodeBytes = qrCode.GetGraphic(10);

            return Convert.ToBase64String(qrCodeBytes);
        }
    }
}
