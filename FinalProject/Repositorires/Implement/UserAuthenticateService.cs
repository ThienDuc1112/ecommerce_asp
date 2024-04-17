using FinalProject.Context;
using FinalProject.Models;
using FinalProject.Repositorires.Abstraction;
using FinalProject.ViewModels;
using FinalProject.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinalProject.Repositorires.Implement
{
    public class UserAuthenticateService : IUserAuthenticationService
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _db;
        private readonly IConfiguration _configuration;

        public UserAuthenticateService(IConfiguration configuration, UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
            IHttpContextAccessor httpContextAccessor, AppDbContext db)
        {
            _configuration = configuration;
            this.userManager = userManager;
            this.roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _db = db;
        }

        public async Task<JwtToken> LoginAsync(LoginModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);
                var jwtToken = new JwtToken
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                };

                return jwtToken;
            }

            throw new Exception("Invalid email or password");
        }

        public async Task<Status> RegisterAsync(RegisterModel model)
        {
            var status = new Status();
            var userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                status.IsSuccess = false;
                status.Message = "This email already exists";
                return status;
            }
            User user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.FirstName.Replace(" ", ""),
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            try
            {
                var result = await userManager.CreateAsync(user, model.Password);
                await _db.SaveChangesAsync();

                if (!result.Succeeded)
                {
                    status.IsSuccess = false;


                    foreach (var item in result.Errors)
                    {
                        status.Message += item.Description + " ";
                    }
                }
                else
                {
                    if (!await roleManager.RoleExistsAsync(model.Role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(model.Role));
                    }

                    if (await roleManager.RoleExistsAsync(model.Role))
                    {
                        await userManager.AddToRoleAsync(user, model.Role);
                    }
                    status.IsSuccess = true;
                    status.Message = "registering succesfully";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;

        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
