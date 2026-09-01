using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.DTOs;
using TicketBox.Domain.Entities;

namespace TicketBox.Application.Interfaces
{
    public interface ITicketImageService
    {
        Task<byte[]> GenerateTicketImage(TicketImageDto ticket);
    }
}
