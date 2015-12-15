using ProXero.Net.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace ProXero.Net.Interfaces
{
    public interface IClient<TMessage>
    {
        Task ConnectAsync(ServerAddress serverAddress);

        IObservable<MessageInfo<TMessage>> Inbox { get; }

        BehaviorSubject<bool> IsConnected { get; }

        bool SendMessage(TMessage message);
    }
}
