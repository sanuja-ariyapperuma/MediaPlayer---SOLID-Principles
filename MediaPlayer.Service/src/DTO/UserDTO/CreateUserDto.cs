using MediaPlayer.Domain;
using MediaPlayer.Service.src.Helpers;
using MediaPlyer.Domain.MediaFileAggregate;
using MediaPlyer.Domain.UserAggregate;

namespace MediaPlayer.Service.DTO.UserDTO
{
    public class CreateUserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public int UserCategory { get; set; }
        public List<PlayTrack>? PlayTracks { get; set; }

        public User Convert() => new(this.Name, this.Email, Hash.ComputeSha256Hash(this.Password) , (UserCategory)UserCategory);
        
    }
}
