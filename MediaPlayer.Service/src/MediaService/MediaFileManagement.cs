using MediaPlayer.Domain.MediaFileAggregate.ValueObject;
using MediaPlayer.Service.DTO.MediaFileDTO;
using MediaPlayer.Service.LogService;
using MediaPlayer.Service.src.MediaService;
using MediaPlayer.Service.UserService;
using MediaPlyer.Domain.MediaFileAggregate;

namespace MediaPlayer.Service.MediaService
{
    public class MediaFileManagement : IMediaFileManagement
    {
        private readonly IMediaFileRepository _mediaFileRepository;
        private readonly ILogger _logger;
        private readonly IMediaFileFactory _fileFactory;

        public MediaFileManagement(
            IMediaFileRepository mediaFileRepository,
            ILogger logger,
            IMediaFileFactory fileFactory)
        {
            this._mediaFileRepository = mediaFileRepository ?? throw new ArgumentNullException(nameof(mediaFileRepository));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
        }
        public CreateMediaFileDto? AddMediaFile(CreateMediaFileDto mediaFile)
        {
            try
            {
                if (mediaFile == null)
                {
                    _logger.Log("Media file cannot be null");
                    return null;
                }

                if (!mediaFile.Validate(out var validationError))
                {
                    _logger.Log(validationError);
                    return null;
                }

                var file = _fileFactory.CreateMediaFile(mediaFile);
                _mediaFileRepository.AddMediaFile(file);

                mediaFile.Id = file.Id;
                _logger.Log($"Media file with ID {file.Id} added successfully");

                return mediaFile;
            }
            catch (Exception e)
            {
                _logger.Log(e.Message);
                return null;
            }
            
        }
        public List<MediaFile>? GetAllMediaFiles() => _mediaFileRepository.GetAllMediaFiles();
        public MediaFile? GetMediaFile(Guid mediaFileId) => _mediaFileRepository.GetMediaFile(mediaFileId);
        public void RemoveAllMediaFile() => _mediaFileRepository.RemoveAllMediaFile();
        public void RemoveMediaFile(Guid mediaFileId) 
        {
            var mediaFile = _mediaFileRepository.GetMediaFile(mediaFileId);

            if (mediaFile == null) 
            {
                _logger.Log("Media file not found");
                return;
            }

            _mediaFileRepository.RemoveMediaFile(mediaFileId);

        } 
        public bool UpdateMediaFile(Guid mediaFileId, CreateMediaFileDto updatedMediaFile)
        {
            var existingFile = _mediaFileRepository.GetMediaFile(mediaFileId);
            if (existingFile == null)
            {
                Console.WriteLine("File not found");
                return false;
            }

            if (!updatedMediaFile.Validate(out var validationError))
            {
                Console.WriteLine(validationError);
                return false;
            }

            existingFile.Name = updatedMediaFile.Name;

            _mediaFileRepository.UpdateMediaFile(mediaFileId, existingFile);
            Console.WriteLine($"Media file with ID {mediaFileId} updated successfully");

            return true;
        }
    }
}
