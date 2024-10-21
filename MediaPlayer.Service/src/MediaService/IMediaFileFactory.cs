using MediaPlayer.Service.DTO.MediaFileDTO;
using MediaPlyer.Domain.MediaFileAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Service.src.MediaService
{
    public interface IMediaFileFactory
    {
        public MediaFile CreateMediaFile(CreateMediaFileDto mediaFileDto);
    }
}
