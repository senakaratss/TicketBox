using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.Features.Categories.Queries;
using TicketBox.Application.Features.Categories.Results;
using TicketBox.Application.Interfaces;

namespace TicketBox.Application.Features.Categories.Handlers
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, GetCategoryByIdQueryResult>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<GetCategoryByIdQueryResult> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var value = await _categoryRepository.GetByIdAsync(request.CategoryId);
            return new GetCategoryByIdQueryResult
            {
                CategoryId = value.CategoryId,
                CategoryName = value.CategoryName,
                Icon=value.Icon
            };

        }
    }
}
