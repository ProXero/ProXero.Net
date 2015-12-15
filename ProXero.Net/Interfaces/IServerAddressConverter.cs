﻿using ProXero.Net.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProXero.Net.Interfaces
{
    public interface IServerAddressConverter
    {
        IStreamClient Convert(ServerAddress address);
    }
}
