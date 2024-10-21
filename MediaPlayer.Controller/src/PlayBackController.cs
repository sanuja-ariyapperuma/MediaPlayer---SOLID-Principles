using MediaPlayer.Controller.src.helper;
using MediaPlayer.Service.AuthenticationService;
using MediaPlayer.Service.DTO.UserDTO;
using MediaPlayer.Service.LogService;
using MediaPlayer.Service.PlaybackService;
using MediaPlyer.Domain.UserAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Controller.src
{
    public class PlayBackController
    {
        private readonly IAuthenticationManagment _authManager;
        private readonly IPlaybackManagment _playbackManagement;
        private readonly ILogger _logger;

        public PlayBackController(
            IAuthenticationManagment authManager,
            IPlaybackManagment playbackManagement,
            ILogger logger
        )
        {
            this._authManager = authManager ?? throw new ArgumentNullException(nameof(authManager));
            this._playbackManagement = playbackManagement ?? throw new ArgumentNullException(nameof(playbackManagement));
            this._logger = logger;
        }
        public void Play(string PlayTrackId, string mediaFileId)
        {

            if (!IsUserAuthenticated(out var loggedInUser)) return;

            if (
                Guid.TryParse(PlayTrackId, out Guid playTrackId) && 
                Guid.TryParse(mediaFileId, out Guid mediaId))
            {
                _playbackManagement.Play(loggedInUser!.Id, playTrackId, mediaId);
                return;
            }
            _logger.Log("Invalid type of data provided");

        }
        public void Pause()
        {
            if (!IsUserAuthenticated()) return;

            _playbackManagement.Pause();
        }
        public void Stop()
        {
            if (!IsUserAuthenticated()) return;

            _playbackManagement.Stop();
        }
        public void SetVolume(string volume)
        {
            if (!IsUserAuthenticated()) return;

            if (int.TryParse(volume, out int vol))
            {
                _playbackManagement.SetVolume(vol);
                return;
            }
            _logger.Log("Invalid volume level provided");
        }

        public void SetBrightness(string brightness)
        {
            if (!IsUserAuthenticated()) return;

            if (int.TryParse(brightness, out int vol))
            {
                _playbackManagement.SetBrightness(vol);
                return;
            }
            _logger.Log("Invalid birghtness level provided");
        }

        private bool IsUserAuthenticated(out ReadUserDto? loggedInUser)
        {
            loggedInUser = _authManager.GetLoggedInUser();
            if (loggedInUser == null)
            {
                Console.WriteLine(Util.unauthorizedMessage);
                return false;
            }
            return true;
        }
        private bool IsUserAuthenticated()
        {
            return IsUserAuthenticated(out _);
        }

    }
}
