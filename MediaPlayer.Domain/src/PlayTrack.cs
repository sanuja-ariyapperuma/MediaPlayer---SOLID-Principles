using MediaPlyer.Domain;
using MediaPlyer.Domain.MediaFileAggregate;
using MediaPlyer.Domain.UserAggregate;

namespace MediaPlayer.Domain
{
    public class PlayTrack : BaseEntity
    {
        public List<MediaFile> MediaFiles { get; set; } = [];

        public User Owner { get; set; }

        public PlayTrack(string name, User owner)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.Owner = owner;
        }
    }
}