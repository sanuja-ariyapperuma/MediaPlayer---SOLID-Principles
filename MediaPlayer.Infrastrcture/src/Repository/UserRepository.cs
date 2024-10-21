using MediaPlayer.Service.UserService;
using MediaPlyer.Domain.UserAggregate;

namespace MediaPlayer.Infrastrcture.Repository
{
    public class UserRepository(Database database) : IUserRepository
    {
        private readonly List<User> _users = database.Users;

        public User AddUser(User user) {
            _users.Add(user);
            return user;
        }
        public List<User>? GetAllUsers() => _users;
        public User? GetUser(Guid userid) => _users.Find(user => user.Id == userid);
        public bool RemoveUser(Guid userid) => _users.Remove(_users.Find(user => user.Id == userid)!);
        public User? GetUserByEmailAndPassword(string email, string password) => _users.Find(user => user.Email == email && user.Password == password);


        public bool UpdateUser(Guid userid, User updatedUser)
        {
            var user = _users.Find(user => user.Id == userid);
            user!.UserCategory = updatedUser.UserCategory;
            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            return true;
        }
    }
}
