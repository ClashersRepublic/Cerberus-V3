namespace CR.Servers.CoC.Packets.Messages.Server.Battle
{
    using System.Collections.Generic;
    using CR.Servers.CoC.Logic;
    using CR.Servers.Extensions.List;

    internal class Live_Replay_Data : Message
    {
        internal List<Command> Commands;

        internal int EndSubTick;
        internal int Spectator;
        internal int Spectator1;

        public Live_Replay_Data(Device Device) : base(Device)
        {
        }

        internal override short Type => 24119;

        internal override void Encode()
        {
            this.Data.AddVInt(this.EndSubTick);
            this.Data.AddVInt(this.Spectator1); //Spectator count
            this.Data.AddVInt(this.Spectator); //Spectator on opposite ssite

            if (this.Commands != null)
            {
                this.Data.AddInt(this.Commands.Count);

                this.Commands.ForEach(Command =>
                {
                    this.Data.AddInt(Command.Type);
                    Command.Encode(this.Data);
                });
            }
            else
            {
                this.Data.AddInt(0);
            }
        }
    }
}