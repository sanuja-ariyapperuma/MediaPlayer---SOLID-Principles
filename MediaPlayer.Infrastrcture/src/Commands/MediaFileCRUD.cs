using MediaPlayer.Controller;
using MediaPlayer.Infrastrcture.Repository;
using MediaPlayer.Service.AuthenticationService;
using MediaPlayer.Infrastrcture.src.Repository;
using MediaPlayer.Service.MediaService;
using MediaPlayer.Controller.src;
using MediaPlayer.Service.DTO.MediaFileDTO;
using MediaPlayer.Domain.MediaFileAggregate.ValueObject;
using MediaPlayer.Infrastrcture.src;
using MediaPlayer.Service.src.MediaService;

namespace MediaPlayer.Infrastrcture.Commands
{
    public static class MediaFileCRUD
    {
        public static void Run() 
        {
            var database = Database.GetInstance;
            var logger = Logger.GetInstance;
            var mediaFileRepo = new MediaFileRepository(database);
            var authRepo = new AuthenticationRepository(database);
            var userRepo = new UserRepository(database);
            var mediaFileFactory = new MediaFileFactory();
            var mediaFileManagement = new MediaFileManagement(mediaFileRepo, logger, mediaFileFactory);
            var authManager = new AuthenticationManager(userRepo, authRepo, logger);
            var mediaFileController = new MediaFileController(mediaFileManagement, authManager, logger);
            var authController = new AuthenticationController(authManager);

            //Login with default user
            Console.WriteLine("----------Trying to login (Admin account)----------");
            var loginUser = authController.Login("john@gmail.com", "john123");

            if (!loginUser)
            {
                return;
            }

            Console.WriteLine();
            Console.WriteLine("----------Trying to Add new media files from admin account----------");
            // Adding media file
            var mediaFile1 = new CreateMediaFileDto()
            {
                Name = "AudioMusicFile.mp3",
                MediaFileCategory = MediaFileCategory.HipHop,
                MediaFileType = MediaFileType.Audio
            };
            var mediaFile2 = new CreateMediaFileDto()
            {
                Name = "VideMusicFile.mp4",
                MediaFileCategory = MediaFileCategory.Classical,
                MediaFileType = MediaFileType.Video
            };
            var mediaFile3 = new CreateMediaFileDto()
            {
                Name = "AudioMusicFile2.mp3",
                MediaFileCategory = MediaFileCategory.Jazz,
                MediaFileType = MediaFileType.Audio
            };
            var mediaFile4 = new CreateMediaFileDto()
            {
                Name = "AudioMusicFile3.mp3",
                MediaFileCategory = MediaFileCategory.Other,
                MediaFileType = MediaFileType.Audio
            };
            var mediaFile5 = new CreateMediaFileDto()
            {
                Name = "VideMusicFile2.mp4",
                MediaFileCategory = MediaFileCategory.Rock,
                MediaFileType = MediaFileType.Video
            };

            mediaFileController.AddMediaFile(mediaFile1);
            mediaFileController.AddMediaFile(mediaFile2);
            mediaFileController.AddMediaFile(mediaFile3);
            mediaFileController.AddMediaFile(mediaFile4);
            mediaFileController.AddMediaFile(mediaFile5);

            PrintAllMediaFiles();

            Console.WriteLine();
            Console.WriteLine("----------Trying to remove media file from admin account----------");

            Console.Write("Enter the id of the media file to remove : ");
            var mediaFileId = Console.ReadLine();
            mediaFileController.RemoveMediaFile(mediaFileId);

            PrintAllMediaFiles();

            Console.WriteLine();
            Console.WriteLine("----------Trying to update media file from admin account----------");

            Console.Write("Enter the id of the media file to update : ");
            mediaFileId = Console.ReadLine();

            var updated = new CreateMediaFileDto()
            {
                Name = "VideMusicFile5.mp4",
                MediaFileCategory = MediaFileCategory.HipHop,
                MediaFileType = MediaFileType.Video
            };

            mediaFileController.UpdateMediaFile(mediaFileId, updated);

            PrintAllMediaFiles();

            Console.WriteLine();
            authManager.Logout();

            Console.WriteLine("----------Trying to login (Non admin account)----------");
            loginUser = authController.Login("jeffry@gmail.com", "jeffry123");

            if (!loginUser)
            {
                return;
            }

            Console.WriteLine();
            Console.WriteLine("----------Trying to Add new media file from non admin account----------");

            
            mediaFile5 = new CreateMediaFileDto()
            {
                Name = "AudieMusicFile10.mp3",
                MediaFileCategory = MediaFileCategory.Classical,
                MediaFileType = MediaFileType.Audio
            };

            // Adding media file from non admin account
            mediaFileController.AddMediaFile(mediaFile5);

            void PrintAllMediaFiles()
            {
                Console.WriteLine();
                Console.WriteLine("----------Retriving all media files to check operation----------");
                var mediafiles = mediaFileController.GetAllMediaFiles();
                Console.WriteLine(mediafiles);
            }


        }
    }
}
