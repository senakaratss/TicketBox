using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.Features.Users.Queries;
using TicketBox.Application.Features.Users.Results;
using TicketBox.Application.Interfaces;

namespace TicketBox.Application.Features.Users.Handlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdQueryResult>
    {
        private readonly IIdentityService _identityService;

        public GetUserByIdQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<GetUserByIdQueryResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
           return await _identityService.GetUserByIdAsync(request.Id);
        }
    }
}
