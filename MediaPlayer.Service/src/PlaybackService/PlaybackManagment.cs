using MediaPlayer.Domain.src.MediaFileAggregate.Entity;
using MediaPlayer.Service.LogService;
using MediaPlayer.Service.src.MediaService;
using MediaPlayer.Service.src.PlaybackService;
using MediaPlayer.Service.UserService;
using MediaPlyer.Domain.MediaFileAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Service.PlaybackService
{
    public class PlaybackManagment 
        : IPlaybackManagment
    {
        private readonly IUserRepository _userRepository;
        private readonly IPlayBackRepository _playBackRepository;
        private readonly ILogger _logger;

        public PlaybackManagment(
            IUserRepository userRepository, 
            IPlayBackRepository playBackRepository,
            ILogger logger
            )
        {
            this._userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this._playBackRepository = playBackRepository ?? throw new ArgumentNullException(nameof(playBackRepository));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public void Play(Guid userId, Guid playTrackId, Guid mediaFileId)
        {
            
            
            var mediaFile = GetMediaFile(userId, playTrackId, mediaFileId);
            if (mediaFile == null) return;
            
            var currentPlayingFile = _playBackRepository.GetCurrentPlayFile();

            if (currentPlayingFile == null || currentPlayingFile!.Id != mediaFile.Id) 
            {
                if (currentPlayingFile != null) currentPlayingFile.Stop();
                _playBackRepository.SetCurrentPlayFile(mediaFile);
                currentPlayingFile = _playBackRepository.GetCurrentPlayFile();
            }

            currentPlayingFile.Play();
        }
        public void Stop() 
        {
            var currentPlayingFile = _playBackRepository.GetCurrentPlayFile();

            if (currentPlayingFile == null) 
            {
                _logger.Log("There is no current files are playing to stop");
                return;
            }

            currentPlayingFile.Stop();
       
        }
        public void Pause() 
        {
            var currentPlayingFile = _playBackRepository.GetCurrentPlayFile();

            if (currentPlayingFile == null)
            {
                _logger.Log("There is no current files are playing to stop");
                return;
            }

            currentPlayingFile.Pause();
        }
        public void SetVolume(int volume) 
        {
            if (!IsValidRange(volume, 0, 100, nameof(volume))) 
            {
                _logger.Log("Volume should be between 0 and 100.");
                return;
            } 

            var currentPlayingFile = GetPlayingFile();

            if (currentPlayingFile == null) return;
            

            currentPlayingFile.SetVolume(volume);

            _logger.Log($"Volume set to {volume}.");
        }
        public void SetBrightness(int brightness)
        {
            if (!IsValidRange(brightness, 0, 100, nameof(brightness))) 
            {
                _logger.Log("Brightness should be between 0 and 100.");
                return;
            }

            var currentPlayingFile = GetPlayingFile();

            if (currentPlayingFile == null) return;

            if (currentPlayingFile is not VideoFile videoFile)
            {
                _logger.Log("Brightness setting only valid for video files");
                return;
            }

            videoFile.SetBrightness(brightness);

            _logger.Log($"Brightness set to {brightness}.");
        }
        private MediaFile? GetMediaFile(Guid userId, Guid playTrackId, Guid mediaFileId)
        {
            var user = _userRepository.GetUser(userId);
            if (user == null)
            {
                _logger.Log($"User not found with ID {userId}.");
                return null;
            }

            var playTrack = user.PlayTracks.Find(x => x.Id == playTrackId);
            if (playTrack == null)
            {
                _logger.Log($"PlayTrack not found with ID {playTrackId}.");
                return null;
            }

            var mediaFile = playTrack.MediaFiles.Find(x => x.Id == mediaFileId);

            if (mediaFile == null)
            {
                _logger.Log($"MediaFile not found with ID {mediaFileId}.");
                return null;
            }

            return mediaFile;
        }
        private MediaFile? GetPlayingFile()
        {
            var currentPlayingFile = _playBackRepository.GetCurrentPlayFile();
            if (currentPlayingFile == null) _logger.Log("No files are currently playing.");


            return currentPlayingFile;
        }
        private bool IsValidRange(int value, int min, int max, string paramName)
        {
            if (value < min || value > max)
            {
                _logger.Log($"Invalid {paramName} value: {value}. Must be between {min} and {max}.");
                return false;
            }
            return true;
        }
    }
}
