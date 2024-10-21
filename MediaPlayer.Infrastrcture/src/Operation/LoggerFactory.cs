using MediaPlayer.Service.LogService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Infrastrcture.src.Operation
{
    public class LoggerFactory
    {
        public static ILogger CreateLogger(string type)
        {
            return type.ToLower() switch
            {
                "console" => new ConsoleLogger(),
                _ => throw new ArgumentException("Invalid logger type")
            };
        }
    }
}
