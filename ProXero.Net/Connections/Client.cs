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
    public class Client<TMessage> : Connection<TMessage>, IClient<TMessage>
    {
        public Client(IMessageSender<TMessage> messageSender, IStreamClientInbox<TMessage> streamInbox, IServerAddressConverter addressConverter) 
            : base(messageSender, streamInbox, addressConverter)
        {
        }

        public async Task ConnectAsync(ServerAddress serverAddress)
        {
            await Task.Run(() =>
                {
                    while (true)
                    {
                        try
                        {
                            Listen(serverAddress);
                        }
                        catch (Exception)
                        {
                            Thread.Sleep(1000);
                        }
                    }
                });
        }
    }
}
