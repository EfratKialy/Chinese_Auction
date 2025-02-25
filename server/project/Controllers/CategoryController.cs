using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project.BLL;
using project.Models;
using project.Models.DTO;

namespace project.Controllers
{
    [ApiController]
    [Route("category/api")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            this.categoryService = categoryService;
            this._mapper = mapper;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            try
            {
                return Ok(await categoryService.GetCategories());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<Category>> AddCategory(CategoryDTO categoryDto)
        {
            try
            {
                var c = _mapper.Map<Category>(categoryDto);
                return Ok(await categoryService.AddCategory(c));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        [HttpDelete]
        [Authorize(Roles = "Manager")]
        public async Task DeleteCategory(int id)
        {
           await categoryService.DeleteCategory(id);
        }
    }
}
