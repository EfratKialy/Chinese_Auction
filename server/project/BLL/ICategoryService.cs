using project.Models;

namespace project.BLL
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<Category> AddCategory(Category category);
        Task DeleteCategory(int id);
    }
}
