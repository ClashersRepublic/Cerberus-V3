using System;
using System.Collections.Generic;
using Magic.Helpers;
using Magic.PacketProcessing;

namespace Magic.PacketProcessing.Commands.Server
{
    internal class ChangedNameCommand : Command
    {
        public string Name { get; set; }

        public long ID { get; set; }

        public ChangedNameCommand()
        {
        }

        public override byte[] Encode()
        {
            List<byte> data = new List<byte>();
            data.AddString(Name);
            data.AddInt64(ID);
            return data.ToArray();
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetId(long id)
        {
            ID = id;
        }
    }
}
