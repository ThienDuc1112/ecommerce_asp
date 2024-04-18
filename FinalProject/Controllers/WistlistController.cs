using AutoMapper;
using FinalProject.Models;
using FinalProject.Repositorires.Abstraction;
using FinalProject.Repositorires.Implement;
using FinalProject.ViewModels;
using FinalProject.ViewModels.Whislist;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WistlistController : Controller
    {

        private readonly IWistlistRepository _wishlistRepository;
        private readonly IMapper _mapper;

        public WistlistController(IWistlistRepository wishlistRepository, IMapper mapper)
        {
            _wishlistRepository = wishlistRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetWistlist>>> GetWishlists([FromQuery(Name = "userId")] string userId)
        {
            var wishlist = await _wishlistRepository.GetWhislists(userId);
            if (wishlist == null)
            {
                return NotFound();
            }
            return Ok(wishlist);
        }

        [HttpPost]
        public async Task<ActionResult<Status>> AddWishlist([FromBody] CreateWistlist wishlist)
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
                if(await _wishlistRepository.IsExisted(wishlist.UserId, wishlist.ProductId))
                {
                    status.IsSuccess = false;
                    status.Message = "This product was already added to your wishlist";
                    return BadRequest(status);
                }
                else
                {
                    var newWishlist = _mapper.Map<Whislist>(wishlist);
                    await _wishlistRepository.Add(newWishlist);
                    status.IsSuccess = true;
                    status.Message = "create succesfully";

                    return Ok(status);
                }
            }

        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery(Name = "id")] int id)
        {
            var product = await _wishlistRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            await _wishlistRepository.Delete(product);
            return NoContent();
        }

    }
}
