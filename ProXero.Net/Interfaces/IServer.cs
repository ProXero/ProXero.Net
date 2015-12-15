using ProXero.Net.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace ProXero.Net.Interfaces
{
    public interface IServer<TMessage>
    {
        void Start(int port);

        IObservable<MessageInfo<TMessage>> Messages { get; }

        void Broadcast(TMessage message);
    }
}
