using Microsoft.EntityFrameworkCore;
using project.Models;

namespace project.DAL
{
    public class CategoryDAL : ICategoryDAL
    {
        private readonly Context context;
        public CategoryDAL(Context contex)
        {
            this.context = contex;
        }
        public async Task<Category> AddCategory(Category category)
        {
            try
            {
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();
                return category;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task DeleteCategory(int id)
        {
            try
            {
                var d = await context.Categories.FirstOrDefaultAsync(dd => dd.Id == id);
                if (d == null)
                {
                    throw new Exception($"Category {id} not found");
                }
                context.Categories.Remove(d);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            try
            {
                return await context.Categories.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
