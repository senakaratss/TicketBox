using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.DTOs;
using TicketBox.Application.Features.Users.Commands;
using TicketBox.Application.Interfaces;

namespace TicketBox.Application.Features.Users.Handlers
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly IIdentityService _identityService;

        public ChangePasswordCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.NewPassword != request.ConfirmNewPassword) return false;
           
            var dto = new ChangePasswordDto
            {
                CurrentPassword = request.CurrentPassword,
                NewPassword = request.NewPassword
            };

            return await _identityService.ChangePasswordAsync(dto);
        }
    }
}
