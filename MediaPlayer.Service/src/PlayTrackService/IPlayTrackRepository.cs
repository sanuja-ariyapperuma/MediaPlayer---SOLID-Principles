using MediaPlayer.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Service.src.PlayTrackService
{
    public interface IPlayTrackRepository
    {
        public void CreatePlayTrack(User user, PlayTrack playTrack);
        public void RemovePlayTrack(Guid userId, Guid playTrackId);
        public PlayTrack? GetPlayTrack(Guid userId, Guid playTrackId);
        public List<PlayTrack>? GetAllPlayTracks(Guid userId);
        public void AddMediaFileToPlayTrack(Guid userId, Guid playTrackId, Guid mediaFileId);
        public void RemoveMediaFileFromPlayTrack(Guid userId, Guid playTrackId, Guid mediaFileId);
        public void RemoveAllPlayTracks(Guid userId);
    }
}
