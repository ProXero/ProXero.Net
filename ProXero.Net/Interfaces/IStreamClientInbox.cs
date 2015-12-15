using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace ProXero.Net.Interfaces
{
    public interface IStreamClientInbox<TMessage>
    {
        void Listen(IStreamClient client);

        IObservable<TMessage> Inbox { get; }
    }
}
