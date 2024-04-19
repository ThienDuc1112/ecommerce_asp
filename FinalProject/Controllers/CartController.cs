using FinalProject.Repositorires.Abstraction;
using FinalProject.Services;
using FinalProject.ViewModels;
using FinalProject.ViewModels.Cart;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CartController : Controller
    {
        private const string SessionKeyName = "_Cart";
        private IProductRepository _productRepository;

        public CartController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCart>>> GetCart()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<GetCart>>(SessionKeyName);
            if(cart == null)
            {
                return NotFound();
            }
            return Ok(cart);
        }

        [HttpPost]
        public async Task<ActionResult<Status>> AddToCart([FromQuery(Name ="productId")] int productId)
        {
            Status status = new Status();
            var product = await _productRepository.GetProductWithRelevant(productId);
            if (product == null)
            {
                return NotFound();
            }

            var cart = HttpContext.Session.GetObjectFromJson<List<GetCart>>(SessionKeyName) ?? new List<GetCart>();
            var cartItem = cart.FirstOrDefault(i => i.ProductId == productId);

            if (product.Quantity <= 0)
            {
                status.IsSuccess = false;
                status.Message = "There isn't enough quantity";
            }

            if (cartItem == null)
            {
                cartItem = new GetCart
                {
                    ProductName = product.Title,
                    ProductId = productId,
                    ImageProduct = product.Image,
                    Price = product.Price,
                    Category = product.Category.Name,
                    Quantity = 1,
                    SubTotal = product.Price
                };
                cart.Add(cartItem);
                status.IsSuccess = true;
                status.Message = $"{product.Title} added successfully to cart";
            }
            else if (product.Quantity > cartItem.Quantity)
            {
                cartItem.Quantity++;
                cartItem.SubTotal = cartItem.Price * cartItem.Quantity;
                status.IsSuccess = true;
                status.Message = $"{product.Title} is updated successfully to cart";

            }
            else
            {
                status.IsSuccess = false;
                status.Message = "There isn't enough quantity";
            }
            HttpContext.Session.SetObjectAsJson(SessionKeyName, cart);
            return Ok(status);
        }

        [HttpPut]
        public async Task<ActionResult<Status>> UpdateCart([FromQuery(Name = "productId")] int productId, [FromQuery(Name = "quantity")] int quantity)
        {
            Status status = new Status();
            var product = await _productRepository.GetProductWithRelevant(productId);
            if (product == null)
            {
                return NotFound();
            }

            var cart = HttpContext.Session.GetObjectFromJson<List<GetCart>>(SessionKeyName);
            var cartItem = cart.FirstOrDefault(p => p.ProductId == productId);

            if (product.Quantity > quantity)
            {
                cartItem.Quantity = quantity;
                cartItem.SubTotal = cartItem.Price * quantity;
                status.IsSuccess = true;
                status.Message = $"{product.Title} is updated successfully to cart";
            }
            else
            {
                status.IsSuccess = false;
                status.Message = "There isn't enough quantity";
            }
            HttpContext.Session.SetObjectAsJson(SessionKeyName, cart);

            return Ok(status);
        }
        [HttpDelete("Clear")]
        public async Task<IActionResult> Clear()
        {
            HttpContext.Session.SetObjectAsJson<List<GetCart>>(SessionKeyName, null);
            return NoContent();
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<GetCart>>(SessionKeyName);
            var itemCart = cart.FirstOrDefault(p => p.ProductId == id);
            if (itemCart != null)
            {
                cart.Remove(itemCart);
                HttpContext.Session.SetObjectAsJson<List<GetCart>>(SessionKeyName, cart);
            }
            return NoContent();
        }

    }
}
