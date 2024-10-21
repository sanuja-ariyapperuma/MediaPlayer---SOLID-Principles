using MediaPlayer.Domain.MediaFileAggregate.ValueObject;
using MediaPlayer.Domain.src.MediaFileAggregate.Entity;
using MediaPlayer.Service.DTO.MediaFileDTO;
using MediaPlayer.Service.LogService;
using MediaPlayer.Service.MediaService;
using MediaPlayer.Service.src.MediaService;
using MediaPlyer.Domain.MediaFileAggregate;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Test.src.Service
{
    public class MediaFileServiceTest
    {
        private readonly Mock<IMediaFileRepository> _mediaFileRepository;
        private readonly Mock<ILogger> _logger;
        private readonly Mock<IMediaFileFactory> _fileFactory;

        private readonly MediaFileManagement _mediaFileManagement;

        public MediaFileServiceTest()
        {
            _mediaFileRepository = new Mock<IMediaFileRepository>();
            _logger = new Mock<ILogger>();
            _fileFactory = new Mock<IMediaFileFactory>();

            _mediaFileManagement = new MediaFileManagement(_mediaFileRepository.Object, _logger.Object, _fileFactory.Object);
            
        }

        [Fact]
        public void MediaFileManagement_WhenCalledWithNullMediaFileRepository_ThrowsArgumentNullException()
        {
            //Act && Assert
            Assert.Throws<ArgumentNullException>(() => new MediaFileManagement(null, _logger.Object, _fileFactory.Object));
        }
        [Fact]
        public void MediaFileManagement_WhenCalledWithNullLogger_ThrowsArgumentNullException()
        {
            //Act && Assert
            Assert.Throws<ArgumentNullException>(() => new MediaFileManagement(_mediaFileRepository.Object, null, _fileFactory.Object));
        }
        [Fact]
        public void MediaFileManagement_WhenCalledWithNullFileFactory_ThrowsArgumentNullException()
        {
            //Act && Assert
            Assert.Throws<ArgumentNullException>(() => new MediaFileManagement(_mediaFileRepository.Object, _logger.Object, null));
        }
        [Fact]
        public void AddMediaFile_WhenCalledWithNullMediaFile_ReturnsNullWithMessage()
        {
            //Arrange
            CreateMediaFileDto mediaFile = null;

            //Act
            var result = _mediaFileManagement.AddMediaFile(mediaFile);

            //Assert
            Assert.Null(result);
            _logger.Verify(logger => logger.Log("Media file cannot be null"), Times.Once);
        }
        [Fact]
        public void AddMediaFile_WhenCalledWithInvalidMediaFile_ReturnsNull()
        {
            //Arrange
            CreateMediaFileDto mediaFile = new CreateMediaFileDto
            {
                Name = "",
                MediaFileCategory = (MediaFileCategory)10,
                MediaFileType = (MediaFileType)10
            };

            //Act
            var result = _mediaFileManagement.AddMediaFile(mediaFile);

            //Assert
            Assert.Null(result);
        }
        [Fact]
        public void AddMediaFile_WhenCalledWithValidAudioFile_ReturnsAudioFile()
        {
            //Arrange
            CreateMediaFileDto mediaFile = new CreateMediaFileDto
            {
                Name = "Test",
                MediaFileCategory = MediaFileCategory.HipHop,
                MediaFileType = MediaFileType.Audio
            };

            var file = new AudioFile(mediaFile.Name, mediaFile.MediaFileCategory);
            _fileFactory.Setup(factory => factory.CreateMediaFile(mediaFile)).Returns(file);

            //Act
            var result = _mediaFileManagement.AddMediaFile(mediaFile);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(mediaFile.Name, result.Name);
            Assert.Equal(mediaFile.MediaFileCategory, result.MediaFileCategory);
            Assert.Equal(mediaFile.MediaFileType, result.MediaFileType);
        }
        [Fact]
        public void AddMediaFile_WhenCalledWithValidMediaFile_ThrowsException() 
        {
            //Arrange
            CreateMediaFileDto mediaFile = new CreateMediaFileDto
            {
                Name = "Test",
                MediaFileCategory = MediaFileCategory.HipHop,
                MediaFileType = MediaFileType.Audio
            };

            var file = new AudioFile(mediaFile.Name, mediaFile.MediaFileCategory);

            _fileFactory.Setup(factory => factory.CreateMediaFile(mediaFile)).Throws(new Exception("Exception Message"));

            //Act
            var result = _mediaFileManagement.AddMediaFile(mediaFile);

            //Assert
            Assert.Null(result);
            _logger.Verify(logger => logger.Log("Exception Message"), Times.Once);
        }
        [Fact]
        public void GetAllMediaFiles_WhenCalled_ReturnsAllMediaFiles()
        {
            //Arrange
            var mediaFiles = new List<MediaFile>
            {
                new AudioFile(It.IsAny<string>(), It.IsAny<MediaFileCategory>()),
                new AudioFile(It.IsAny<string>(), It.IsAny<MediaFileCategory>()),
                new AudioFile(It.IsAny<string>(), It.IsAny<MediaFileCategory>()),
                new AudioFile(It.IsAny<string>(), It.IsAny<MediaFileCategory>())
            };

            _mediaFileRepository.Setup(repo => repo.GetAllMediaFiles()).Returns(mediaFiles);

            //Act
            var result = _mediaFileManagement.GetAllMediaFiles();

            //Assert
            Assert.IsType<List<MediaFile>>(result);
        }
        [Fact]
        public void GetMediaFile_WhenCalledWithValidMediaFileId_ReturnsMediaFile()
        {
            //Arrange
            var mediaFile = new AudioFile(It.IsAny<string>(), It.IsAny<MediaFileCategory>());

            _mediaFileRepository.Setup(repo => repo.GetMediaFile(It.IsAny<Guid>())).Returns(mediaFile);

            //Act
            var result = _mediaFileManagement.GetMediaFile(It.IsAny<Guid>());

            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        public void GetMediaFile_WhenCalledWithInvalidMediaFileId_ReturnsNull()
        {
            //Arrange
            _mediaFileRepository.Setup(repo => repo.GetMediaFile(It.IsAny<Guid>())).Returns((MediaFile)null);

            //Act
            var result = _mediaFileManagement.GetMediaFile(It.IsAny<Guid>());

            //Assert
            Assert.Null(result);
        }
        [Fact]
        public void RemoveAllMediaFile_WhenCalled_RemovesAllMediaFiles()
        {
            //Act
            _mediaFileManagement.RemoveAllMediaFile();

            //Assert
            _mediaFileRepository.Verify(repo => repo.RemoveAllMediaFile(), Times.Once);
        }
        [Fact]
        public void RemoveMediaFile_WhenCalledWithValidMediaFileId_RemovesMediaFile()
        {
            //Arrange
            var mediaFile = new AudioFile(It.IsAny<string>(), It.IsAny<MediaFileCategory>());
            _mediaFileRepository.Setup(repo => repo.GetMediaFile(It.IsAny<Guid>())).Returns(mediaFile);

            //Act
            _mediaFileManagement.RemoveMediaFile(It.IsAny<Guid>());

            //Assert
            _mediaFileRepository.Verify(repo => repo.RemoveMediaFile(It.IsAny<Guid>()), Times.Once);
        }
        [Fact]
        public void RemoveMediaFile_WhenCalledWithInvalidMediaFileId_DoesNotRemoveMediaFile()
        {
            //Arrange
            _mediaFileRepository.Setup(repo => repo.GetMediaFile(It.IsAny<Guid>())).Returns((MediaFile)null);

            //Act
            _mediaFileManagement.RemoveMediaFile(It.IsAny<Guid>());

            //Assert
            _mediaFileRepository.Verify(repo => repo.RemoveMediaFile(It.IsAny<Guid>()), Times.Never);
        }
        [Fact]
        public void UpdateMediaFile_WhenCalledWithValidMediaFileId_ReturnsTrue()
        {
            //Arrange
            var mediaFile = new AudioFile(It.IsAny<string>(), It.IsAny<MediaFileCategory>());
            _mediaFileRepository.Setup(repo => repo.GetMediaFile(It.IsAny<Guid>())).Returns(mediaFile);

            var updatedMediaFile = new CreateMediaFileDto
            {
                Name = "Test",
                MediaFileCategory = MediaFileCategory.HipHop,
                MediaFileType = MediaFileType.Audio
            };

            //Act
            var result = _mediaFileManagement.UpdateMediaFile(It.IsAny<Guid>(), updatedMediaFile);

            //Assert
            Assert.True(result);
        }
        [Fact]
        public void UpdateMediaFile_WhenCalledWithInvalidMediaFileId_ReturnsFalse()
        {
            //Arrange
            _mediaFileRepository.Setup(repo => repo.GetMediaFile(It.IsAny<Guid>())).Returns((MediaFile)null);

            var updatedMediaFile = new CreateMediaFileDto
            {
                Name = "Test",
                MediaFileCategory = MediaFileCategory.HipHop,
                MediaFileType = MediaFileType.Audio
            };

            //Act
            var result = _mediaFileManagement.UpdateMediaFile(It.IsAny<Guid>(), updatedMediaFile);

            //Assert
            Assert.False(result);
        }
        [Fact]
        public void UpdateMediaFile_WhenCalledWithInvalidMediaFile_ReturnsFalse()
        {
            //Arrange
            var mediaFile = new AudioFile(It.IsAny<string>(), It.IsAny<MediaFileCategory>());
            _mediaFileRepository.Setup(repo => repo.GetMediaFile(It.IsAny<Guid>())).Returns(mediaFile);

            var updatedMediaFile = new CreateMediaFileDto
            {
                Name = "",
                MediaFileCategory = (MediaFileCategory)10,
                MediaFileType = (MediaFileType)10
            };

            //Act
            var result = _mediaFileManagement.UpdateMediaFile(It.IsAny<Guid>(), updatedMediaFile);

            //Assert
            Assert.False(result);
        }
    }
}
