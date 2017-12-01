using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Speed_Up_All_Training_V2 : Command
    {
        internal override int Type => 593;

        public Speed_Up_All_Training_V2(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }
    }
}
