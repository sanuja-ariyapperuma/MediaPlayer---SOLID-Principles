using MediaPlyer.Domain.MediaFileAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Service.src.PlaybackService
{
    public interface IPlayBackRepository
    {
        public void SetCurrentPlayFile(MediaFile playFile);
        public MediaFile GetCurrentPlayFile();
    }
}
