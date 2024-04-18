using AutoMapper;
using FinalProject.Models;
using FinalProject.Repositorires.Abstraction;
using FinalProject.ViewModels;
using FinalProject.ViewModels.Brand;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BrandController : Controller
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public BrandController(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetBrand>>> GetCategories()
        {
            var brands = await _brandRepository.GetListBrands();
            if(brands == null)
            {
                return NotFound();
            }
            return Ok(brands);
        }

        [HttpGet("Detail")]
        public async Task<ActionResult<Brand>> GetDetail([FromQuery(Name ="id")] int id)
        {
            var brand = await _brandRepository.GetById(id);
            if(brand == null)
            {
                return NotFound();
            }

            return Ok(brand);
        }

        [HttpPost]
        public async Task<ActionResult<Status>> Add([FromBody] AddBrand brandVM)
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
                var check = await _brandRepository.IsExisted(brandVM.Name);
                if(check == true)
                {
                    status.IsSuccess = false;
                    status.Message = "this brand existed";
                    return BadRequest(status);
                }
                else
                {
                    Brand brand = _mapper.Map<Brand>(brandVM);
                    await _brandRepository.Add(brand);
                    status.IsSuccess = true;
                    status.Message = "create succesfully";
                    return Ok(status);
                }
            }
        }

        [HttpPut]
        public async Task<ActionResult<Status>> Update([FromBody] UpdateBrand brandVM)
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
                var brand = await _brandRepository.GetById(brandVM.Id);
                if (brand == null)
                {
                    status.IsSuccess = false;
                    status.Message = $"this brand doesn't exist with id: {brandVM.Id}";
                    return BadRequest(status);
                }
                else
                {
                    _mapper.Map(brandVM, brand);
                    await _brandRepository.Update(brand);
                    status.IsSuccess = true;
                    status.Message = "update succesfully";
                    return Ok(status);
                }
            }
        }

        [HttpPut("Toggle")]
        public async Task<ActionResult> Toggle([FromQuery(Name = "id")] int id)
        {
            var brand = await _brandRepository.GetById(id);
            if(brand == null)
            {
                return BadRequest("not found");
            }
            else
            {
                brand.IsDeleted = brand.IsDeleted == true ? false : true;
                await _brandRepository.Update(brand);

                return NoContent();
            }
        }
    }
}
