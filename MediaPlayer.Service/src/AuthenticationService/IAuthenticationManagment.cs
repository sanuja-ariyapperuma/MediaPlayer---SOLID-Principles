using MediaPlayer.Service.DTO.UserDTO;
using MediaPlyer.Domain.UserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Service.AuthenticationService
{
    public interface IAuthenticationManagment
    {
        public bool Login(string email, string password);
        public void Logout();

        public ReadUserDto? GetLoggedInUser();
    }
}
