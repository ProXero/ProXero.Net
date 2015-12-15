using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProXero.Net.DataClasses
{
    public class ServerAddress
    {
        public readonly int Port;

        public readonly string Host;

        public ServerAddress(string host, int port)
        {
            Host = host;
            Port = port;
        }
    }
}
