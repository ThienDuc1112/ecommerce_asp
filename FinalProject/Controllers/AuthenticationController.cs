using FinalProject.Repositorires.Abstraction;
using FinalProject.ViewModels;
using FinalProject.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthenticationController : Controller
    {
        private IUserAuthenticationService userAuthenticationService;

        public AuthenticationController(IUserAuthenticationService userAuthenticationService)
        {
            this.userAuthenticationService = userAuthenticationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid login request");
            }

            try
            {
                var jwtToken = await userAuthenticationService.LoginAsync(model);
                return Ok(jwtToken);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Status
                {
                    IsSuccess = false,
                    Message = "Invalid registration request"
                });
            }

            try
            {
                var registrationStatus = await userAuthenticationService.RegisterAsync(model);

                if (registrationStatus.IsSuccess)
                {
                    return Ok(registrationStatus);
                }
                else
                {
                    return BadRequest(registrationStatus);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Test()
        {
            return Ok("authen success");
        }
    }
}
