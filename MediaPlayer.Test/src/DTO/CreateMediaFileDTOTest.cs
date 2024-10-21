using MediaPlayer.Domain.MediaFileAggregate.ValueObject;
using MediaPlayer.Domain.src.MediaFileAggregate.Entity;
using MediaPlayer.Service.DTO.MediaFileDTO;
using Moq;

namespace MediaPlayer.Test.src.DTO
{
    public class CreateMediaFileDTOTest
    {
        [Fact]
        public void Validate_EmptyName_ReturnsFalse()
        {
            //Arrange
            var createMediaFileDto = new CreateMediaFileDto
            {
                Name = String.Empty,
                MediaFileCategory = It.IsAny<MediaFileCategory>(),
                MediaFileType = It.IsAny<MediaFileType>(),
            };

            //Act
            var result = createMediaFileDto.Validate(out var errorMessage);

            //Assert
            Assert.False(result);
            Assert.Equal("Name cannot be empty", errorMessage);
        }

        [Fact]
        public void Validate_InvalidMediaFileCategory_ReturnsFalse()
        {
            //Arrange
            var createMediaFileDto = new CreateMediaFileDto
            {
                Name = "Test Name",
                MediaFileCategory = (MediaFileCategory)Int32.MaxValue,
                MediaFileType = It.IsAny<MediaFileType>(),
            };

            //Act
            var result = createMediaFileDto.Validate(out var errorMessage);

            //Assert
            Assert.False(result);
            Assert.Equal("Invalid Media File Category", errorMessage);
        }

        [Fact]
        public void Validate_InvalidMediaFileType_ReturnsFalse()
        {
            //Arrange
            var createMediaFileDto = new CreateMediaFileDto
            {
                Name = "Test name",
                MediaFileCategory = MediaFileCategory.HipHop,
                MediaFileType = (MediaFileType)Int32.MaxValue,
            };

            //Act
            var result = createMediaFileDto.Validate(out var errorMessage);

            //Assert
            Assert.False(result);
            Assert.Equal("Invalid Media File Type", errorMessage);
        }

        [Fact]
        public void Validate_ValidMediaFile_ReturnsTrue()
        {
            //Arrange
            var createMediaFileDto = new CreateMediaFileDto
            {
                Name = "Test Name",
                MediaFileCategory = MediaFileCategory.HipHop,
                MediaFileType = MediaFileType.Audio,
            };

            //Act
            var result = createMediaFileDto.Validate(out var errorMessage);

            //Assert
            Assert.True(result);
            Assert.Equal(String.Empty, errorMessage);
        }
    }
}
