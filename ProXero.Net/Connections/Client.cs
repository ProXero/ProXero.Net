using ProXero.Net.DataClasses;
using ProXero.Net.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reactive.Subjects;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProXero.Net
{
    class Client<TMessage> : IClient<TMessage>
    {
        private IConnection<TMessage> connection;

        public Client(IConnection<TMessage> connection)
        {
            this.connection = connection;
        }

        public async Task ConnectAsync(ServerAddress serverAddress)
        {
            await Task.Run(() =>
                {
                    while (true)
                    {
                        try
                        {
                            connection.Listen(serverAddress);
                        }
                        catch (Exception)
                        {
                            Thread.Sleep(1000);
                        }
                    }
                });
        }

        public bool SendMessage(TMessage message)
        {
            return connection.SendMessage(message);
        }


        public IObservable<MessageInfo<TMessage>> Inbox
        {
            get { return connection.Inbox; }
        }


        public BehaviorSubject<bool> IsConnected
        {
            get { return connection.IsConnected; }
        }
    }
}
