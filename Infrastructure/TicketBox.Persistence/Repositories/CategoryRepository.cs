using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBox.Application.Interfaces;
using TicketBox.Domain.Entities;
using TicketBox.Persistence.Context;

namespace TicketBox.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly TicketContext _context;

        public CategoryRepository(TicketContext context)
        {
            _context = context;
        }

        public async Task CreateCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var value = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(value);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.Include(x=>x.Events).ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(x=>x.CategoryId==id);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
