using MediaPlayer.Domain.MediaFileAggregate.ValueObject;

namespace MediaPlyer.Domain.MediaFileAggregate
{
    public abstract class MediaFile : BaseEntity
    {
        public MediaFileCategory MediaFileCategory { get; set; }
        //public MediaFileType MediaFileType { get; set; }
        public int Volume { get; set; }
        public MediaFileStatus PlayStatus { get; set; }
        
        public virtual void Play() { }
        public virtual void Stop() { }
        public virtual void Pause() { }

        //Volume can be set on both audio and video files
        public void SetVolume(int volume)
        {
            this.Volume = volume;
            Console.WriteLine($"Volume of the file {this.Name} is set to {this.Volume}%");

        }
    }

}