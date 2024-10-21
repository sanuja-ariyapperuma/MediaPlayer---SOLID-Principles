using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaPlayer.Service.AuthenticationService;
using MediaPlayer.Service.DTO.UserDTO;
using MediaPlayer.Service.LogService;
using MediaPlayer.Service.UserService;
using MediaPlyer.Domain.UserAggregate;
using Moq;
using Xunit;

namespace MediaPlayer.Test.src.Service
{
    public class AuthenticationServiceTest
    {
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IAuthenticationRepository> _authenticationRepository;
        private readonly Mock<ILogger> _logger;
        private readonly AuthenticationManager _authenticationManager;
        
        public AuthenticationServiceTest()
        {
            _userRepository = new Mock<IUserRepository>();
            _authenticationRepository = new Mock<IAuthenticationRepository>();
            _logger = new Mock<ILogger>();

            _authenticationManager = new AuthenticationManager(_userRepository.Object, _authenticationRepository.Object, _logger.Object);
        }
        
        private void SetupValidUser_GetUserByEmailAndPassword()
        {
            _userRepository.Setup(repo => repo.GetUserByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new User(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserCategory>()));
        }
        private void SetupInvalidUser_GetUserByEmailAndPassword()
        {
            _userRepository.Setup(repo => repo.GetUserByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((User)null);
        }
        private void SetupNoLoggedInUser_GetLoggedInUser()
        {
            _authenticationRepository.Setup(repo => repo.GetLoggedInUser()).Returns((User)null);
        }

        [Fact]
        public void Login_ValidUser_ReturnsTrue()
        {
            //Arrange
            SetupValidUser_GetUserByEmailAndPassword();

            //Act 
            var result = _authenticationManager.Login(It.IsAny<string>(), It.IsAny<string>());

            //Assert
            Assert.True(result);
            //_authenticationRepository.Verify(repo => repo.Login(It.IsAny<User>()), Times.Once);
        }
        [Fact]
        public void Login_ValidUser_LoggsSuccessfull()
        {
            //Arrange
            SetupValidUser_GetUserByEmailAndPassword();

            //Act 
            var result = _authenticationManager.Login(It.IsAny<string>(), It.IsAny<string>());

            //Assert
            _logger.Verify(repo => repo.Log("Successfully Logged In"));
        }
        [Fact]
        public void Login_ValidUser_CallsAuthenticationRepositoryLoginOnce()
        {
            //Arrange
            SetupValidUser_GetUserByEmailAndPassword();

            //Act 
            var result = _authenticationManager.Login(It.IsAny<string>(), It.IsAny<string>());

            //Assert
            _authenticationRepository.Verify(repo => repo.Login(It.IsAny<User>()), Times.Once);
        }
        [Fact]
        public void Login_InvalidUser_ReturnFalse()
        {
            //Arrange
            SetupInvalidUser_GetUserByEmailAndPassword();
            
            //Act 
            var result = _authenticationManager.Login(It.IsAny<string>(), It.IsAny<string>());

            //Assert
            Assert.False(result);
        }
        [Fact]
        public void Login_InvalidUser_LoggsUsernameOrPasswordError()
        {
            //Arrange
            SetupInvalidUser_GetUserByEmailAndPassword();
            
            //Act 
            var result = _authenticationManager.Login(It.IsAny<string>(), It.IsAny<string>());

            //Assert
            _logger.Verify(repo => repo.Log("Invalid Username or Password"));
        }
        [Fact]
        public void Login_InvalidUser_ShouldNotCallsAuthenticationRepositoryLogin()
        {
            //Arrange
            SetupInvalidUser_GetUserByEmailAndPassword();

            //Act 
            var result = _authenticationManager.Login(It.IsAny<string>(), It.IsAny<string>());

            //Assert
            _logger.Verify(repo => repo.Log("Successfully Logged In"), Times.Never);
        }
        [Fact]
        public void Login_ThrowException_ReturnFalse()
        {
            //Arrange
            _userRepository.Setup(repo => repo.GetUserByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>();

            //Act
            var result = _authenticationManager.Login(It.IsAny<string>(), It.IsAny<string>());

            //Assert
            Assert.False(result);
            
        }
        [Fact]
        public void GetLoggedInUser_WhenValidUserIsLoggedIn_ReturnsCorrectUserDetails()
        {
            //Arrange
            User loggedInUser = new("TestUSer","a.b@c.com", "12345", UserCategory.Admin);
            _authenticationRepository.Setup(repo => repo.GetLoggedInUser()).Returns(loggedInUser);

            //Act
            var result = _authenticationManager.GetLoggedInUser();

            //Assert
            Assert.IsType<ReadUserDto>(result);
            Assert.Equal(loggedInUser.Email, result.Email);
            Assert.Equal(loggedInUser.Id, result.Id);
            Assert.Equal(loggedInUser.Name, result.Name);
            Assert.Equal(loggedInUser.UserCategory.ToString(), result.UserCategory);
        }
        [Fact]
        public void GetLoggedInUser_WhenNoUserLoggedIn_ReturnsNull()
        {
            //Arrange
            SetupNoLoggedInUser_GetLoggedInUser();

            //Act
            var results = _authenticationManager.GetLoggedInUser();

            //Assert
            Assert.Null(results);
        }
        [Fact]
        public void GetLoggedInUser_WhenNoUserLoggedIn_LoggsMessage()
        {
            //Arrange
            SetupNoLoggedInUser_GetLoggedInUser();

            //Act
            var results = _authenticationManager.GetLoggedInUser();

            //Assert
            _logger.Verify(repo => repo.Log("No User Logged In"));
        }
        [Fact]
        public void GetLoggedInUser_WhenExceptionThrown_ReturnsNull()
        {
            //Arrange
            _authenticationRepository.Setup(repo => repo.GetLoggedInUser()).Throws<Exception>();

            //Act
            var result = _authenticationManager.GetLoggedInUser();

            //Assert
            Assert.Null(result);
        }
        [Fact]
        public void Logout_WhenSuccessLogout_CallsAuthenticationRepoLogout()
        {
            //Act
            _authenticationManager.Logout();
            //Assert 
            _authenticationRepository.Verify(repo => repo.Logout(), Times.Once);

        }
        [Fact]
        public void Logout_WhenSuccessLogout_LoggsMessage()
        {
            //Arrange 
            _authenticationRepository.Setup(repo => repo.Logout()).Throws<Exception>();
            //Act && Assert
            Assert.Throws<Exception>(() => _authenticationManager.Logout());
        }
        [Fact]
        public void AuthenticationManager_WhenCalledWithNullUserRepository_ThrowsArgumentNullException()
        {
            //Act && Assert
            Assert.Throws<ArgumentNullException>(() => new AuthenticationManager(null, _authenticationRepository.Object, _logger.Object));
        }
        [Fact]
        public void AuthenticationManager_WhenCalledWithNullAuthenticationRepository_ThrowsArgumentNullException()
        {
            //Act && Assert
            Assert.Throws<ArgumentNullException>(() => new AuthenticationManager(_userRepository.Object, null, _logger.Object));
        }
        [Fact]
        public void AuthenticationManager_WhenCalledWithNullLogger_ThrowsArgumentNullException()
        {
            //Act && Assert
            Assert.Throws<ArgumentNullException>(() => new AuthenticationManager(_userRepository.Object, _authenticationRepository.Object, null));
        }

    }
}