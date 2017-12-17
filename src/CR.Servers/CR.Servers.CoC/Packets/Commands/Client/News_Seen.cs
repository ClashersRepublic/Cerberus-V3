using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class News_Seen : Command
    {
        internal override int Type => 539;

        public News_Seen(Device device, Reader reader) : base(device, reader)
        {
        }

        internal int NewsId;

        internal override void Decode()
        {
            this.NewsId = this.Reader.ReadInt32();
            base.Decode();
        }

        internal override void Execute()
        {
        }
    }
}
