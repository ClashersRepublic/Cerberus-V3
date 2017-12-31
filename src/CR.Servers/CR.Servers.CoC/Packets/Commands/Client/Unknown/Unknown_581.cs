using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client.Unknown
{
    internal class Unknown_581 : Command
    {
        internal override int Type => 581;

        public Unknown_581(Device Device, Reader Reader) : base(Device, Reader)
        {
            //Seems to be war related
        }

        internal int Count;

        internal override void Decode()
        {
            this.Count = this.Reader.ReadInt32();

            for (int i = 0; i < this.Count; i++)
            {
                this.Reader.ReadInt64();
            }
            this.Reader.ReadInt64();
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
            base.Decode();
        }
    }
}
