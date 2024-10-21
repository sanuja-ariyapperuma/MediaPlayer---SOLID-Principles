using MediaPlayer.Controller;
using MediaPlayer.Controller.src;
using MediaPlayer.Infrastrcture.Repository;
using MediaPlayer.Infrastrcture.src;
using MediaPlayer.Infrastrcture.src.Repository;
using MediaPlayer.Service.AuthenticationService;
using MediaPlayer.Service.PlaybackService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Infrastrcture.Commands
{
    public static class PlayBack
    {
        public static void Run()
        {
            var database = Database.GetInstance;
            var logger = Logger.GetInstance;
            var authRepo = new AuthenticationRepository(database);
            var userRepo = new UserRepository(database);
            var playBackRepo = new PlayBackRepository(database);
            var authManager = new AuthenticationManager(userRepo, authRepo, logger);
            var authController = new AuthenticationController(authManager);
            var playBackManagement = new PlaybackManagment(userRepo, playBackRepo, logger);
            var playBackController = new PlayBackController(authManager, playBackManagement, logger);

            Console.WriteLine("----------Trying to login----------");
            var loginUser = authController.Login("jeffry@gmail.com", "jeffry123");

            if (!loginUser)
            {
                return;
            }

            Console.WriteLine();
            Console.WriteLine("----------Trying to play file----------");
            Console.Write("Enter the file ID :");
            var fileId = Console.ReadLine();
            Console.Write("Enter the PlayTrack ID :");
            var playTrackId = Console.ReadLine();

            
            playBackController.Play(playTrackId, fileId);
            Console.WriteLine("----------Trying to pause the file----------");
            playBackController.Pause();
            Console.WriteLine("----------Trying to stop the file----------");
            playBackController.Stop();
            Console.WriteLine("----------Trying to set volume of the file----------");
            Console.Write("Enter the volume level :");
            var volume = Console.ReadLine();
            playBackController.SetVolume(volume);
            Console.WriteLine("----------Trying to set brightness of the file----------");
            Console.Write("Enter the brightness level :");
            var brightness = Console.ReadLine();
            playBackController.SetBrightness(brightness);

        }

    }
}
