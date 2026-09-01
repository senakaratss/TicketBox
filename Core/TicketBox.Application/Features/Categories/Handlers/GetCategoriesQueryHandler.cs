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
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<GetCategoriesQueryResult>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<GetCategoriesQueryResult>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(x => new GetCategoriesQueryResult
            {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName,
                Icon=x.Icon,
                EventsCount=x.Events.Count
            }).ToList();
        }
    }
}
