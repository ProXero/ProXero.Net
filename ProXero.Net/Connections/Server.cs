using ProXero.Net.DataClasses;
using ProXero.Net.Interfaces;
using ProXero.Net.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProXero.Net
{
    public class Server<TMessage> : IServer<TMessage>
    {
        private readonly IConnectionBuilder listener;
        private readonly IFormatter formatter;
        private readonly Func<IConnection<TMessage>> connectionFactory;
        private readonly IHeartBeatFactory<TMessage> heartbeatFactory;
        private Dictionary<IStreamClient, IConnection<TMessage>> clients = new Dictionary<IStreamClient, IConnection<TMessage>>();
        private Subject<MessageInfo<TMessage>> inbox = new Subject<MessageInfo<TMessage>>();

        public Server(IConnectionBuilder listener, IFormatter formatter, Func<IConnection<TMessage>> connectionFactory, IHeartBeatFactory<TMessage> heartbeatFactory)
        {
            this.listener = listener;
            this.formatter = formatter;
            this.connectionFactory = connectionFactory;
            this.heartbeatFactory = heartbeatFactory;
        }

        public void Start(int port)
        {
            listener.Listen(port);

            var timer = Observable.Timer(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(5));
            timer.Subscribe(t => Broadcast(heartbeatFactory.CreateHeartBeat()));

            AcceptClientsAsync(listener);
        }

        private void AcceptClientsAsync(IConnectionBuilder listenerObject)
        {
            Task.Run(() =>
                {
                    while (true)
                    {
                        AcceptAsync(listenerObject.AcceptClient());
                    }
                });
        }

        private void AcceptAsync(IStreamClient client)
        {
            Task.Run(() =>
                {
                    var conn = connectionFactory();
                    clients.Add(client, conn);
                    Console.WriteLine("client connected {0}", client.ToString());

                    conn.Inbox.Subscribe(inbox.OnNext);

                    conn.Listen(client);
                });
        }

        public IObservable<MessageInfo<TMessage>> Messages
        {
            get { return inbox; }
        }

        public void Broadcast(TMessage message)
        {
            foreach (var client in clients.ToArray())
                SendMessage(client.Key, message);
        }

        protected bool SendMessage(IStreamClient client, TMessage message)
        {
            bool result = clients[client].SendMessage(message);
            if (!result)
        	{
                clients.Remove(client);
                Console.WriteLine("client disconnected {0}", client.ToString());
            }

            return result;
        }
    }
}
