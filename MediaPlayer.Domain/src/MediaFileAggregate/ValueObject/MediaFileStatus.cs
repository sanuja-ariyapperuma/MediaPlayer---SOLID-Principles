using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Domain.MediaFileAggregate.ValueObject
{
    public enum MediaFileStatus
    {
        Stopped = 1,
        Playing = 2,
        Paused = 3
    }
}
