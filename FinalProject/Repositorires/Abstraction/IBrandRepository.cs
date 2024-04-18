using FinalProject.Models;
using FinalProject.Repositorires.Implement;
using FinalProject.ViewModels.Brand;

namespace FinalProject.Repositorires.Abstraction
{
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        Task<bool> IsExisted(string name);
        Task<IEnumerable<GetBrand>> GetListBrands();
    }
}
