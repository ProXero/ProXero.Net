using ProXero.Net.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProXero.Net
{
    class MessageSender<TMessage> : IMessageSender<TMessage>
    {
        private IFormatter formatter;
        private ILogger logger;

        public MessageSender(IFormatter formatter, ILogger logger)
        {
            this.formatter = formatter;
            this.logger = logger;
        }

        public bool SendMessage(Stream stream, TMessage message)
        {
            try
            {
                formatter.Serialize(stream, message);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Couldn't send message");
                return false;
            }
        }
    }
}
