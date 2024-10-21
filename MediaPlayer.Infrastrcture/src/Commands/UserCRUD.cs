using MediaPlayer.Controller;
using MediaPlayer.Infrastrcture.Repository;
using MediaPlayer.Service.UserService;
using MediaPlayer.Service.AuthenticationService;
using MediaPlayer.Service.DTO.UserDTO;
using MediaPlayer.Infrastrcture.src;

namespace MediaPlayer.Infrastrcture.Commands
{
    public static class UserCRUD
    {
        public static void Run()
        {
            var database = Database.GetInstance;
            var logger = Logger.GetInstance;
            var userRepo = new UserRepository(database);
            var authRepo = new AuthenticationRepository(database);
            var userManagement = new UserManagement(userRepo, logger);
            var authManager = new AuthenticationManager(userRepo, authRepo, logger);
            var userController = new UserController(userManagement, authManager, logger);
            var authController = new AuthenticationController(authManager);

            //Login with default user
            Console.WriteLine("----------Trying to login (Admin account)----------");
            var loginUser = authController.Login("john@gmail.com", "john123");

            if (!loginUser)
            {
                return;
            }
            Console.WriteLine();
            Console.WriteLine("----------Trying to Add new user from admin account----------");
            // Adding admin user creation
            var user1 = new CreateUserDto()
            {
                Email = "Kane@gmail.com",
                Name = "Kane",
                UserCategory = 2
            };


            userController.AddUser(user1);
            PrintAllUser();

            Console.WriteLine();
            Console.WriteLine("----------Trying to remove user from admin account----------");

            Console.Write("Enter the id of the user to remove : ");
            var userId = Console.ReadLine();
            userController.RemoveUser(userId);

            PrintAllUser();

            Console.WriteLine();
            Console.WriteLine("----------Trying to update user from admin account----------");

            Console.Write("Enter the id of the user to update : ");
            userId = Console.ReadLine();

            var updated = new CreateUserDto()
            {
                Email = "Tony@gmail.com",
                Name = "Tony",
                UserCategory = 2
            };

            userController.UpdateUser(userId, updated);

            PrintAllUser();

            Console.WriteLine();
            authManager.Logout();

            Console.WriteLine("----------Trying to login (Non admin account)----------");
            loginUser = authController.Login("jeffry@gmail.com", "jeffry123");

            Console.WriteLine();
            Console.WriteLine("----------Trying to Add new user from non admin account----------");

            // Adding admin user creation
            user1 = new CreateUserDto()
            {
                Email = "Kane@gmail.com",
                Name = "Kane",

                UserCategory = 2
            };
            userController.AddUser(user1);


            void PrintAllUser()
            {
                Console.WriteLine();
                Console.WriteLine("----------Retriving all users to check operation----------");
                var users = userController.GetAllUsers();
                Console.WriteLine(users);
            }
        }
            
    }
}
