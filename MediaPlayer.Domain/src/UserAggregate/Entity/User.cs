using MediaPlayer.Domain;

namespace MediaPlyer.Domain.UserAggregate
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public UserCategory UserCategory { get; set; }
        public List<PlayTrack> PlayTracks { get; set; } = [];

        public User(string name, string email, string password, UserCategory userCategory)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            Password = password;
            UserCategory = userCategory;

        }
    }

}