using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.Features.Users.Commands;
using TicketBox.Application.Interfaces;

namespace TicketBox.Application.Features.Users.Handlers
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand>
    {
        private readonly IIdentityService _identityService;

        public ConfirmEmailCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            await _identityService.ConfirmEmail(request.Email, request.ConfirmCode);
        }
    }
}
