using MediaPlayer.Service.src.MediaService;
using MediaPlyer.Domain.MediaFileAggregate;
using MediaPlyer.Domain.UserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Infrastrcture.src.Repository
{
    public class MediaFileRepository(Database database) : IMediaFileRepository
    {
        private readonly List<MediaFile> _mediaFiles = database.MediaFiles;
        private MediaFile _currentPlayingFile = database.CurrentlyPlayingFile;

        public void AddMediaFile(MediaFile mediaFile) => _mediaFiles.Add(mediaFile);

        public List<MediaFile>? GetAllMediaFiles() => _mediaFiles;

        public MediaFile? GetMediaFile(Guid mediaFileId) => _mediaFiles.Find(x => x.Id == mediaFileId);
        public void RemoveAllMediaFile() => _mediaFiles.Clear();
        public void RemoveMediaFile(Guid mediaFileId) => _mediaFiles.RemoveAll(x => x.Id == mediaFileId);
        public void UpdateMediaFile(Guid mediaFileId, MediaFile updatedMediaFile)
        {
            var mediaFile = _mediaFiles.Find(x => x.Id == mediaFileId);
            
            mediaFile.MediaFileCategory = updatedMediaFile.MediaFileCategory;
            mediaFile.Name = updatedMediaFile.Name;

        }
    }
}
