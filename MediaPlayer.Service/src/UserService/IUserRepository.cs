using MediaPlyer.Domain.UserAggregate;

namespace MediaPlayer.Service.UserService
{
    public interface IUserRepository
    {
        public User AddUser(User user);
        public bool RemoveUser(Guid userid);
        public User? GetUser(Guid userid);
        public User? GetUserByEmailAndPassword(string email, string password);
        public List<User>? GetAllUsers();
        public bool UpdateUser(Guid userid, User updatedUser);
    }
}
