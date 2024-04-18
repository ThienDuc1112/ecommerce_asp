using FinalProject.Context;
using FinalProject.Models;
using FinalProject.Repositorires.Abstraction;
using FinalProject.ViewModels.Brand;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Repositorires.Implement
{
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        private readonly AppDbContext _dbContext;
        public BrandRepository(AppDbContext DbContext) : base(DbContext)
        {
            _dbContext = DbContext;
        }

        public async Task<IEnumerable<GetBrand>> GetListBrands()
        {
            return await _dbContext.Brands.Where(b => b.IsDeleted == false)
                .Select(b => new GetBrand
                {
                    Id = b.Id,
                    Name = b.Name
                }).AsNoTracking()
                 .OrderByDescending(c => c.Id)
                .ToListAsync();
        }

        public async Task<bool> IsExisted(string name)
        {
            return await _dbContext.Brands.AnyAsync(b => b.Name == name);
        }
    }
}
