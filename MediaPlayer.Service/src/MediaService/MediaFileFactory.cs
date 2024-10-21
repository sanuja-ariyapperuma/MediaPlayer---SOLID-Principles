using MediaPlayer.Domain.MediaFileAggregate.ValueObject;
using MediaPlayer.Domain.src.MediaFileAggregate.Entity;
using MediaPlayer.Service.DTO.MediaFileDTO;
using MediaPlyer.Domain.MediaFileAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MediaPlayer.Service.src.MediaService
{
    public class MediaFileFactory : IMediaFileFactory
    {
        public MediaFile CreateMediaFile(CreateMediaFileDto mediaFileDto)
        {
            return mediaFileDto.MediaFileType switch
            {
                MediaFileType.Audio => new AudioFile(mediaFileDto.Name, mediaFileDto.MediaFileCategory),
                MediaFileType.Video => new VideoFile(mediaFileDto.Name, mediaFileDto.MediaFileCategory),
                _ => throw new Exception("Invalid Media File Type")
            };
        }
    }
}
