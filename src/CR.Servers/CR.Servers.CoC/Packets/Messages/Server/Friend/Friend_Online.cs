using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.List;

namespace CR.Servers.CoC.Packets.Messages.Server.Friend
{
    internal class Friend_Online : Message
    {
        internal override short Type => 20206;

        public Friend_Online(Device Device) : base(Device)
        {
            
        }

        internal Logic.Friend Friend;

        internal override void Encode()
        {
            this.Data.AddLong(this.Friend.PlayerId);
            this.Data.AddVInt((int)this.Friend.GameState); 
            //1 = Player online
        }
    }
}
