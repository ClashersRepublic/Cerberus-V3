using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client.Unknown
{
    internal class Unknown_566 : Command
    {
        internal override int Type => 566;

        public Unknown_566(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }
    }
}
