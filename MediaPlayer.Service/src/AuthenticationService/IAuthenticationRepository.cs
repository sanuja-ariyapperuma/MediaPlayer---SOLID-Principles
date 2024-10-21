using MediaPlyer.Domain.UserAggregate;

namespace MediaPlayer.Service.AuthenticationService
{
    public interface IAuthenticationRepository
    {
        public void Login(User user);
        public void Logout();
        public User? GetLoggedInUser();
    }
}
