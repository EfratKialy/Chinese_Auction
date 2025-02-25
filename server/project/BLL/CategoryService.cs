using project.DAL;
using project.Models;

namespace project.BLL
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryDAL categoryDAL;
        public CategoryService(ICategoryDAL categoryDal)
        {
            this.categoryDAL = categoryDal;
        }
        public async Task<Category> AddCategory(Category category)
        {
            return await categoryDAL.AddCategory(category);
        }

        public async Task DeleteCategory(int id)
        {
            await categoryDAL.DeleteCategory(id);
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await categoryDAL.GetCategories();
        }
    }
}
