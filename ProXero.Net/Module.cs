using ProXero.Net.Interfaces;
using ProXero.Net.Tcp;
using LightInject;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ProXero.Net
{
    public class Module : ICompositionRoot
    {

        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IFormatter, BinaryFormatter>();
            serviceRegistry.Register<ILogger>(sf => NLog.LogManager.GetCurrentClassLogger());
            
            serviceRegistry.Register<IConnectionBuilder, TcpConnectionBuilder>();
            serviceRegistry.Register<IStreamClient, TcpStreamClient>();
            serviceRegistry.Register<IServerAddressConverter, TcpAddressConverter>();
            serviceRegistry.Register<IHeartBeatFactory<IMessage>, HeartBeatFactory>();
            
            serviceRegistry.Register(typeof(IConnection<>), typeof(Connection<>));
            serviceRegistry.Register(typeof(IMessageSender<>), typeof(MessageSender<>));
            serviceRegistry.Register(typeof(IMessageReceiver<>), typeof(MessageReceiver<>));
            serviceRegistry.Register(typeof(IStreamClientInbox<>), typeof(StreamClientInbox<>));

            serviceRegistry.Register(typeof(IServer<>), typeof(Server<>));
            serviceRegistry.Register(typeof(IClient<>), typeof(Client<>));
        }
    }
}
