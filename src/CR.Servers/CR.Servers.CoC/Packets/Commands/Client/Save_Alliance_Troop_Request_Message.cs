using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Save_Alliance_Troop_Request_Message : Command
    {
        internal override int Type => 540;

        public Save_Alliance_Troop_Request_Message(Device Device, Reader Reader) : base(Device, Reader)
        {

        }

        internal string Message;

        internal override void Decode()
        {
            this.Message = this.Reader.ReadString();
            base.Decode();
        }

        internal override void Execute()
        {
            this.Device.GameMode.Level.TroopRequestMessage = this.Message;
        }
    }
}