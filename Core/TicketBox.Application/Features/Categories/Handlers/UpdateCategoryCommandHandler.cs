using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.Features.Categories.Commands;
using TicketBox.Application.Interfaces;
using TicketBox.Domain.Entities;

namespace TicketBox.Application.Features.Categories.Handlers
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (category == null)
            {
                throw new Exception("Category bulunamadı");
            }
            category.CategoryName = request.CategoryName;
            category.Icon = request.Icon;
            await _categoryRepository.UpdateCategoryAsync(category);
        }
    }
}
