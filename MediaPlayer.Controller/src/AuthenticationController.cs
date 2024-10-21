using MediaPlayer.Service.AuthenticationService;
using MediaPlayer.Service.DTO.UserDTO;
using MediaPlayer.Service.LogService;

namespace MediaPlayer.Controller
{
    public class AuthenticationController
    {
        private readonly IAuthenticationManagment _authManagement;

        public AuthenticationController(
            IAuthenticationManagment authManagement
        )
        {
            this._authManagement = authManagement;
        }
        public bool Login(string email, string password) => _authManagement.Login(email, password);
        public void Logout() => _authManagement.Logout();
        public ReadUserDto? GetLoggedInUser() => _authManagement.GetLoggedInUser();
    }
}
