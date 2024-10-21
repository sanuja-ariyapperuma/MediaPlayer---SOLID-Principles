

using MediaPlayer.Infrastrcture.Commands;

UserCRUD.Run();
MediaFileCRUD.Run();
PlayTrackCRUD.Run();
PlayBack.Run();

var userInput = "";

while (userInput != "exit")
{
   Console.WriteLine("-------------Welcome to HighFi media player-------------");
   Console.WriteLine("1. User CRUD operations");
   Console.WriteLine("2. MediaFile CRUD operations");
   Console.WriteLine("3. PlayTrack CRUD operations");
   Console.WriteLine("4. PlayBack operations");
   Console.WriteLine("Type 'exit' to exit the application");
   Console.WriteLine();
   Console.Write("Enter your choice : ");
   userInput = Console.ReadLine();

   switch (userInput)
   {
       case "1":
           UserCRUD.Run();
           break;
       case "2":
           MediaFileCRUD.Run();
           break;
       case "3":
           PlayTrackCRUD.Run();
           break;
       case "4":
           PlayBack.Run();
           break;
       case "exit":
           break;
       default:
           Console.WriteLine("Invalid input");
           break;
   }
}



