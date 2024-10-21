using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Service.PlaybackService
{
    public interface IPlaybackManagment
    {
        public void Play(Guid userId, Guid playTrackId, Guid mediaFileId);
        public void Stop();
        public void Pause();
        public void SetVolume(int volume);
        public void SetBrightness(int brightness);
    }
}
