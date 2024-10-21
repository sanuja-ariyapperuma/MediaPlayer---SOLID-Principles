using MediaPlayer.Domain;
using MediaPlayer.Domain.MediaFileAggregate.ValueObject;
using MediaPlayer.Domain.src.MediaFileAggregate.Entity;
using MediaPlayer.Service.LogService;
using MediaPlayer.Service.MediaService;
using MediaPlayer.Service.PlayTrackService;
using MediaPlayer.Service.src.MediaService;
using MediaPlayer.Service.UserService;
using MediaPlyer.Domain.MediaFileAggregate;
using MediaPlyer.Domain.UserAggregate;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Test.src.Service
{
    public class PlayTrackServiceTest
    {
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IMediaFileRepository> _mediaFileRepository;
        private readonly Mock<ILogger> _logger;

        private readonly PlayTrackManagment _playTrackManagement;

        public PlayTrackServiceTest()
        {
            _userRepository = new Mock<IUserRepository>();
            _mediaFileRepository = new Mock<IMediaFileRepository>();
            _logger = new Mock<ILogger>();

            _playTrackManagement = new PlayTrackManagment(_userRepository.Object, _mediaFileRepository.Object, _logger.Object);

        }

        [Fact]
        public void PlayTrackManagement_WhenCalledWithNullUserRepository_ShouldThrowArgumentException()
        {
            //Arrange & Assert
            Assert.Throws<ArgumentNullException>(() => new PlayTrackManagment(null, _mediaFileRepository.Object, _logger.Object));
        }
        [Fact]
        public void PlayTrackManagement_WhenCalledWithNullMediaFileRepository_ShouldThrowArgumentException()
        {
            //Arrange & Assert
            Assert.Throws<ArgumentNullException>(() => new PlayTrackManagment(_userRepository.Object, null, _logger.Object));
        }
        [Fact]
        public void PlayTrackManagement_WhenCalledWithNullLogger_ShouldThrowArgumentException()
        {
            //Arrange & Assert
            Assert.Throws<ArgumentNullException>(() => new PlayTrackManagment(_userRepository.Object, _mediaFileRepository.Object, null));
        }
        [Fact]
        public void AddMediaFileToPlayTrack_WhenCalledWithInvalidUser_ShouldReturnFalse()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var playTrackId = Guid.NewGuid();
            var mediaFileId = Guid.NewGuid();

            _userRepository.Setup(x => x.GetUser(userId)).Returns((User)null);

            //Act
            var result = _playTrackManagement.AddMediaFileToPlayTrack(userId, playTrackId, mediaFileId);

            //Assert
            Assert.False(result);
            _logger.Verify(x => x.Log($"User with ID {userId} not found."), Times.Once);
        }
        [Fact]
        public void AddMediaFileToPlayTrack_WhenCalledWithInvalidPlayTrack_ShouldReturnFalse()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var playTrackId = Guid.NewGuid();
            var mediaFileId = Guid.NewGuid();
            var user = new User("test", "test", "test", UserCategory.Admin);

            _userRepository.Setup(x => x.GetUser(userId)).Returns(user);


            //Act
            var result = _playTrackManagement.AddMediaFileToPlayTrack(userId, playTrackId, mediaFileId);

            //Assert
            Assert.False(result);
            _logger.Verify(x => x.Log($"PlayTrack with ID {playTrackId} not found."), Times.Once);
        }
        [Fact]
        public void AddMediaFileToPlayTrack_WhenCalledWithInvalidMediaFile_ShouldReturnFalse()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var mediaFileId = Guid.NewGuid();
            var user = new User("test", "test", "test", UserCategory.Admin);
            var playTrack = new PlayTrack("test", user);

            _userRepository.Setup(x => x.GetUser(userId)).Returns(user);
            user.PlayTracks.Add(playTrack);

            var playTrackId = playTrack.Id;

            //Act
            var result = _playTrackManagement.AddMediaFileToPlayTrack(userId, playTrackId, mediaFileId);

            //Assert
            Assert.False(result);
            _logger.Verify(x => x.Log($"MediaFile with ID {mediaFileId} not found."), Times.Once);
        }
        [Fact]
        public void AddMediaFileToPlayTrack_WhenCalledWithValidData_ShouldReturnTrue()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var mediaFileId = Guid.NewGuid();
            var user = new User("test", "test", "test", UserCategory.Admin);
            var playTrack = new PlayTrack("test", user);
            var mediaFile = new AudioFile("test", MediaFileCategory.HipHop);

            _userRepository.Setup(x => x.GetUser(userId)).Returns(user);
            user.PlayTracks.Add(playTrack);
            var playTrackId = playTrack.Id;

            _mediaFileRepository.Setup(x => x.GetMediaFile(mediaFileId)).Returns(mediaFile);

            //Act
            var result = _playTrackManagement.AddMediaFileToPlayTrack(userId, playTrackId, mediaFileId);

            //Assert
            Assert.True(result);
            _logger.Verify(x => x.Log("MediaFile added to PlayTrack successfully."), Times.Once);
        }
        [Fact]
        public void CreatePlayTrack_WhenCalledWithInvalidUser_ShouldReturnNull()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var name = "test";

            _userRepository.Setup(x => x.GetUser(userId)).Returns((User)null);

            //Act
            var result = _playTrackManagement.CreatePlayTrack(userId, name);

            //Assert
            Assert.Null(result);
            _logger.Verify(x => x.Log($"User with ID {userId} not found."), Times.Once);
        }
        [Fact]
        public void CreatePlayTrack_WhenCalledWithValidData_ShouldReturnPlayTrack()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var playTrackName = "test";
            var user = new User("test", "test", "test", UserCategory.Admin);

            _userRepository.Setup(x => x.GetUser(userId)).Returns(user);

            //Act
            var result = _playTrackManagement.CreatePlayTrack(userId, playTrackName);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(playTrackName, result.Name);
            _userRepository.Verify(x => x.UpdateUser(userId, user), Times.Once);
            _logger.Verify(x => x.Log($"PlayTrack '{playTrackName}' created successfully."), Times.Once);
        }
        [Fact]
        public void GetAllPlayTracks_WhenCalledWithInvalidUser_ShouldReturnNull()
        {
            //Arrange
            var userId = Guid.NewGuid();

            _userRepository.Setup(x => x.GetUser(userId)).Returns((User)null);

            //Act
            var result = _playTrackManagement.GetAllPlayTracks(userId);

            //Assert
            Assert.Null(result);
            _logger.Verify(x => x.Log($"User with ID {userId} not found."), Times.Once);
        }
        [Fact]
        public void GetAllPlayTracks_WhenCalledWithValidUser_ShouldReturnPlayTracks()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var user = new User("test", "test", "test", UserCategory.Admin);
            var playTrack = new PlayTrack("test", user);

            _userRepository.Setup(x => x.GetUser(userId)).Returns(user);
            user.PlayTracks.Add(playTrack);

            //Act
            var result = _playTrackManagement.GetAllPlayTracks(userId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<PlayTrack>>(result);
            Assert.Equal(playTrack, result.FirstOrDefault());
        }
        [Fact]
        public void GetPlayTrack_WhenCalledWithInvalidUser_ShouldReturnNull()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var playTrackId = Guid.NewGuid();

            _userRepository.Setup(x => x.GetUser(userId)).Returns((User)null);

            //Act
            var result = _playTrackManagement.GetPlayTrack(userId, playTrackId);

            //Assert
            Assert.Null(result);
            _logger.Verify(x => x.Log($"User with ID {userId} not found."), Times.Once);
        }
        [Fact]
        public void GetPlayTrack_WhenCalledWithInvalidPlayTrack_ShouldReturnNull()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var playTrackId = Guid.NewGuid();
            var user = new User("test", "test", "test", UserCategory.Admin);

            _userRepository.Setup(x => x.GetUser(userId)).Returns(user);

            //Act
            var result = _playTrackManagement.GetPlayTrack(userId, playTrackId);

            //Assert
            Assert.Null(result);
            _logger.Verify(x => x.Log($"PlayTrack with ID {playTrackId} not found."), Times.Once);
        }
        [Fact]
        public void GetPlayTrack_WhenCalledWithValidData_ShouldReturnPlayTrack()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var playTrackId = Guid.NewGuid();
            var user = new User("test", "test", "test", UserCategory.Admin);
            var playTrack = new PlayTrack("test", user);
            playTrackId = playTrack.Id;

            _userRepository.Setup(x => x.GetUser(userId)).Returns(user);
            user.PlayTracks.Add(playTrack);
            

            //Act
            var result = _playTrackManagement.GetPlayTrack(userId, playTrackId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<PlayTrack>(result);
            Assert.Equal(playTrack, result);
        }
        [Fact]
        public void RemoveAllPlayTracks_WhenCalledWithInvalidUser_ShouldReturnFalse()
        {
            //Arrange
            var userId = Guid.NewGuid();

            _userRepository.Setup(x => x.GetUser(userId)).Returns((User)null);

            //Act
            var result = _playTrackManagement.RemoveAllPlayTracks(userId);

            //Assert
            Assert.False(result);
            _logger.Verify(x => x.Log($"User with ID {userId} not found."), Times.Once);
        }
        [Fact]
        public void RemoveAllPlayTracks_WhenCalledWithValidUser_ShouldReturnTrue()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var user = new User("test", "test", "test", UserCategory.Admin);
            var playTrack = new PlayTrack("test", user);

            _userRepository.Setup(x => x.GetUser(userId)).Returns(user);
            user.PlayTracks.Add(playTrack);

            //Act
            var result = _playTrackManagement.RemoveAllPlayTracks(userId);

            //Assert
            Assert.True(result);
            Assert.Empty(user.PlayTracks);
            _userRepository.Verify(x => x.UpdateUser(userId, user), Times.Once);
            _logger.Verify(x => x.Log("All PlayTracks removed successfully."), Times.Once);
        }
        [Fact]
        public void RemoveMediaFileFromPlayTrack_WhenCalledWithInvalidUser_ShouldReturnFalse()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var playTrackId = Guid.NewGuid();
            var mediaFileId = Guid.NewGuid();

            _userRepository.Setup(x => x.GetUser(userId)).Returns((User)null);

            //Act
            var result = _playTrackManagement.RemoveMediaFileFromPlayTrack(userId, playTrackId, mediaFileId);

            //Assert
            Assert.False(result);
            _logger.Verify(x => x.Log($"User with ID {userId} not found."), Times.Once);
        }
        [Fact]
        public void RemoveMediaFileFromPlayTrack_WhenCalledWithInvalidPlayTrack_ShouldReturnFalse()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var playTrackId = Guid.NewGuid();
            var mediaFileId = Guid.NewGuid();
            var user = new User("test", "test", "test", UserCategory.Admin);

            _userRepository.Setup(x => x.GetUser(userId)).Returns(user);

            //Act
            var result = _playTrackManagement.RemoveMediaFileFromPlayTrack(userId, playTrackId, mediaFileId);

            //Assert
            Assert.False(result);
            _logger.Verify(x => x.Log($"PlayTrack with ID {playTrackId} not found."), Times.Once);
        }
        [Fact]
        public void RemoveMediaFileFromPlayTrack_WhenCalledWithInvalidMediaFile_ShouldReturnFalse()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var mediaFileId = Guid.NewGuid();
            var user = new User("test", "test", "test", UserCategory.Admin);
            var playTrack = new PlayTrack("test", user);

            _userRepository.Setup(x => x.GetUser(userId)).Returns(user);
            user.PlayTracks.Add(playTrack);
            var playTrackId = playTrack.Id;

            //Act
            var result = _playTrackManagement.RemoveMediaFileFromPlayTrack(userId, playTrackId, mediaFileId);

            //Assert
            Assert.False(result);
            _logger.Verify(x => x.Log($"MediaFile with ID {mediaFileId} not found."), Times.Once);
        }
        [Fact]
        public void RemoveMediaFileFromPlayTrack_WhenCalledWithValidData_ShouldReturnTrue()
        {
            //Arrange
            var userId = Guid.NewGuid();
         
            var user = new User("test", "test", "test", UserCategory.Admin);
            var playTrack = new PlayTrack("test", user);
            var mediaFile = new AudioFile("test", MediaFileCategory.HipHop);

            playTrack.MediaFiles.Add(mediaFile);

            user.PlayTracks.Add(playTrack);
            var playTrackId = playTrack.Id;
            var mediaFileId = mediaFile.Id;

            _userRepository.Setup(x => x.GetUser(userId)).Returns(user);
            _mediaFileRepository.Setup(x => x.GetMediaFile(mediaFileId)).Returns(mediaFile);

            //Act
            var result = _playTrackManagement.RemoveMediaFileFromPlayTrack(userId, playTrackId, mediaFileId);

            //Assert
            Assert.True(result);
            _logger.Verify(x => x.Log("MediaFile removed from PlayTrack successfully."), Times.Once);
        }
        [Fact]
        public void RemovePlayTrack_WhenCalledWithInvalidUser_ShouldReturnFalse()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var playTrackId = Guid.NewGuid();

            _userRepository.Setup(x => x.GetUser(userId)).Returns((User)null);

            //Act
            var result = _playTrackManagement.RemovePlayTrack(userId, playTrackId);

            //Assert
            Assert.False(result);
            _logger.Verify(x => x.Log($"User with ID {userId} not found."), Times.Once);
        }
        [Fact]
        public void RemovePlayTrack_WhenCalledWithInvalidPlayTrack_ShouldReturnFalse()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var playTrackId = Guid.NewGuid();
            var user = new User("test", "test", "test", UserCategory.Admin);

            _userRepository.Setup(x => x.GetUser(userId)).Returns(user);

            //Act
            var result = _playTrackManagement.RemovePlayTrack(userId, playTrackId);

            //Assert
            Assert.False(result);
            _logger.Verify(x => x.Log($"PlayTrack with ID {playTrackId} not found."), Times.Once);
        }
        [Fact]
        public void RemovePlayTrack_WhenCalledWithValidData_ShouldReturnTrue()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var user = new User("test", "test", "test", UserCategory.Admin);
            var playTrack = new PlayTrack("test", user);

            user.PlayTracks.Add(playTrack);
            var playTrackId = playTrack.Id;

            _userRepository.Setup(x => x.GetUser(userId)).Returns(user);

            //Act
            var result = _playTrackManagement.RemovePlayTrack(userId, playTrackId);

            //Assert
            Assert.True(result);
            _logger.Verify(x => x.Log("PlayTrack removed successfully."), Times.Once);
        }
        [Fact]
        public void UpdatePlayTrack_WhenCalledWithInvalidUser_ShouldReturnFalse()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var playTrackId = Guid.NewGuid();
            var name = "test";

            _userRepository.Setup(x => x.GetUser(userId)).Returns((User)null);

            //Act
            var result = _playTrackManagement.UpdatePlayTrack(userId, playTrackId, name);

            //Assert
            Assert.False(result);
            _logger.Verify(x => x.Log($"User with ID {userId} not found."), Times.Once);
        }
        [Fact]
        public void UpdatePlayTrack_WhenCalledWithInvalidPlayTrack_ShouldReturnFalse()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var playTrackId = Guid.NewGuid();
            var name = "test";
            var user = new User("test", "test", "test", UserCategory.Admin);

            _userRepository.Setup(x => x.GetUser(userId)).Returns(user);

            //Act
            var result = _playTrackManagement.UpdatePlayTrack(userId, playTrackId, name);

            //Assert
            Assert.False(result);
            _logger.Verify(x => x.Log($"PlayTrack with ID {playTrackId} not found."), Times.Once);
        }
        [Fact]
        public void UpdatePlayTrack_WhenCalledWithValidData_ShouldReturnTrue()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var name = "test";
            var user = new User("test", "test", "test", UserCategory.Admin);
            var playTrack = new PlayTrack("test", user);

            user.PlayTracks.Add(playTrack);
            var playTrackId = playTrack.Id;

            _userRepository.Setup(x => x.GetUser(userId)).Returns(user);

            //Act
            var result = _playTrackManagement.UpdatePlayTrack(userId, playTrackId, name);

            //Assert
            Assert.True(result);
            _logger.Verify(x => x.Log("PlayTrack updated successfully."), Times.Once);
        }
    }
}
