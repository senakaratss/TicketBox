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
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly IIdentityService _identityService;

        public RegisterUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (request.Password != request.ConfirmPassword)
            {
                throw new Exception("Password do not match");
            }
            var user = new RegisterUserDto
            {
                Name = request.Name,
                Surname = request.Surname,
                Username = request.Username,
                Phone = request.Phone,
                Email = request.Email,
                Password = request.Password
            };

            return await _identityService.RegisterAsync(user);
        }
    }
}
