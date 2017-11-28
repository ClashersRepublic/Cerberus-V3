using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Unknown_544 : Command
    {
        internal override int Type => 544;

        public Unknown_544(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }
    }
}
