using MediaPlayer.Service.DTO.UserDTO;
using MediaPlyer.Domain.UserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Controller.src.helper
{
    public static class Util
    {
        public static readonly string unauthorizedMessage = "You are not authorized to this action";
        public static bool IsLoggedInAdmin(ReadUserDto user)
        {
            return user.UserCategory == UserCategory.Admin.ToString();
        }
    }
}
