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
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, bool>
    {
        private readonly IIdentityService _identityService;

        public LoginUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<bool> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var dto = new LoginUserDto
            {
                Username = request.Username,
                Password = request.Password
            };
            return await _identityService.LoginAsync(dto);
        }
    }
}
