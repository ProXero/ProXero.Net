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
    public interface IConnection<TMessage>
    {
        void Listen(ServerAddress address);

        void Listen(IStreamClient client);

        bool SendMessage(TMessage message);

        IObservable<MessageInfo<TMessage>> Inbox { get; }

        BehaviorSubject<bool> IsConnected { get; }
    }
}
