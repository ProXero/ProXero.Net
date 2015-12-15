using ProXero.Net.Interfaces;
using ProXero.Net.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProXero.Net
{
    public class HeartBeatFactory : IHeartBeatFactory<IMessage>
    {
        public IMessage CreateHeartBeat()
        {
            return new HeartBeat();
        }
    }
}
