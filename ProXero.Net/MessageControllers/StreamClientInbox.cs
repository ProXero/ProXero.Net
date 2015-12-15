using ProXero.Net.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace ProXero.Net
{
    class StreamClientInbox<TMessage> : IStreamClientInbox<TMessage>
    {
        private IMessageReceiver<TMessage> messageReceiver;

        private readonly Subject<TMessage> inbox = new Subject<TMessage>();

        public StreamClientInbox(IMessageReceiver<TMessage> messageReceiver)
        {
            this.messageReceiver = messageReceiver;
        }

        public void Listen(IStreamClient client)
        {
            var stream = client.GetStream();
            while (client.IsConnected)
            {
                client.WaitUntilDataIsAvailable();

                var message = messageReceiver.GetMessage(stream);
                inbox.OnNext(message);
            }
        }

        public IObservable<TMessage> Inbox
        {
            get { return inbox; }
        }
    }
}
