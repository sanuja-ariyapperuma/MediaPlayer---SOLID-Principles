using MediaPlayer.Domain.MediaFileAggregate.ValueObject;
using MediaPlyer.Domain.MediaFileAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Domain.src.MediaFileAggregate.Entity
{
    public class AudioFile : MediaFile
    {
        public AudioFile(string name, MediaFileCategory mediaFile)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.MediaFileCategory = mediaFile;
        }
        public override void Play() 
        {
            this.PlayStatus = MediaFileStatus.Playing;
            Console.WriteLine($"Playing Audio File {this.Name}");
        }
        public override void Stop() 
        {
            this.PlayStatus = MediaFileStatus.Stopped;
            Console.WriteLine($"Stopping Audio File {this.Name}");
        }
        public override void Pause() 
        {
            this.PlayStatus = MediaFileStatus.Paused;
            Console.WriteLine($"Pausing Audio File {this.Name}");
        } 

    }
}
