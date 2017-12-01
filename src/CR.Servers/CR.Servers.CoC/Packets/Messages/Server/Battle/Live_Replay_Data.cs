using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.Battle
{
    internal class Live_Replay_Data : Message
    {
        internal override short Type => 24119;

        public Live_Replay_Data(Device Device) : base(Device)
        {
            
        }

        internal int EndSubTick;
        internal List<Command> Commands;

        internal override void Encode()
        {
            this.Data.AddInt(this.EndSubTick);
            this.Data.AddInt(this.Commands.Count);

            this.Commands.ForEach(Command =>
            {
                this.Data.AddInt(Command.Type);
                Command.Encode(this.Data);
            });
        }
    }
}
