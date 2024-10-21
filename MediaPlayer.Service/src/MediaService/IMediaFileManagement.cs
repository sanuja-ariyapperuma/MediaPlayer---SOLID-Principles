using MediaPlayer.Service.DTO.MediaFileDTO;
using MediaPlyer.Domain.MediaFileAggregate;
using MediaPlyer.Domain.UserAggregate;

namespace MediaPlayer.Service.UserService
{
    public interface IMediaFileManagement
    {
        public CreateMediaFileDto? AddMediaFile(CreateMediaFileDto mediaFile);
        public void RemoveMediaFile(Guid mediaFileId);
        public MediaFile? GetMediaFile(Guid mediaFileId);
        public List<MediaFile>? GetAllMediaFiles();
        public bool UpdateMediaFile(Guid mediaFileId, CreateMediaFileDto updatedMediaFile);
        public void RemoveAllMediaFile();
    }
}