using FinalProject.Context;
using FinalProject.Models;
using FinalProject.Repositorires.Abstraction;
using FinalProject.ViewModels.Category;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Repositorires.Implement
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _dbContext;
        public CategoryRepository(AppDbContext DbContext) : base(DbContext)
        {
            _dbContext = DbContext;
        }

        public async Task<IEnumerable<GetCategory>> GetListCategories()
        {
            return await _dbContext.Categories.Where(c => c.IsDeleted == false)
                .Select(c => new GetCategory
                {
                    Id = c.Id,
                    Name = c.Name,
                }).AsNoTracking()
                .OrderByDescending(c => c.Id)
                .ToListAsync();
        }

        public async Task<bool> IsExisted(string name)
        {
            return await _dbContext.Categories.AnyAsync(c => c.Name == name);
        }
    }
}
