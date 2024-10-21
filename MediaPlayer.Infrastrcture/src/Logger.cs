using MediaPlayer.Infrastrcture.src.Operation;
using MediaPlayer.Service.LogService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Infrastrcture.src
{
    public class Logger
    {
        //This is a singleton class
        private static ILogger? _instance;
        private static readonly object padlock = new();
        private Logger() 
        {
            // Prevent creation through reflection
            if (_instance != null)
            {
                throw new InvalidOperationException("Singleton instance already exists.");
            }
        }
        public static ILogger GetInstance
        {
            get
            {
                // Prevents multiple threads from creating multiple instances
                lock (padlock)
                {
                    _instance ??= LoggerFactory.CreateLogger("console");
                    return _instance;
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
