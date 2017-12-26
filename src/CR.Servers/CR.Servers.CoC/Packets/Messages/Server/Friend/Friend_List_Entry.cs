using CR.Servers.CoC.Logic;

namespace CR.Servers.CoC.Packets.Messages.Server.Friend
{
    internal class Friend_List_Entry : Message
    {
        internal override short Type => 20106;

        internal Friend_List_Entry(Device Device) : base(Device)
        {
            
        }

        internal Logic.Friend Friend;

        internal override void Encode()
        {
            this.Friend.Encode(this.Data);
        }
    }
}
