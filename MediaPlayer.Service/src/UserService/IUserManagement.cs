
using MediaPlayer.Service.DTO.UserDTO;

namespace MediaPlayer.Service.UserService
{
    public interface IUserManagement
    {
        public CreateUserDto AddUser(CreateUserDto user);
        public bool RemoveUser(Guid userid);
        public ReadUserDto? GetUser(Guid userid);
        public List<ReadUserDto>? GetAllUsers();
        public bool UpdateUser(Guid userid, CreateUserDto updatedUser);
    }
}