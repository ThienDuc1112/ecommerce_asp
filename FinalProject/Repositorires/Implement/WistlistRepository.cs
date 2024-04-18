using FinalProject.Context;
using FinalProject.Models;
using FinalProject.Repositorires.Abstraction;
using FinalProject.ViewModels.Whislist;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Repositorires.Implement
{
    public class WistlistRepository : GenericRepository<Whislist>, IWistlistRepository
    {
        private readonly AppDbContext _dbContext;
        public WistlistRepository(AppDbContext DbContext) : base(DbContext)
        {
            _dbContext = DbContext;
        }

        public async Task<IEnumerable<GetWistlist>> GetWhislists(string userId)
        {
            return await _dbContext.Wishlists
                .Where(x => x.UserId == userId)
                .Select(x => new GetWistlist
                {
                    Id = x.Id,
                    Brand = x.Product.Brand.Name,
                    Category = x.Product.Category.Name,
                    ProductImage = x.Product.Image,
                    Title = x.Product.Title
                }).AsNoTracking().ToListAsync();
        }

        public async Task<bool> IsExisted(string userId, int productId)
        {
            return await _dbContext.Wishlists
                .AnyAsync(w => w.UserId == userId && w.ProductId == productId);
        }
    }
}
