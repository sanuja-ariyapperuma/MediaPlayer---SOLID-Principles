using MediaPlayer.Controller;
using MediaPlayer.Infrastrcture.Repository;
using MediaPlayer.Infrastrcture.src;
using MediaPlayer.Infrastrcture.src.Repository;
using MediaPlayer.Service.AuthenticationService;
using MediaPlayer.Service.PlayTrackService;
using MediaPlayer.Service.UserService;

namespace MediaPlayer.Infrastrcture.Commands
{
    public static class PlayTrackCRUD
    {
        public static void Run()
        {
            var database = Database.GetInstance;
            var logger = Logger.GetInstance;
            var userRepo = new UserRepository(database);
            var authRepo = new AuthenticationRepository(database);
            var authManager = new AuthenticationManager(userRepo, authRepo, logger);
            var authController = new AuthenticationController(authManager);
            var userManagement = new UserManagement(userRepo, logger);
            var mediaFileRepo = new MediaFileRepository(database);
            var playTrackManagement = new PlayTrackManagment(userRepo, mediaFileRepo, logger);
            var playTrackController = new PlayTrackController(userManagement, authManager, playTrackManagement, logger);

            //Login with default user
            Console.WriteLine("----------Trying to login (Admin account)----------");
            var loginUser = authController.Login("jeffry@gmail.com", "jeffry123");

            if (!loginUser)
            {
                return;
            }

            //Create new PlayTrack
            Console.WriteLine();
            Console.WriteLine("----------Trying to create new PlayTrack----------");
            playTrackController.CreatePlayTrack("My PlayTrack 1");
            playTrackController.CreatePlayTrack("My PlayTrack 2");

            PrintAllPlayTracks();

            //Remove PlayTrack
            Console.WriteLine();
            Console.WriteLine("----------Trying to remove PlayTrack----------");
            Console.Write("Enter the PlayTrack ID you wish to remove : ");
            var playTrackId = Console.ReadLine();
            playTrackController.RemovePlayTrack(playTrackId);

            PrintAllPlayTracks();

            //Rename PlayTrack
            Console.WriteLine();
            Console.WriteLine("----------Trying to rename PlayTrack----------");
            Console.Write("Enter the PlayTrack ID you wish to rename : ");
            playTrackId = Console.ReadLine();
            Console.Write("Enter the new name : ");
            var newName = Console.ReadLine();
            playTrackController.UpdatePlayTrack(playTrackId, newName);

            PrintAllPlayTracks();

            //Add media file to PlayTrack
            Console.WriteLine();
            Console.WriteLine("----------Trying to add media file to PlayTrack----------");
            Console.Write("Enter the PlayTrack ID you wish to add the file : ");
            playTrackId = Console.ReadLine();
            Console.Write("Enter the media file ID you wish to add : ");
            var mediaFileId = Console.ReadLine();
            playTrackController.AddMediaFileToPlayTrack(playTrackId, mediaFileId);

            PrintAllPlayTracks();




            //Remove media file from PlayTrack
            Console.WriteLine();
            Console.WriteLine("----------Trying to remove media file from PlayTrack----------");
            Console.Write("Enter the PlayTrack ID you wish to remove the file : ");
            playTrackId = Console.ReadLine();
            Console.Write("Enter the media file ID you wish to remove : ");
            mediaFileId = Console.ReadLine();
            playTrackController.RemoveMediaFileFromPlayTrack(playTrackId, mediaFileId);



            void PrintAllPlayTracks()
            {
                Console.WriteLine();
                Console.WriteLine("----------All available playlists----------");
                playTrackController.GetAllPlayTracks();
            }

            PrintAllPlayTracks();


        }

        
    }
}
