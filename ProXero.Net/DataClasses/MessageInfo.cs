using ProXero.Net.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ProXero.Net.DataClasses
{
    public class MessageInfo<T>
    {
        private Connection<T> sender;

        public MessageInfo(Connection<T> sender, T message)
        {
            Message = message;
            this.sender = sender;
        }

        public T Message { get; private set; }

        public bool Reply(T message)
        {
            return sender.SendMessage(message);
        }
    }
}
