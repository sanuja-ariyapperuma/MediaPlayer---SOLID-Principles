using MediaPlayer.Service.AuthenticationService;
using MediaPlyer.Domain.UserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Infrastrcture.Repository
{
    public class AuthenticationRepository(Database database) : IAuthenticationRepository
    {
        private User? loggedInUser = database.LoggedInUser;
        public void Login(User user) => loggedInUser = user;
        public void Logout() => loggedInUser = null;
        public User? GetLoggedInUser() => loggedInUser;
    }
}
