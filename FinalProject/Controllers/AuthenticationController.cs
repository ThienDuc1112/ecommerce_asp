using AutoMapper;
using FinalProject.Models;
using FinalProject.Repositorires.Abstraction;
using FinalProject.ViewModels;
using FinalProject.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthenticationController : Controller
    {
        private IUserAuthenticationService userAuthenticationService;
        private IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public AuthenticationController(IUserAuthenticationService userAuthenticationService, IUserRepository userRepository
            , IMapper mapper, UserManager<User> userManager)
        {
            this.userAuthenticationService = userAuthenticationService;
            _userRepository = userRepository;
            _mapper = mapper;
            _userManager = userManager;
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


        [HttpGet("GetProfile")]
        public async Task<IActionResult> ViewProfile(string userId)
        {
            var user = await _userRepository.GetUser(userId);
            if (user == null)
            {
                return NotFound();
            }
           GetUser userVM = _mapper.Map<GetUser>(user);
            return Ok(userVM);
        }

        [HttpPut]
        public async Task<ActionResult<Status>> Update([FromBody] UpdateUser userVM)
        {
            Status status = new Status();

            if (!ModelState.IsValid)
            {
                status.IsSuccess = false;
                status.Message = "Invalid model";
                return Ok(status);
            }

            var user = await _userRepository.GetUser(userVM.UserId);
            if (user == null)
            {
                status.IsSuccess = false;
                status.Message = $"User with ID {userVM.UserId} does not exist.";
                return NotFound(status);
            }
            var userExists = await _userManager.FindByEmailAsync(userVM.Email);
            if (userExists != null )
            {
                if(userVM.Email != userExists.Email)
                {
                    status.IsSuccess = false;
                    status.Message = "This email already exists";
                    return Ok(status);
                }
            }

            _mapper.Map(userVM, user);

            try
            {
                await _userRepository.Update(user);
                status.IsSuccess = true;
                status.Message = "Update successful";
                return Ok(status);
            }
            catch (Exception ex)
            {
                status.IsSuccess = false;
                status.Message = "An error occurred while updating the user.";
                return StatusCode(500, status);
            }
        }
    }
}
