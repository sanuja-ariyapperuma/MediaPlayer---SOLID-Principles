using MediaPlayer.Service.src.PlaybackService;
using MediaPlyer.Domain.MediaFileAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Infrastrcture.src.Repository
{
    public class PlayBackRepository(Database database) : IPlayBackRepository
    {
        private MediaFile _currentPlayingFile = database.CurrentlyPlayingFile;

        public MediaFile GetCurrentPlayFile()
        {
            return _currentPlayingFile;
        }

        public void SetCurrentPlayFile(MediaFile playFile)
        {
            _currentPlayingFile = playFile;
        }
    }
}
