using MediaPlayer.Controller.src.helper;
using MediaPlayer.Service.AuthenticationService;
using MediaPlayer.Service.DTO.UserDTO;
using MediaPlayer.Service.LogService;
using MediaPlayer.Service.UserService;
using MediaPlyer.Domain.UserAggregate;
using System.Text.Json;

namespace MediaPlayer.Controller
{
    public class UserController
    {
        private readonly IUserManagement _userManagement;
        private readonly IAuthenticationManagment _authManager;
        private readonly ILogger _logger;
        private readonly string _unauthorizedMessage = Util.unauthorizedMessage;

        public UserController(
            IUserManagement userManagement, 
            IAuthenticationManagment authManager, 
            ILogger logger)
        {
            this._userManagement = userManagement;
            this._authManager = authManager;
            this._logger = logger;
        }

        public void AddUser(CreateUserDto user)
        {
            if (!IsUserAuthenticatedAsAdmin()) return;
            _userManagement.AddUser(user);
        }
        public void RemoveUser(string userid)
        {
            if (!IsUserAuthenticatedAsAdmin()) return;

            if (Guid.TryParse(userid, out Guid id)) 
            {
                _userManagement.RemoveUser(id);
                return;
            }
            _logger.Log("Invalid user ID found. Cannot delete the user");
        }
        public string GetUser(string userid)
        {

            if (Guid.TryParse(userid, out Guid id)) 
            {
                var user = _userManagement.GetUser(id);

                if (user == null) return "User not found";

                var userJson = JsonSerializer.Serialize(user);
                return userJson;
            }

            return "Invalid user ID found. Cannot update the user";
        }
        public string GetAllUsers()
        {
            var users = _userManagement.GetAllUsers();

            if(users == null) return "No users found";
            
            var usersJson = JsonSerializer.Serialize(users);
            return usersJson;
        }
        public void UpdateUser(string userid, CreateUserDto updatedUser)
        {
            if (!IsUserAuthenticatedAsAdmin()) return;
            if (Guid.TryParse(userid, out Guid id)) 
            {
                _userManagement.UpdateUser(id, updatedUser);
                return;
            }

            _logger.Log("Invalid user ID found. Cannot update the user");
        }
        private bool IsUserAuthenticatedAsAdmin()
        {
            var loggedInUser = _authManager.GetLoggedInUser();
            if (loggedInUser == null || !Util.IsLoggedInAdmin(loggedInUser))
            {
                Console.WriteLine(_unauthorizedMessage);
                return false;
            }
            return true;
        }



    }
}
