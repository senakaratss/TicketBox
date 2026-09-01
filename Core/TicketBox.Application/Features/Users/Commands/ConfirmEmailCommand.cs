using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBox.Application.Features.Users.Commands
{
    public class ConfirmEmailCommand:IRequest
    {
        public string Email { get; set; }
        public string ConfirmCode { get; set; }
    }
}
