using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBApi.Serialization;

namespace IBApi.Messages.Client
{
    [IbSerializable(71)]
    class StartApiMessage : IClientMessage
    {
        public int Version;
        public int ClientId;
    }
}
