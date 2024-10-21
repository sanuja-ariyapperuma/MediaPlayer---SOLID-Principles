using MediaPlayer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Service.PlayTrackService
{
    public interface IPlayTrackManagment
    {
        public PlayTrack CreatePlayTrack(Guid userId, string name);
        public bool RemovePlayTrack(Guid userId, Guid playTrackId);
        public bool AddMediaFileToPlayTrack(Guid userId, Guid playTrackId, Guid mediaFileId);
        public bool RemoveMediaFileFromPlayTrack(Guid userId, Guid playTrackId, Guid mediaFileId);
        public bool RemoveAllPlayTracks(Guid userId);
        public PlayTrack? GetPlayTrack(Guid userId, Guid playTrackId);
        public List<PlayTrack>? GetAllPlayTracks(Guid userId);
        public bool UpdatePlayTrack(Guid userId, Guid playTrackId, string name);
    }
}
