using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Edit_Mode_Shown : Command
    {
        internal override int Type => 544;

        public Edit_Mode_Shown(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal override void Execute()
        {
            this.Device.GameMode.Level.EditModeShown = true;
        }
    }
}
