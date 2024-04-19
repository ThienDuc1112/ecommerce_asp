using FinalProject.Models;
using FinalProject.ViewModels.Brand;
using FinalProject.ViewModels.Product;

namespace FinalProject.Repositorires.Abstraction
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<bool> IsExisted(string name);
        Task<DisplayingProduct> GetListProduct(int page, string query);
        Task<GetDetailProduct> GetDetailProduct(int id);
        Task<IEnumerable<Product>> GetNewestProducts();
        Task<DisplayingAdminProduct> GetAdminListProduct(int page, string query);
        Task<Product> GetProductWithRelevant(int id);
    }
}
