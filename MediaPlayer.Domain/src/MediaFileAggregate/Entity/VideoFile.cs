using MediaPlayer.Domain.MediaFileAggregate.ValueObject;
using MediaPlyer.Domain.MediaFileAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Domain.src.MediaFileAggregate.Entity
{
    public class VideoFile : MediaFile
    {
        public VideoFile(string name, MediaFileCategory mediaFile)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.MediaFileCategory = mediaFile;
        }

        public int Brightness { get; set; }

        public override void Play() {
            this.PlayStatus = MediaFileStatus.Playing;
            Console.WriteLine($"Playing Video File {this.Name}");
        }
        public override void Stop() 
        {
            this.PlayStatus = MediaFileStatus.Stopped;
            Console.WriteLine($"Stopping Video File {this.Name}");
        }
        public override void Pause() 
        {
            this.PlayStatus = MediaFileStatus.Paused;
            Console.WriteLine($"Pausing Video File {this.Name}");
        } 

        public void SetBrightness(int brightness)
        {
            this.Brightness = brightness;
            Console.WriteLine($"Brightness of the video {this.Name} is set to {this.Brightness}%");
        }
    }
}
