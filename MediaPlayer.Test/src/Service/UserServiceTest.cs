using MediaPlayer.Service.DTO.UserDTO;
using MediaPlayer.Service.LogService;
using MediaPlayer.Service.UserService;
using MediaPlyer.Domain.UserAggregate;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Test.src.Service
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<ILogger> _logger;

        private readonly UserManagement _userManagement;

        public UserServiceTest()
        {
            _userRepository = new Mock<IUserRepository>();
            _logger = new Mock<ILogger>();

            _userManagement = new UserManagement(_userRepository.Object, _logger.Object);
        }

        [Fact]
        public void UserManagement_WhenCalledWithNullUserRepository_ThrowsArgumentNullException()
        {
            //Act && Assert
            Assert.Throws<ArgumentNullException>(() => new UserManagement(null, _logger.Object));
        }
        [Fact]
        public void UserManagement_WhenCalledWithNullLogger_ThrowsArgumentNullException()
        {
            //Act && Assert
            Assert.Throws<ArgumentNullException>(() => new UserManagement(_userRepository.Object, null));
        }
        [Fact]
        public void AddUser_WhenCalledWithNullUser_ThrowsArgumentNullException()
        {
            //Act && Assert
            Assert.Throws<ArgumentNullException>(() => _userManagement.AddUser(null));
        }
        [Fact]
        public void AddUser_WhenCalledWithValidUser_ReturnsValidUser()
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

            _userRepository.Setup(repo => repo.AddUser(It.IsAny<User>()))
                .Returns(
                new User(
                    createUserDto.Name,
                    createUserDto.Email,
                    createUserDto.Password,
                    (UserCategory)createUserDto.UserCategory));

            //Act
            var result = _userManagement.AddUser(createUserDto);
            var hasId = result.Id != Guid.Empty;

            //Assert
            Assert.IsType<CreateUserDto>(result);
            Assert.True(hasId);
            Assert.Empty(result.Password);

        }
        [Fact]
        public void AddUser_WhenCalledWithoutAUser_ThrowsArgumentNullException()
        {
            //Act && Assert
            Assert.Throws<ArgumentNullException>(() => _userManagement.AddUser(null));
        }
        [Fact]
        public void GetAllUsers_WhenCalled_ReturnsAllUsers()
        {
            //Arrange
            var users = new List<User>
            {
                new User(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserCategory>()),
                new User(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserCategory>()),
                new User(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserCategory>()),
                new User(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserCategory>())
            };

            _userRepository.Setup(repo => repo.GetAllUsers()).Returns(users);

            //Act
            var result = _userManagement.GetAllUsers();

            //Assert
            Assert.IsType<List<ReadUserDto>>(result);
        }
        [Fact]
        public void GetAllUsers_WhenCalled_ThrowsException()
        {
            //Arrange
            var exceptionMessage = "Exception Message";
            _userRepository.Setup(repo => repo.GetAllUsers()).Throws(new Exception(exceptionMessage));

            //Act
            var result = _userManagement.GetAllUsers();

            //Assert
            Assert.Null(result);
            _logger.Verify(logger => logger.Log(exceptionMessage), Times.Once);
        }
        [Fact]
        public void GetUser_WhenCalledWithValidUserId_ReturnsUser()
        {
            //Arrange
            var user = new User(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserCategory>());
            _userRepository.Setup(repo => repo.GetUser(It.IsAny<Guid>())).Returns(user);

            //Act
            var result = _userManagement.GetUser(It.IsAny<Guid>());

            //Assert
            Assert.IsType<ReadUserDto>(result);
        }
        [Fact]
        public void GetUser_WhenCalledWithInvalidUserId_ReturnsNull()
        {
            //Arrange
            _userRepository.Setup(repo => repo.GetUser(It.IsAny<Guid>())).Returns((User)null);

            //Act
            var result = _userManagement.GetUser(It.IsAny<Guid>());

            //Assert
            Assert.Null(result);
        }
        [Fact]
        public void GetUser_WhenCall_ThrowsException()
        {
            //Arrange
            var exceptionMessage = "Exception Message";
            _userRepository.Setup(repo => repo.GetUser(It.IsAny<Guid>())).Throws(new Exception(exceptionMessage));

            //Act
            var result = _userManagement.GetUser(It.IsAny<Guid>());

            //Assert
            Assert.Null(result);
            _logger.Verify(logger => logger.Log(exceptionMessage), Times.Once);
        }
        [Fact]
        public void RemoveUser_WhenCalledWithValidUserId_ReturnsTrue()
        {
            //Arrange
            _userRepository.Setup(repo => repo.GetUser(It.IsAny<Guid>())).Returns(new User(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserCategory>()));

            //Act
            var result = _userManagement.RemoveUser(It.IsAny<Guid>());

            //Assert
            Assert.True(result);
            _logger.Verify(logger => logger.Log("User removed successfully"), Times.Once);
        }
        [Fact]
        public void RemoveUser_WhenCalledWithInvalidUserId_ReturnsFalse()
        {
            //Arrange
            _userRepository.Setup(repo => repo.GetUser(It.IsAny<Guid>())).Returns((User)null);

            //Act
            var result = _userManagement.RemoveUser(It.IsAny<Guid>());

            //Assert
            Assert.False(result);
            _logger.Verify(logger => logger.Log("User not found"), Times.Once);
        }
        [Fact]
        public void RemoveUser_WhenCalled_ThrowsException()
        {
            //Arrange
            var exceptionMessage = "Exception Message";
            _userRepository.Setup(repo => repo.GetUser(It.IsAny<Guid>())).Throws(new Exception(exceptionMessage));

            //Act
            var result = _userManagement.RemoveUser(It.IsAny<Guid>());

            //Assert
            Assert.False(result);
            _logger.Verify(logger => logger.Log(exceptionMessage), Times.Once);
        }
        [Fact]
        public void UpdateUser_WhenCalledWithNullUser_ThrowsArgumentNullException()
        {
            //Act && Assert
            Assert.Throws<ArgumentNullException>(() => _userManagement.UpdateUser(It.IsAny<Guid>(), null));
        }
        [Fact]
        public void UpdateUser_WhenCalledWithValidUser_ReturnsTrue()
        {
            //Arrange
            _userRepository.Setup(repo => repo.GetUser(It.IsAny<Guid>()))
                .Returns(new User(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserCategory>()));
            var updatedUser = new CreateUserDto
            {
                Id = It.IsAny<Guid>(),
                Name = It.IsAny<string>(),
                Email = It.IsAny<string>(),
                Password = It.IsAny<string>(),
                UserCategory = It.IsAny<int>(),
                PlayTracks = []
            };

            //Act
            var result = _userManagement.UpdateUser(It.IsAny<Guid>(), updatedUser);

            //Assert
            Assert.True(result);
            _logger.Verify(logger => logger.Log("User updated successfully"), Times.Once);
        }

        [Fact]
        public void UpdateUser_WhenCallWithInvalidUser_ReturnsFalse() 
        {
            //Arrange
            _userRepository.Setup(repo => repo.GetUser(It.IsAny<Guid>())).Returns((User)null);

            var updatedUser = new CreateUserDto
            {
                Id = It.IsAny<Guid>(),
                Name = It.IsAny<string>(),
                Email = It.IsAny<string>(),
                Password = It.IsAny<string>(),
                UserCategory = It.IsAny<int>(),
                PlayTracks = []
            };


            //Act
            var result = _userManagement.UpdateUser(It.IsAny<Guid>(), updatedUser);

            //Assert
            Assert.False(result);
            _logger.Verify(logger => logger.Log("User not found"), Times.Once);

        }
    }
}
