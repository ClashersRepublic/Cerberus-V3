using CR.Servers.Extensions.Binary;
using CR.Servers.CoC.Logic;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Toggle_Clan_Filter : Command
    {
        internal override int Type => 571;

        public Toggle_Clan_Filter(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal byte Unknown;

        internal override void Decode()
        {
            this.Unknown = this.Reader.ReadByte();
            base.Decode();
        }
    }
}
