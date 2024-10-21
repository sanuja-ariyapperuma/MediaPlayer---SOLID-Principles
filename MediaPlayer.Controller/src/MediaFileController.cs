using MediaPlayer.Controller.src.helper;
using MediaPlayer.Service.AuthenticationService;
using MediaPlayer.Service.DTO.MediaFileDTO;
using MediaPlayer.Service.LogService;
using MediaPlayer.Service.UserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MediaPlayer.Controller.src
{
    public class MediaFileController
    {
        private readonly IMediaFileManagement _mediaFileManagement;
        private readonly IAuthenticationManagment _authManager;
        private readonly ILogger _logger;

        public MediaFileController(
            IMediaFileManagement mediaFileManagement, 
            IAuthenticationManagment authManager,
            ILogger logger)
        {
            this._mediaFileManagement = mediaFileManagement ?? throw new ArgumentException(nameof(mediaFileManagement));
            this._authManager = authManager ?? throw new ArgumentException(nameof(authManager));
            this._logger = logger;
        }
        public void AddMediaFile(CreateMediaFileDto mediaFile)
        {
            if (!IsAdminAuthorized()) return;
            if (!ValidateMediaFile(mediaFile)) return;

            _mediaFileManagement.AddMediaFile(mediaFile);

        }
        public void RemoveMediaFile(string mediaFileId)
        {
            if (!IsAdminAuthorized()) return;

            if (Guid.TryParse(mediaFileId, out Guid id)) 
            {
                _mediaFileManagement.RemoveMediaFile(id);
                return;
            }
            _logger.Log("Invalid media file ID found. Cannot delete the media file");
        }
        public string GetMediaFile(string mediaFileId)
        {

            if (Guid.TryParse(mediaFileId, out Guid id)) 
            {
                var mediaFile = _mediaFileManagement.GetMediaFile(id);

                if (mediaFile == null) return "Media file not found";

                var mediaFileJson = JsonSerializer.Serialize(mediaFile);
                return mediaFileJson;
            }

            return "Invalid media file ID found. Cannot update the media file";
        }
        public string GetAllMediaFiles()
        {
            var mediaFiles = _mediaFileManagement.GetAllMediaFiles();

            if(mediaFiles == null) return "No media files found";
            
            var mediaFilesJson = JsonSerializer.Serialize(mediaFiles);
            return mediaFilesJson;
        }
        public void UpdateMediaFile(string mediaFileId, CreateMediaFileDto updatedMediaFile)
        {
            if (!IsAdminAuthorized()) return;
            if (!ValidateMediaFile(updatedMediaFile)) return;

            if (Guid.TryParse(mediaFileId, out Guid id)) 
            {
                _mediaFileManagement.UpdateMediaFile(id, updatedMediaFile);
                return;
            }
            _logger.Log("Invalid media file ID found. Cannot update the media file");
        }
        private bool IsAdminAuthorized()
        {
            var user = _authManager.GetLoggedInUser();
            if (user == null || !Util.IsLoggedInAdmin(user))
            {
                Console.WriteLine(Util.unauthorizedMessage);
                return false;
            }
            return true;
        }
        private bool ValidateMediaFile(CreateMediaFileDto mediaFile)
        {
            if (mediaFile.Validate(out string errorMessage)) return true;

            Console.WriteLine(errorMessage);
            return false;
        }

    }
}
