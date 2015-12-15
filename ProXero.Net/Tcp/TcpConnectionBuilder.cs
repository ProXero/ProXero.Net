using ProXero.Net.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProXero.Net.Tcp
{
    class TcpConnectionBuilder : IConnectionBuilder
    {
        private TcpListener listener;

        public void Listen(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
        }

        public IStreamClient AcceptClient()
        {
            return new TcpStreamClient(listener.AcceptTcpClient());
        }
    }
}
