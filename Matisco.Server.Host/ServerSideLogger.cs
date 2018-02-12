using Matisco.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Matisco.Server.Host
{
    public class ServerSideLogger : ILogger
    {
        public void Error(string source, string message, object data = null)
        {
            log(source, "ERROR", message);
            if(data != null)
            {
                log(source, "ERROR", JsonConvert.SerializeObject(data));
            }
        }

        public void Info(string source, string message, object data = null)
        {
            log(source, "INFO", message);
            if (data != null)
            {
                log(source, "INFO", JsonConvert.SerializeObject(data));
            }
        }

        public void Verbose(string source, string message, object data = null)
        {
            log(source, "VERBOSE", message);
            if (data != null)
            {
                log(source, "VERBOSE", JsonConvert.SerializeObject(data));
            }
        }

        public void Warning(string source, string message, object data = null)
        {
            log(source, "WARN", message);
            if (data != null)
            {
                log(source, "WARN", JsonConvert.SerializeObject(data));
            }
        }

        private void log(string level, string source, string message)
        {
            Console.WriteLine(string.Format("{0} {1}: {2}: {3}", DateTime.Now, source, level, message));
        }
    }
}
