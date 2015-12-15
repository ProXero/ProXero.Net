using ProXero.Net.DataClasses;
using ProXero.Net.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Text;

namespace ProXero.Net
{
    public class Connection<TMessage> : IConnection<TMessage>
    {
        private IMessageSender<TMessage> messageSender;
        private IStreamClientInbox<TMessage> streamInbox;
        private IServerAddressConverter addressConverter;

        private readonly BehaviorSubject<bool> isConnected = new BehaviorSubject<bool>(false);

        private IStreamClient client;

        public Connection(IMessageSender<TMessage> messageSender, IStreamClientInbox<TMessage> streamInbox, IServerAddressConverter addressConverter)
        {
            this.messageSender = messageSender;
            this.streamInbox = streamInbox;
            this.addressConverter = addressConverter;
        }

        public void Listen(IStreamClient client)
        {
            this.client = client;

            isConnected.OnNext(true);
            streamInbox.Listen(client);
            isConnected.OnNext(false);
        }

        public void Listen(ServerAddress address)
        {
            Listen(addressConverter.Convert(address));
        }

        public bool SendMessage(TMessage message)
        {
            bool result = false;
            if (IsConnected.Value)
            {
                result = messageSender.SendMessage(client.GetStream(), message);
                isConnected.OnNext(result);
            }

            return result;
        }

        public IObservable<MessageInfo<TMessage>> Inbox
        {
            get { return streamInbox.Inbox.Select(m => new MessageInfo<TMessage>(this, m)); }
        }

        public BehaviorSubject<bool> IsConnected
        {
            get { return isConnected; }
        }
    }
}
