using System.Collections.Generic;
using CR.Servers.CoC.Logic;
using CR.Servers.Core.Consoles.Colorful;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.Battle
{
    internal class Live_Replay_Data : Message
    {
        internal override short Type => 24119;

        public Live_Replay_Data(Device Device) : base(Device)
        {
            this.Version = 9;
        }

        internal int EndSubTick;
        internal int Spectator;
        internal int Spectator1;
        internal List<Command> Commands;

        internal override void Encode()
        {
            this.Data.AddVInt(this.EndSubTick);
            this.Data.AddVInt(this.Spectator1);//Spectator count
            this.Data.AddVInt(this.Spectator); //Spectator on opposite ssite

            this.Data.AddInt(513);

            this.Commands.ForEach(Command =>
            {
                this.Data.AddInt(Command.Type);
                Command.Encode(this.Data);
            });
        }
    }
}
