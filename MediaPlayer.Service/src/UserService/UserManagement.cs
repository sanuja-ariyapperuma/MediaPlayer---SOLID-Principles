using MediaPlayer.Service.DTO.UserDTO;
using MediaPlayer.Service.LogService;
using MediaPlyer.Domain.UserAggregate;

namespace MediaPlayer.Service.UserService
{
    public class UserManagement : IUserManagement
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;

        public UserManagement(
            IUserRepository userRepository, 
            ILogger logger
            )
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this._logger = logger ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public CreateUserDto AddUser(CreateUserDto user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var newUser = user.Convert();
            _userRepository.AddUser(newUser);
            _logger.Log($"User, {newUser.Name} added successfully");
            
            user.Id = newUser.Id;
            user.Password = string.Empty;

            return user;
        }

        public List<ReadUserDto>? GetAllUsers()
        {
            try
            {
                var users = _userRepository.GetAllUsers();
                return users?.Select(ConvertToReadUserDto).ToList();
            }
            catch (Exception e)
            {
                _logger.Log(e.Message);
                return null;
            }
        }

        public ReadUserDto? GetUser(Guid userid)
        {
            try
            {
                var user = _userRepository.GetUser(userid);
                return user != null ? ConvertToReadUserDto(user) : null;
            }
            catch (Exception e)
            {
                _logger.Log(e.Message);
                return null;
            }
        }

        public bool RemoveUser(Guid userid)
        {
            try
            {
                var deletingUser = _userRepository.GetUser(userid);
                if (deletingUser == null)
                {
                    _logger.Log("User not found");
                    return false;
                }

                _userRepository.RemoveUser(userid);

                _logger.Log("User removed successfully");
                return true;
            }
            catch (Exception e)
            {
                _logger.Log(e.Message);
                return false;
            }
        }

        public bool UpdateUser(Guid userid, CreateUserDto updatedUser)
        {
            if (updatedUser == null) throw new ArgumentNullException(nameof(updatedUser));

            var existingUser = _userRepository.GetUser(userid);
            if (existingUser == null)
            {
                _logger.Log("User not found");
                return false;
            }

            UpdateUserFromDto(existingUser, updatedUser);

            _userRepository.UpdateUser(userid, existingUser);
            _logger.Log("User updated successfully");

            return true;
        }

        private ReadUserDto ConvertToReadUserDto(User user) => new ReadUserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            UserCategory = user.UserCategory.ToString()
        };

        private void UpdateUserFromDto(User existingUser, CreateUserDto updatedUser)
        {
            existingUser.Name = updatedUser.Name;
            existingUser.Email = updatedUser.Email;
            existingUser.UserCategory = (UserCategory)updatedUser.UserCategory;
            if (updatedUser.PlayTracks != null) existingUser.PlayTracks = updatedUser.PlayTracks;
        }
    }
}
