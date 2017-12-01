using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Messages.Server.Battle;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client.Battle
{
    internal class Search_Opponent_V2 : Command
    {
        internal override int Type => 601;

        public Search_Opponent_V2(Device device, Reader reader) : base(device, reader)
        {
        }

        internal int UnknownInt;
        internal int UnknownByte;

        internal override void Decode()
        {
            this.UnknownInt = this.Reader.ReadInt32();
            this.UnknownByte = this.Reader.ReadByte();
            base.Decode();
        }

        internal override void Execute()
        {
            new Pc_Battle_Data_V2(this.Device) {Enemy = this.Device.GameMode.Level}.Send();
            //new V2_Battle_Info(this.Device, this.Device.GameMode.Level).Send();
        }
    }
}
