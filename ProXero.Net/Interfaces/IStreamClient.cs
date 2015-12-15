using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ProXero.Net.Interfaces
{
    public interface IStreamClient : IDisposable
    {
        NetworkStream GetStream();

        void Close();

        //TcpClient Client { get; }

        bool IsConnected { get; }

        void WaitUntilDataIsAvailable();
    }
}
