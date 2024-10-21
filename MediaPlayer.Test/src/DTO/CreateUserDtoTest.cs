using MediaPlayer.Domain;
using MediaPlayer.Service.DTO.UserDTO;
using MediaPlayer.Service.src.Helpers;
using MediaPlyer.Domain.UserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Test.src.DTO
{
    public class CreateUserDtoTest
    {
        [Fact]
        public void Convert_ValidUser_ReturnsUser()
        {
            //Arrange
            var createUserDto = new CreateUserDto
            {
                Name = "Test Name",
                Email = "a.b@c.com",
                Password = "password",
                UserCategory = 1,
                PlayTracks = []
            };

            //Act
            var result = createUserDto.Convert();

            //Assert
            Assert.IsType<User>(result);
            Assert.Equal(createUserDto.Name, result.Name);
            Assert.Equal(createUserDto.Email, result.Email);
            Assert.Equal(createUserDto.UserCategory, (int)result.UserCategory);
            Assert.Equal(Hash.ComputeSha256Hash(createUserDto.Password), result.Password);
            Assert.Equal(createUserDto.PlayTracks, result.PlayTracks);
            
        }
    }
}
