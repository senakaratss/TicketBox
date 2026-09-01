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
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<GetUsersQueryResult>>
    {
        private readonly IIdentityService _identityService;

        public GetUsersQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<List<GetUsersQueryResult>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return await _identityService.GetAllUsersAsync();
        }
    }
}
