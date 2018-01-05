namespace CR.Servers.CoC.Packets.Messages.Server.Friend
{
    using CR.Servers.CoC.Logic;

    internal class Friend_List_Entry : Message
    {
        internal Friend Friend;

        internal Friend_List_Entry(Device Device) : base(Device)
        {
        }

        internal override short Type => 20106;

        internal override void Encode()
        {
            this.Friend.Encode(this.Data);
        }
    }
}