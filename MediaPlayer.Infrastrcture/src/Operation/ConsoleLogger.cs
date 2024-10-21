using MediaPlayer.Service.LogService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Infrastrcture.src.Operation
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message) => Console.WriteLine(message);

    }
}
