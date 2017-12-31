using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client.Unknown
{
    internal class Unknown_534 : Command
    {
        internal override int Type => 534;

        public Unknown_534(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }
    }
}
