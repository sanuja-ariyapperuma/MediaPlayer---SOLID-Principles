using MediaPlayer.Domain;
using MediaPlayer.Domain.MediaFileAggregate.ValueObject;
using MediaPlayer.Domain.src.MediaFileAggregate.Entity;
using MediaPlayer.Service.LogService;
using MediaPlayer.Service.PlaybackService;
using MediaPlayer.Service.PlayTrackService;
using MediaPlayer.Service.src.PlaybackService;
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
    public class PlayBackServiceTest
    {
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IPlayBackRepository> _playBackRepository;
        private readonly Mock<ILogger> _logger;

        private readonly PlaybackManagment _playbackManagment;
        public PlayBackServiceTest()
        {
            _playBackRepository = new Mock<IPlayBackRepository>();
            _userRepository = new Mock<IUserRepository>();
            _logger = new Mock<ILogger>();

            _playbackManagment = new PlaybackManagment(
                _userRepository.Object,
                _playBackRepository.Object,
                _logger.Object);
        }

        [Fact]
        public void PlaybackManagement_WhenCalledWithNullUserRepository_ShouldThrowArgumentException() 
        {
            Assert.Throws<ArgumentNullException>(() => new PlaybackManagment(null, _playBackRepository.Object, _logger.Object));

        }
        [Fact]
        public void PlaybackManagement_WhenCalledWithNullPlayBackRepository_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => new PlaybackManagment(_userRepository.Object, null, _logger.Object));

        }
        [Fact]
        public void PlaybackManagement_WhenCalledWithNullLogger_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => new PlaybackManagment(_userRepository.Object, _playBackRepository.Object, null));

        }
        [Fact]
        public void Play_WhenCalledWithInValidUser_ShouldNotPlayMediaFile()
        {
            //Arrange
            var userId = Guid.NewGuid();
            _userRepository.Setup(x => x.GetUser(userId)).Returns((User)null);

            //Act
            _playbackManagment.Play(userId, Guid.NewGuid(), Guid.NewGuid());

            //Assert
            _playBackRepository.Verify(x => x.GetCurrentPlayFile(), Times.Never);
            _logger.Verify(x => x.Log($"User not found with ID {userId}."), Times.Once);
        }
        [Fact]
        public void Play_WhenCalledWithInValidPlayTrack_ShouldNotPlayMediaFile()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var invalidPlayTrackId = Guid.NewGuid();
            var user = new User("Test User", "a.b@c.com" ,"TestUser", UserCategory.Admin);
            
            var playTrack = new PlayTrack("Test PlayTrack", user);
            user.PlayTracks.Add(playTrack);
            
            _userRepository.Setup(x => x.GetUser(userId)).Returns(user);

            //Act
            _playbackManagment.Play(userId, invalidPlayTrackId, Guid.NewGuid());

            //Assert
            _playBackRepository.Verify(x => x.GetCurrentPlayFile(), Times.Never);
            _logger.Verify(x => x.Log($"PlayTrack not found with ID {invalidPlayTrackId}."), Times.Once);
        }
        [Fact]
        public void Play_WhenCalledWithInValidMediaFile_ShouldNotPlayMediaFile()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var user = new User("Test User", "a.b@c.com", "TestUser", UserCategory.Admin);
            var playTrack = new PlayTrack("Test PlayTrack", user);
            user.PlayTracks.Add(playTrack);

            var invalidMediaFileId = Guid.NewGuid();
            var mediaFile = new AudioFile("Test MediaFile", MediaFileCategory.Other);
            playTrack.MediaFiles.Add(mediaFile);

            _userRepository.Setup(x => x.GetUser(userId)).Returns(user);

            //Act
            _playbackManagment.Play(userId, playTrack.Id, invalidMediaFileId);

            //Assert
            _playBackRepository.Verify(x => x.GetCurrentPlayFile(), Times.Never);
            _logger.Verify(x => x.Log($"MediaFile not found with ID {invalidMediaFileId}."), Times.Once);

        }
        [Fact]
        public void Play_WhenCalledWithValidDataWhilePlayingNoSong_ShouldPlayMediaFile()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var user = new User("Test User", "", "TestUser", UserCategory.Admin);
            var playTrack = new PlayTrack("Test PlayTrack", user);
            user.PlayTracks.Add(playTrack);
            var mediaFile = new AudioFile("Test MediaFile", MediaFileCategory.Other);
            playTrack.MediaFiles.Add(mediaFile);

            _userRepository.Setup(x => x.GetUser(userId)).Returns(user);
            _playBackRepository.SetupSequence(x => x.GetCurrentPlayFile())
                .Returns((MediaFile)null)
                .Returns(mediaFile);

            //Act
            _playbackManagment.Play(userId, playTrack.Id, mediaFile.Id);

            //Assert
            _playBackRepository.Verify(x => x.GetCurrentPlayFile(), Times.Between(2,2,Moq.Range.Inclusive));
            _playBackRepository.Verify(x => x.SetCurrentPlayFile(mediaFile), Times.Once);
            Assert.Equal(MediaFileStatus.Playing, mediaFile.PlayStatus);

        }
        [Fact]
        public void Play_WhenCalledWithValidDataWhilePlayingSong_ShouldStopCurrentSongAndPlayNewSong()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var user = new User("Test User", "", "TestUser", UserCategory.Admin);
            var playTrack = new PlayTrack("Test PlayTrack", user);
            user.PlayTracks.Add(playTrack);
            var mediaFile = new AudioFile("Test MediaFile", MediaFileCategory.Other);
            playTrack.MediaFiles.Add(mediaFile);

            var currentPlayingFile = new AudioFile("Current Playing File", MediaFileCategory.Other);

            _userRepository.Setup(x => x.GetUser(userId)).Returns(user);
            _playBackRepository.SetupSequence(x => x.GetCurrentPlayFile())
                .Returns(currentPlayingFile)
                .Returns(mediaFile);

            //Act
            _playbackManagment.Play(userId, playTrack.Id, mediaFile.Id);

            //Assert
            _playBackRepository.Verify(x => x.GetCurrentPlayFile(), Times.Between(2, 2, Moq.Range.Inclusive));
            _playBackRepository.Verify(x => x.SetCurrentPlayFile(mediaFile), Times.Once);
            Assert.Equal(MediaFileStatus.Playing, mediaFile.PlayStatus);
            Assert.Equal(MediaFileStatus.Stopped, currentPlayingFile.PlayStatus);

        }
        [Fact]
        public void Stop_WhenCalledWithNothingPlaying_ShouldLoggMessage()
        {
            //Arrange
            var userId = Guid.NewGuid();
            _playBackRepository.Setup(x => x.GetCurrentPlayFile()).Returns((MediaFile)null);

            //Act
            _playbackManagment.Stop();

            //Assert
  
            _logger.Verify(x => x.Log($"There is no current files are playing to stop"), Times.Once);
        }
        [Fact]
        public void Stop_WhenCalledWithPlayingSong_ShouldStopPlayingSong()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var currentPlayingFile = new AudioFile("Current Playing File", MediaFileCategory.Other);
            _playBackRepository.Setup(x => x.GetCurrentPlayFile()).Returns(currentPlayingFile);

            //Act
            _playbackManagment.Stop();

            //Assert
            _playBackRepository.Verify(x => x.GetCurrentPlayFile(), Times.Once);
            Assert.Equal(MediaFileStatus.Stopped, currentPlayingFile.PlayStatus);
        }
        [Fact]
        public void Pause_WhenCalledWithNothingPlaying_ShouldLoggMessage()
        {
            //Arrange
            var userId = Guid.NewGuid();
            _playBackRepository.Setup(x => x.GetCurrentPlayFile()).Returns((MediaFile)null);

            //Act
            _playbackManagment.Pause();

            //Assert

            _logger.Verify(x => x.Log($"There is no current files are playing to stop"), Times.Once);
        }
        [Fact]
        public void Pause_WhenCalledWithPlayingSong_ShouldPausePlayingSong()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var currentPlayingFile = new AudioFile("Current Playing File", MediaFileCategory.Other);
            _playBackRepository.Setup(x => x.GetCurrentPlayFile()).Returns(currentPlayingFile);

            //Act
            _playbackManagment.Pause();

            //Assert
            _playBackRepository.Verify(x => x.GetCurrentPlayFile(), Times.Once);
            Assert.Equal(MediaFileStatus.Paused, currentPlayingFile.PlayStatus);
        }
        [Fact]
        public void SetVolume_WhenCalledWithPlayingSong_ShouldSetVolume()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var currentPlayingFile = new AudioFile("Current Playing File", MediaFileCategory.Other);
            _playBackRepository.Setup(x => x.GetCurrentPlayFile()).Returns(currentPlayingFile);

            //Act
            _playbackManagment.SetVolume(50);

            //Assert
            _playBackRepository.Verify(x => x.GetCurrentPlayFile(), Times.Once);
            Assert.Equal(50, currentPlayingFile.Volume);
        }
        [Fact]
        public void SetVolume_WhenCalledWithNothingPlaying_ShouldNotSetVolume()
        {
            //Arrange
            var userId = Guid.NewGuid();
            _playBackRepository.Setup(x => x.GetCurrentPlayFile()).Returns((MediaFile)null);

            //Act
            _playbackManagment.SetVolume(50);

            //Assert
            _playBackRepository.Verify(x => x.GetCurrentPlayFile(), Times.Once);
            _logger.Verify(x => x.Log($"No files are currently playing."), Times.Once);
        }
        [Fact]
        public void SetVolume_WhenCalledWithInvalidVolume_ShouldNotSetVolume()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var currentPlayingFile = new AudioFile("Current Playing File", MediaFileCategory.Other);
            _playBackRepository.Setup(x => x.GetCurrentPlayFile()).Returns(currentPlayingFile);

            //Act
            _playbackManagment.SetVolume(150);

            //Assert
            _logger.Verify(x => x.Log($"Volume should be between 0 and 100."), Times.Once);
        }
        [Fact]
        public void SetVolume_WhenCalledWithMinusVolume_ShouldNotSetVolume()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var currentPlayingFile = new AudioFile("Current Playing File", MediaFileCategory.Other);
            _playBackRepository.Setup(x => x.GetCurrentPlayFile()).Returns(currentPlayingFile);

            //Act
            _playbackManagment.SetVolume(-50);

            //Assert
            _logger.Verify(x => x.Log($"Volume should be between 0 and 100."), Times.Once);
        }
        [Fact]
        public void SetBrightness_WhenCalledWithPlayingVideoFile_ShouldSetBrightness()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var currentPlayingFile = new VideoFile("Current Playing File", MediaFileCategory.Other);
            _playBackRepository.Setup(x => x.GetCurrentPlayFile()).Returns(currentPlayingFile);

            //Act
            _playbackManagment.SetBrightness(50);

            //Assert
            _playBackRepository.Verify(x => x.GetCurrentPlayFile(), Times.Once);
            Assert.Equal(50, currentPlayingFile.Brightness);
        }
        [Fact]
        public void SetBrightness_WhenCalledWithPlayingAudioFile_ShouldNotSetBrightness()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var currentPlayingFile = new AudioFile("Current Playing File", MediaFileCategory.Other);
            _playBackRepository.Setup(x => x.GetCurrentPlayFile()).Returns(currentPlayingFile);

            //Act
            _playbackManagment.SetBrightness(50);

            //Assert
            _logger.Verify(x => x.Log("Brightness setting only valid for video files"), Times.Once);
        }
        [Fact]
        public void SetBrightness_WhenCalledWithNothingPlaying_ShouldNotSetBrightness()
        {
            //Arrange
            var userId = Guid.NewGuid();
            _playBackRepository.Setup(x => x.GetCurrentPlayFile()).Returns((MediaFile)null);

            //Act
            _playbackManagment.SetBrightness(50);

            //Assert
            _playBackRepository.Verify(x => x.GetCurrentPlayFile(), Times.Once);
            _logger.Verify(x => x.Log($"No files are currently playing."), Times.Once);
        }
        [Fact]
        public void SetBrightness_WhenCalledWithInvalidBrightness_ShouldNotSetBrightness()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var currentPlayingFile = new VideoFile("Current Playing File", MediaFileCategory.Other);
            _playBackRepository.Setup(x => x.GetCurrentPlayFile()).Returns(currentPlayingFile);

            //Act
            _playbackManagment.SetBrightness(150);

            //Assert
            _logger.Verify(x => x.Log($"Brightness should be between 0 and 100."), Times.Once);
        }
        [Fact]
        public void SetBrightness_WhenCalledWithMinusBrightness_ShouldNotSetBrightness()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var currentPlayingFile = new VideoFile("Current Playing File", MediaFileCategory.Other);
            _playBackRepository.Setup(x => x.GetCurrentPlayFile()).Returns(currentPlayingFile);

            //Act
            _playbackManagment.SetBrightness(-50);

            //Assert
            _logger.Verify(x => x.Log($"Brightness should be between 0 and 100."), Times.Once);
        }

        
    }
}
