# Media Player Application - SOLID Principles | Clean Architecture | Unit Testing

This is a console application built with dotnet 8 and C# representing the functionalities of a media player. Solution done according to 

- SOLID principles
- Clean architecture
- Unit Testing (xUnit)
- Factory pattern - Logging, Dynamic Media File Creation
- Singleton pattern - Database instance (In Memory), Logging instance
- object lifetime
- Thread safety - Logging, Database

## Features

- Each user could create their own playtracks and perform other actions on the media file in their playtracks.
- Only Admins of the application should be able to add, remove, update, delete all users in the application.
- Only Admins of the application should be able to add, remove, update, delete all the files in the application.
- Users should be able to manage their playtracks, including adding, removing, playing, pausing, stopping the media files.
- Videos can change volume, brightness
- Audios can change volume, sound effect
- Handle potential errors and exceptions gracefully, providing meaningful error messages to the user.
- While all instances of database, repositories, services, controllers could be initiated in Program.cs, all functionalities should be handled via controllers only.
- Unit tests for Service layer
