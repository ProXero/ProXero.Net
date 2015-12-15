using ProXero.Net.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ProXero.Net.Tcp
{
    class TcpAddressConverter : IServerAddressConverter
    {
        public IStreamClient Convert(DataClasses.ServerAddress address)
        {
            var client = new TcpClient(address.Host, address.Port);
            return new TcpStreamClient(client);
        }
    }
}
