using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ProXero.Net.Interfaces
{
    interface IConnectionBuilder
    {
        IStreamClient AcceptClient();

        void Listen(int port);
    }
}
