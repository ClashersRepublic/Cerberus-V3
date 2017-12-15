using System.Collections.Generic;
using CR.Servers.CoC.Logic;
using CR.Servers.Core.Consoles.Colorful;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.Battle
{
    internal class Live_Replay_Header : Message
    {
        internal override short Type => 24118;

        public Live_Replay_Header(Device Device) : base(Device)
        {
            this.Version = 9;
        }

        internal string ReplayHeaderJson;

        internal int InitialStreamEndSubTick;
        internal List<Command> InitialCommands;

        internal override void Encode()
        {
            Console.WriteLine(this.ReplayHeaderJson);

            this.Data.AddString(null);
            this.Data.AddCompressed(this.ReplayHeaderJson, false);
            this.Data.AddInt(this.InitialStreamEndSubTick);
            this.Data.AddInt(this.InitialCommands.Count);

            this.InitialCommands.ForEach(Command =>
            {
                this.Data.AddInt(Command.Type);
                Command.Encode(this.Data);
            });

            this.Data.AddInt(this.InitialCommands.Count);
        }
    }
}
