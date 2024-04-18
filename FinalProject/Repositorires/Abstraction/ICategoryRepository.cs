using FinalProject.Models;
using FinalProject.ViewModels.Category;

namespace FinalProject.Repositorires.Abstraction
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<bool> IsExisted(string name);
        Task<IEnumerable<GetCategory>> GetListCategories();
    }
}
