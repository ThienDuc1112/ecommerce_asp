using AutoMapper;
using FinalProject.Models;
using FinalProject.Repositorires.Abstraction;
using FinalProject.Repositorires.Implement;
using FinalProject.ViewModels;
using FinalProject.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCategory>>> GetCategories()
        {
            var categories = await _categoryRepository.GetListCategories();
            if (categories == null)
            {
                return NotFound();
            }
            return Ok(categories);
        }

        [HttpPost]
        public async Task<ActionResult<Status>> Add([FromBody] AddCategory categoryVM)
        {
            Status status = new Status();

            if (!ModelState.IsValid)
            {
                status.IsSuccess = false;
                status.Message = "invalid model";
                return BadRequest(status);
            }
            else
            {
                var check = await _categoryRepository.IsExisted(categoryVM.Name);
                if (check == true)
                {
                    status.IsSuccess = false;
                    status.Message = "this category existed";
                    return BadRequest(status);
                }
                else
                {
                    Category category = _mapper.Map<Category>(categoryVM);
                    await _categoryRepository.Add(category);
                    status.IsSuccess = true;
                    status.Message = "create succesfully";
                    return Ok(status);
                }
            }
        }

        [HttpPut]
        public async Task<ActionResult<Status>> Update([FromBody] UpdateCategory categoryVM)
        {
            Status status = new Status();

            if (!ModelState.IsValid)
            {
                status.IsSuccess = false;
                status.Message = "invalid model";
                return BadRequest(status);
            }
            else
            {
                var category = await _categoryRepository.GetById(categoryVM.Id);
                if (category == null)
                {
                    status.IsSuccess = false;
                    status.Message = $"this category doesn't exist with id: {categoryVM.Id}";
                    return BadRequest(status);
                }
                else
                {
                    _mapper.Map(categoryVM, category);
                    await _categoryRepository.Update(category);
                    status.IsSuccess = true;
                    status.Message = "update succesfully";
                    return Ok(status);
                }
            }
        }


        [HttpPut("Toggle")]
        public async Task<ActionResult> Toggle([FromQuery(Name = "id")] int id)
        {
            var category = await _categoryRepository.GetById(id);
            if (category == null)
            {
                return BadRequest("not found");
            }
            else
            {
                category.IsDeleted = category.IsDeleted == true ? false : true;
                await _categoryRepository.Update(category);

                return NoContent();
            }
        }
    }
}
