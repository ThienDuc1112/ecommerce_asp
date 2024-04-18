using AutoMapper;
using FinalProject.Models;
using FinalProject.Repositorires.Abstraction;
using FinalProject.ViewModels;
using FinalProject.ViewModels.Category;
using FinalProject.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet("ProductList")]
        public async Task<ActionResult<DisplayingProduct>> GetProductList([FromQuery(Name = "query")] string? query, [FromQuery(Name = "page")] int page = 1)
        {
            var products = await _productRepository.GetListProduct(page, query);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpGet("GetNewProducts")]
        public async Task<ActionResult<IEnumerable<GetProduct>>> GetNewProducts()
        {
            var query = await _productRepository.GetNewestProducts();
            var products = _mapper.Map<List<GetProduct>>(query);

            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }
        [HttpGet("Detail")]
        public async Task<ActionResult<GetDetailProduct>> GetDetail([FromQuery(Name ="id")] int id)
        {
            var product = await _productRepository.GetDetailProduct(id);
            if(product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("ProductAdminList")]
        public async Task<ActionResult<DisplayingProduct>> GetProductAdminList([FromQuery(Name = "query")] string? query, [FromQuery(Name = "page")] int page = 1)
        {
            var products = await _productRepository.GetAdminListProduct(page, query);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<Status>> Add([FromBody] CreateProduct productVM)
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
                var check = await _productRepository.IsExisted(productVM.Title);
                if (check == true)
                {
                    status.IsSuccess = false;
                    status.Message = "this product already exists";
                    return BadRequest(status);
                }
                else
                {
                    Product product = _mapper.Map<Product>(productVM);
                    await _productRepository.Add(product);
                    status.IsSuccess = true;
                    status.Message = "product created successfully";
                    return Ok(status);
                }
            }
        }

        [HttpPut]
        public async Task<ActionResult<Status>> Update([FromBody] UpdateProduct productVM)
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
                var product = await _productRepository.GetById(productVM.Id);
                if (product == null)
                {
                    status.IsSuccess = false;
                    status.Message = $"product doesn't exist with id: {productVM.Id}";
                    return BadRequest(status);
                }
                else
                {
                    _mapper.Map(productVM, product);
                    await _productRepository.Update(product);
                    status.IsSuccess = true;
                    status.Message = "product updated successfully";
                    return Ok(status);
                }
            }
        }

        [HttpPut("Toggle")]
        public async Task<ActionResult> Toggle([FromQuery(Name = "id")] int id)
        {
            var product = await _productRepository.GetById(id);
            if (product == null)
            {
                return BadRequest("not found");
            }
            else
            {
                product.IsDeleted = product.IsDeleted == true ? false : true;
                await _productRepository.Update(product);

                return NoContent();
            }
        }
    }
}
