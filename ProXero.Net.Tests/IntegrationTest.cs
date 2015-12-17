using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LightInject;
using NLog;
using NUnit.Framework;
using ProXero.Net.DataClasses;
using ProXero.Net.Interfaces;
using ProXero.Net.LightInject;
using ProXero.Net.Messages;

namespace ProXero.Net.Tests
{
    [TestFixture]
    class IntegrationTest
    {
        private static readonly ServiceContainer container = new ServiceContainer();
        static AutoResetEvent e = new AutoResetEvent(false);

        [Test]
        public void Connect()
        {
            container.Register<ILogger>(sf => NLog.LogManager.GetCurrentClassLogger());
            container.RegisterFrom<Module>();

            var server = container.GetInstance<IServer<IMessage>>();

            server.Messages.Subscribe(receive);

            server.Start(8090);
            Console.WriteLine("Server is running on port 8090");

            var client = container.GetInstance<IClient<IMessage>>();

            client.Inbox.Subscribe(clientReceive);

            client.ConnectAsync(new ServerAddress("localhost", 8090));

            Assert.That(e.WaitOne(TimeSpan.FromSeconds(10)), Is.True);
        }

        private void clientReceive(MessageInfo<IMessage> obj)
        {
            var message = obj.Message as HeartBeat;
            Console.WriteLine("server heartbeat received, ID: {0}, data length: {1}", message.ID, message.data == null ? 0 : message.data.Length);
            obj.Reply(new HeartBeat() { ID = 10 });
        }

        private static void receive(MessageInfo<IMessage> obj)
        {
            var message = obj.Message as HeartBeat;
            Console.WriteLine("client heartbeat received, ID: {0}, data length: {1}", message.ID, message.data == null ? 0 : message.data.Length);

            obj.Reply(new HeartBeat() { ID = 100 });

            e.Set();
        }
    }
}
