using ProXero.Net.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProXero.Net.Tcp
{
    public class TcpStreamClient : IStreamClient
    {
        private TcpClient tcpClient;

        public TcpStreamClient(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
        }

        public NetworkStream GetStream()
        {
            return tcpClient.GetStream();
        }

        public void Close()
        {
            tcpClient.Close();
        }

        public void Dispose()
        {
            tcpClient.Close();
        }

        public TcpClient Client
        {
            get { return tcpClient; }
        }

        public override string ToString()
        {
            return tcpClient.Client.RemoteEndPoint.ToString();
        }


        public bool IsConnected
        {
            get { return tcpClient.Connected; }
        }


        public void WaitUntilDataIsAvailable()
        {
            while (tcpClient.Connected && tcpClient.Available == 0)
            {
                Thread.Sleep(500);
            }
        }
    }
}
