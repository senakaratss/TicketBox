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
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, bool>
    {
        private readonly IIdentityService _identityService;

        public UpdateProfileCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<bool> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var dto = new UpdateProfileDto
            {
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                Phone = request.Phone,
                Username = request.Username
            };
            return await _identityService.UpdateProfileAsync(dto);
        }
    }
}
