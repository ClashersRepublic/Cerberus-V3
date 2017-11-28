using System.Linq;
using CR.Servers.CoC.Logic;

namespace CR.Servers.CoC.Packets.Messages.Server.Avatar
{
    internal class Avatar_Stream : Message
    {
        internal override short Type => 24411;

        public Avatar_Stream(Device Device) : base (Device)
        {
        }


        internal override void Encode()
        {
            this.Device.GameMode.Level.Player.Inbox.Encode(this.Data);
        }

        internal override void Process()
        {
            var Streams = this.Device.GameMode.Level.Player.Inbox.Entries.Values.Where(T => T.New == 2).ToArray();

            foreach (var Stream in Streams)
            {
                Stream.New = 0;
            }
        }
    }
}
