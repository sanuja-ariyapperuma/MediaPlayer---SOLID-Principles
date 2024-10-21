using MediaPlayer.Service.DTO.UserDTO;
using MediaPlayer.Service.UserService;
using MediaPlayer.Service.LogService;

namespace MediaPlayer.Service.AuthenticationService
{
    public class AuthenticationManager : IAuthenticationManagment
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly ILogger _logger;

        public AuthenticationManager(
            IUserRepository userRepository, 
            IAuthenticationRepository authenticationRepository,
            ILogger logger
            )
        {
            this._userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this._authenticationRepository = authenticationRepository ?? throw new ArgumentNullException(nameof(authenticationRepository));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        } 
        public bool Login(string email, string password)
        {
            try{
                var user = _userRepository.GetUserByEmailAndPassword(email, password);

                if (user == null)
                {
                    _logger.Log("Invalid Username or Password");
                    return false;
                }

                _authenticationRepository.Login(user);

                _logger.Log("Successfully Logged In");
                return true;
            }catch(Exception e)
            {
                _logger.Log(e.Message);
                return false;
            }
            
        }
        public ReadUserDto? GetLoggedInUser() 
        {
            try{
                var user = _authenticationRepository.GetLoggedInUser();

                if (user == null)
                {
                    _logger.Log("No User Logged In");
                    return null;
                }

                return new ReadUserDto()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    UserCategory = user.UserCategory.ToString()
                };
            }catch(Exception e){
                _logger.Log(e.Message);
                return null;
            }
        }

        public void Logout()
        {
            _authenticationRepository.Logout();
            _logger.Log("Successfully Logged Out");
        }
    }
}
