using ProXero.Net.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProXero.Net
{
    class MessageReceiver<TMessage> : IMessageReceiver<TMessage>
    {
        private IFormatter formatter;

        public MessageReceiver(IFormatter formatter)
        {
            this.formatter = formatter;       
        }

        public TMessage GetMessage(System.Net.Sockets.NetworkStream stream)
        {
            return (TMessage)formatter.Deserialize(stream);
        }
    }
}
