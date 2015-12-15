using ProXero.Net.Interfaces;
using LightInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reactive.Linq;
using ProXero.Net.Messages;
using ProXero.Net.DataClasses;
using NLog;

namespace ServerConsole
{
    class Program
    {
        private static readonly ServiceContainer container = new ServiceContainer();
        private static IServer<IMessage> server;

        static AutoResetEvent e = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            server = container.GetInstance<IServer<IMessage>>();

            server.Messages.Subscribe(receive);

            server.Start(8090);
            Console.WriteLine("Server is running on port 8090");

            e.WaitOne();
        }

        private static void receive(MessageInfo<IMessage> obj)
        {
            var message = obj.Message as HeartBeat;
            Console.WriteLine("heartbeat received, ID: {0}, data length: {1}", message.ID, message.data == null ? 0 : message.data.Length);

            obj.Reply(new HeartBeat() { ID = 100 });

            //server.Broadcast(new HeartBeat() { ID = 100 });
            //e.Set();
        }
    }
}
