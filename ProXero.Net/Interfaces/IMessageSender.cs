using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProXero.Net.Interfaces
{
    public interface IMessageSender<TMessage>
    {
        bool SendMessage(Stream stream, TMessage message);
    }
}
