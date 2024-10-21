using MediaPlayer.Domain;
using MediaPlayer.Service.AuthenticationService;
using MediaPlayer.Service.DTO.UserDTO;
using MediaPlayer.Service.LogService;
using MediaPlayer.Service.PlayTrackService;
using MediaPlayer.Service.UserService;

namespace MediaPlayer.Controller
{
    public class PlayTrackController
    {
        private readonly string _unauthorizedMessage = "You are not authorized to perform this action";
        private readonly IAuthenticationManagment _authManager;
        private readonly IPlayTrackManagment _playTrackManagement;
        private readonly ILogger _logger;

        public PlayTrackController(
            IUserManagement userManagement,
            IAuthenticationManagment authManager,
            IPlayTrackManagment playTrackManagement,
            ILogger logger
        )
        {
            this._authManager = authManager ?? throw new ArgumentNullException(nameof(authManager));
            this._playTrackManagement = playTrackManagement ?? throw new ArgumentNullException(nameof(playTrackManagement));
            this._logger = logger;
        }
        public void CreatePlayTrack(string name)
        {
            if (!IsUserAuthenticated(out var loggedInUser)) return;
            _playTrackManagement.CreatePlayTrack(_authManager.GetLoggedInUser()!.Id, name);
        }
        public void RemovePlayTrack(string id)
        {
            if (!IsUserAuthenticated(out var loggedInUser)) return;
            if (!Guid.TryParse(id, out Guid guid))
            {
                _logger.Log("Invalid play track ID found. Cannot delete the play track");
                return;
            }

            _playTrackManagement.RemovePlayTrack(loggedInUser.Id, guid);
        }
        public PlayTrack? GetPlayTrack(string playTrackId)
        {
            if (!IsUserAuthenticated(out var loggedInUser)) return null;
            if (Guid.TryParse(playTrackId, out Guid guid)) 
            {
                return _playTrackManagement.GetPlayTrack(loggedInUser!.Id, guid);
            }

            _logger.Log("Invalid play track ID found. Cannot get the play track");

            return null;
            
        }
        public void GetAllPlayTracks()
        {
            if (!IsUserAuthenticated(out var loggedInUser)) return;

            var playTracks = _playTrackManagement.GetAllPlayTracks(loggedInUser.Id);
            if (playTracks is null)
            {
                _logger.Log("No play tracks found");
                return;
            }
            foreach (var playTrack in playTracks) 
            {
                _logger.Log($"PlayTrack {playTrack.Id} | {playTrack.Name}");
                if (playTrack.MediaFiles.Count == 0)
                {
                    _logger.Equals("\t No media files found");
                    continue;
                }

                foreach (var mediaFile in playTrack.MediaFiles)
                {
                    _logger.Log($"\t MediaFile {mediaFile.Id} | {mediaFile.Name} | {mediaFile.GetType()}");
                }
            }
            
        }
        public void AddMediaFileToPlayTrack(string playTrackId, string mediaFileId)
        {
            if (!IsUserAuthenticated(out var loggedInUser)) return;

            if (!Guid.TryParse(playTrackId, out Guid playTrackGuid))
            {
                _logger.Log("Invalid play track ID found. Cannot add the media file to the play track");
                return;
            }
            if(!Guid.TryParse(mediaFileId, out Guid mediaFileGuid))
            {
                _logger.Log("Invalid media file ID found. Cannot add the media file to the play track");
                return;
            }

            _playTrackManagement.AddMediaFileToPlayTrack(loggedInUser.Id, playTrackGuid, mediaFileGuid);
            
        }
        public void RemoveMediaFileFromPlayTrack(string playTrackId, string mediaFileId)
        {
            if (!IsUserAuthenticated(out var loggedInUser)) return;
            if (!Guid.TryParse(playTrackId, out Guid playTrackGuid))
            {
                _logger.Log("Invalid play track ID found. Cannot remove the media file from the play track");
                return;
            }

            if(!Guid.TryParse(mediaFileId, out Guid mediaFileGuid))
            {
                _logger.Log("Invalid media file ID found. Cannot remove the media file from the play track");
                return;
            }
            _playTrackManagement.RemoveMediaFileFromPlayTrack(loggedInUser.Id, playTrackGuid, mediaFileGuid);
        }
        public void RemoveAllPlayTracks()
        {
            if (!IsUserAuthenticated(out var loggedInUser)) return;
            _playTrackManagement.RemoveAllPlayTracks(loggedInUser.Id);
        }
        public void UpdatePlayTrack(string playTrackId, string newName)
        {
            if (!IsUserAuthenticated(out var loggedInUser)) return;
            if (!Guid.TryParse(playTrackId, out Guid guid))
            {
                _logger.Log("Invalid play track ID found. Cannot update the play track");
                return;
            }

            _playTrackManagement.UpdatePlayTrack(loggedInUser.Id, guid, newName);
        }
        private ReadUserDto? GetLoggedInUser() => _authManager.GetLoggedInUser();
        public void PlayFileFromPlayTrack(string playTrackId, string mediaFileId) 
        {
            if (!IsUserAuthenticated(out var loggedInUser)) return;

            if (!Guid.TryParse(playTrackId, out Guid playTrackGuid))
            {
                _logger.Log("Invalid play track ID found. Cannot play the media file from the play track");
                return;
            }

            if(!Guid.TryParse(mediaFileId, out Guid mediaFileGuid))
            {
                _logger.Log("Invalid media file ID found. Cannot play the media file from the play track");
                return;
            }

            var playTrack = _playTrackManagement.GetPlayTrack(loggedInUser.Id, playTrackGuid);
            if (playTrack is null)
            {
                _logger.Log("PlayTrack not found");
                return;
            }

            var mediaFile = playTrack.MediaFiles.Find(x => x.Id == mediaFileGuid);
            if (mediaFile is null)
            {
                _logger.Log("MediaFile not found");
                return;
            }

            _logger.Log($"Playing {mediaFile.Name}");
            
        }
        private bool IsUserAuthenticated(out ReadUserDto? loggedInUser)
        {
            loggedInUser = _authManager.GetLoggedInUser();
            if (loggedInUser == null)
            {
                Console.WriteLine(_unauthorizedMessage);
                return false;
            }
            return true;
        }
    }
}
