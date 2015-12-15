using ProXero.Net.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProXero.Net.Messages
{
    [Serializable]
    public class HeartBeat : IMessage
    {
        public int ID { get; set; }

        public byte[] data { get; set; }
    }
}
