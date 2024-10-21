using MediaPlayer.Domain;
using MediaPlayer.Service.LogService;
using MediaPlayer.Service.src.MediaService;
using MediaPlayer.Service.UserService;
using MediaPlyer.Domain.UserAggregate;

namespace MediaPlayer.Service.PlayTrackService
{
    public class PlayTrackManagment : IPlayTrackManagment
    {
        private readonly IUserRepository _userRepository;
        private readonly IMediaFileRepository _mediaFileRepository;
        private readonly ILogger _logger;

        public PlayTrackManagment(
            IUserRepository userRepository,
            IMediaFileRepository mediaFileRepository, 
            ILogger logger
        )
        {
            this._userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository)); 
            this._mediaFileRepository = mediaFileRepository ?? throw new ArgumentNullException(nameof(mediaFileRepository));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public bool AddMediaFileToPlayTrack(Guid userId, Guid playTrackId, Guid mediaFileId)
        {
            if (!TryGetUserAndPlayTrack(userId, playTrackId, out var user, out var playTrack))
                return false;

            var mediaFile = _mediaFileRepository.GetMediaFile(mediaFileId);
            if (mediaFile == null)
            {
                _logger.Log($"MediaFile with ID {mediaFileId} not found.");
                return false;
            }

            playTrack.MediaFiles.Add(mediaFile);
            _userRepository.UpdateUser(userId, user);

            _logger.Log("MediaFile added to PlayTrack successfully.");
            return true;
        }
        public PlayTrack CreatePlayTrack(Guid userId, string name)
        {
            var user = GetUserOrLogError(userId);
            if (user == null) return null;

            var newPlayTrack = new PlayTrack(name, user);
            user.PlayTracks.Add(newPlayTrack);

            _userRepository.UpdateUser(userId, user);

            _logger.Log($"PlayTrack '{name}' created successfully.");
            return newPlayTrack;
        }
        public List<PlayTrack>? GetAllPlayTracks(Guid userId)
        {
            var user = GetUserOrLogError(userId);
            return user?.PlayTracks;
        }
        public PlayTrack? GetPlayTrack(Guid userId, Guid playTrackId)
        {
            return TryGetUserAndPlayTrack(userId, playTrackId, out _, out var playTrack) ? playTrack : null;
        }
        public bool RemoveAllPlayTracks(Guid userId)
        {
            var user = GetUserOrLogError(userId);
            if (user == null) return false;

            user.PlayTracks.Clear();
            _userRepository.UpdateUser(userId, user);

            _logger.Log("All PlayTracks removed successfully.");
            return true;
        }
        public bool RemoveMediaFileFromPlayTrack(Guid userId, Guid playTrackId, Guid mediaFileId)
        {
            if (!TryGetUserAndPlayTrack(userId, playTrackId, out var user, out var playTrack))
                return false;

            var mediaFile = playTrack.MediaFiles.Find(x => x.Id == mediaFileId);
            if (mediaFile == null)
            {
                _logger.Log($"MediaFile with ID {mediaFileId} not found.");
                return false;
            }

            playTrack.MediaFiles.Remove(mediaFile);
            _userRepository.UpdateUser(userId, user);

            _logger.Log("MediaFile removed from PlayTrack successfully.");
            return true;
        }
        public bool RemovePlayTrack(Guid userId, Guid playTrackId)
        {
            if (!TryGetUserAndPlayTrack(userId, playTrackId, out var user, out var playTrack))
                return false;

            user.PlayTracks.Remove(playTrack);
            _userRepository.UpdateUser(userId, user);

            _logger.Log("PlayTrack removed successfully.");
            return true;
        }
        public bool UpdatePlayTrack(Guid userId, Guid playTrackId, string name)
        {
            if (!TryGetUserAndPlayTrack(userId, playTrackId, out var user, out var playTrack))
                return false;

            playTrack.Name = name;
            _userRepository.UpdateUser(userId, user);

            _logger.Log("PlayTrack updated successfully.");
            return true; 
        }
        private User? GetUserOrLogError(Guid userId)
        {
            var user = _userRepository.GetUser(userId);
            if (user == null)
            {
                _logger.Log($"User with ID {userId} not found.");
            }
            return user;
        }
        private bool TryGetUserAndPlayTrack(Guid userId, Guid playTrackId, out User? user, out PlayTrack? playTrack)
        {
            user = GetUserOrLogError(userId);
            if (user == null)
            {
                playTrack = null;
                return false;
            }

            playTrack = user.PlayTracks.Find(x => x.Id == playTrackId);
            if (playTrack == null)
            {
                _logger.Log($"PlayTrack with ID {playTrackId} not found.");
                return false;
            }

            return true;
        }
    }
}
