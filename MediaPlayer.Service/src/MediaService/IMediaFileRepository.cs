using MediaPlyer.Domain.MediaFileAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Service.src.MediaService
{
    public interface IMediaFileRepository
    {
        public void AddMediaFile(MediaFile mediaFile);
        public void RemoveMediaFile(Guid mediaFileId);
        public MediaFile? GetMediaFile(Guid mediaFileId);
        public List<MediaFile>? GetAllMediaFiles();
        public void UpdateMediaFile(Guid mediaFileId, MediaFile updatedMediaFile);
        public void RemoveAllMediaFile();
    }
}
