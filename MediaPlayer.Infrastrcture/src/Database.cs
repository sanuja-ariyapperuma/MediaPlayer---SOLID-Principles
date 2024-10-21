using MediaPlyer.Domain.MediaFileAggregate;
using MediaPlyer.Domain.UserAggregate;

namespace MediaPlayer.Infrastrcture
{
    public class Database
    {
        //Default users
        public List<User> Users { get; set; } =
        [
            new User("John Doe", "john@gmail.com", "john123", UserCategory.Admin),
            new User("Jeffry Sinh", "jeffry@gmail.com", "jeffry123", UserCategory.User)
        ];
        public User LoggedInUser { get; set; }

        public MediaFile CurrentlyPlayingFile { get; set; }

        public List<MediaFile> MediaFiles { get; set; } = [];

        private static Database _databaseInstance;

        private static readonly object padlock = new();

        private Database()
        {
            // Prevent creation through reflection
            if (_databaseInstance != null)
            {
                throw new InvalidOperationException("Singleton instance already exists.");
            }
        }

        public static Database GetInstance
        {
            get
            {
                // Prevents multiple threads from creating multiple instances
                lock (padlock)
                {
                    _databaseInstance ??= new();
                    return _databaseInstance;
                }
            }
        }

        // Prevents the instance from being cloned
        public object Clone()
        {
            throw new NotSupportedException("Cloning of singleton instance is not allowed.");
        }

        // Prevents the instance from being deserialized
        [System.Runtime.Serialization.OnDeserialized]
        private void OnDeserialized(System.Runtime.Serialization.StreamingContext context)
        {
            throw new NotSupportedException("Deserialization of singleton instance is not allowed.");
        }

    }
}
