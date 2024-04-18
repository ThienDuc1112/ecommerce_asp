using FinalProject.Context;
using FinalProject.Models;
using FinalProject.Repositorires.Abstraction;
using FinalProject.ViewModels.Product;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Repositorires.Implement
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {

        private readonly AppDbContext _dbContext;
        public ProductRepository(AppDbContext DbContext) : base(DbContext)
        {
            _dbContext = DbContext;
        }

        public async Task<GetDetailProduct> GetDetailProduct(int id)
        {
            return await _dbContext.Products
                .Select(p => new GetDetailProduct
                {
                    Id = p.Id,
                    BrandName = p.Brand.Name,
                    CategoryName = p.Category.Name,
                    Decription = p.Decription,
                    Image = p.Image,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    Title = p.Title
                }).AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<DisplayingProduct> GetListProduct(int page, string query)
        {
            int pageSize = 8;
            int itemsToSkip = (int)(page - 1) * pageSize;
            IQueryable<Product> productQuery = _dbContext.Products;

            if (!string.IsNullOrEmpty(query))
            {
                productQuery = productQuery.Where(x => x.Title.Contains(query));
            }

            var products = await productQuery.Select(p => new GetProduct
            {
                Id = p.Id,
                BrandName = p.Brand.Name,
                CategoryName = p.Category.Name,
                Image = p.Image,
                Price = p.Price,
                Title = p.Title
            }).AsNoTracking()
            .OrderByDescending(p => p.Id)
            .ToListAsync();

            int total = await productQuery.CountAsync();

            return new DisplayingProduct { Products = products, TotalProduct = total };
        }

        public async Task<bool> IsExisted(string name)
        {
            return await _dbContext.Products.AnyAsync(p => p.Title == name);
        }
    }
}
