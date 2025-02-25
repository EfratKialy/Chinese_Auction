using Microsoft.AspNetCore.Mvc;
using project.Models;

namespace project.DAL
{
    public interface ICategoryDAL
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<Category> AddCategory(Category category);
        Task DeleteCategory(int id);
    }
}
