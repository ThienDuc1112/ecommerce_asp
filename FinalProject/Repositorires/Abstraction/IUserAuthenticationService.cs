using FinalProject.ViewModels;
using FinalProject.ViewModels.Users;

namespace FinalProject.Repositorires.Abstraction
{
    public interface IUserAuthenticationService
    {
         Task<JwtToken> LoginAsync(LoginModel model);
         Task<Status> RegisterAsync(RegisterModel model);
    }
}
