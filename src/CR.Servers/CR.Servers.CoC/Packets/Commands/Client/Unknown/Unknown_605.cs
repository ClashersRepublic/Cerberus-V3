using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client.Unknown
{
    internal class Unknown_605 : Command
    {
        internal override int Type => 605;

        public Unknown_605(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }
    }
}
