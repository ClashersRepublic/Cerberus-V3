using System.Collections.Generic;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Packets
{
    internal class Command
    {
        internal int ExecuteSubTick = -1;
        internal virtual int Type => 0;
        internal bool IsServerCommand => false;

        internal Reader Reader;
        internal Device Device;

        internal List<byte> Data;

        public Command(Device Device)
        {
            this.Device = Device;
            this.Data = new List<byte>();
        }

        public Command(Device Device, Reader Reader)
        {
            this.Device = Device;
            this.Reader = Reader;
        }

        internal virtual void Decode()
        {
        }

        internal virtual void Encode()
        {
        }

        internal virtual void Execute()
        {
        }
        
        internal virtual void Load(JToken Token)
        {
        }

        internal virtual JObject Save()
        {
            return null;
        }
        
        internal JObject SaveBase()
        {
            return new JObject
            {
                {
                    "t", this.ExecuteSubTick
                }
            };
        }
    }
}