using MediaPlayer.Domain.MediaFileAggregate.ValueObject;
using MediaPlayer.Domain.src.MediaFileAggregate.Entity;
using MediaPlyer.Domain.MediaFileAggregate;

namespace MediaPlayer.Service.DTO.MediaFileDTO
{
    public class CreateMediaFileDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public MediaFileCategory MediaFileCategory { get; set; }
        public MediaFileType MediaFileType { get; set; }

        public bool Validate(out string errorMessage)
        {
            if (string.IsNullOrEmpty(Name)) 
            {
                errorMessage = "Name cannot be empty";
                return false;
            }

            if (!Enum.IsDefined(typeof(MediaFileCategory), MediaFileCategory)) 
            {
                errorMessage = "Invalid Media File Category";
                return false;
            }

            if (!Enum.IsDefined(typeof(MediaFileType), MediaFileType))
            {
                errorMessage = "Invalid Media File Type";
                return false;
            }

            errorMessage = String.Empty;
            return true;
        }
    }
}
