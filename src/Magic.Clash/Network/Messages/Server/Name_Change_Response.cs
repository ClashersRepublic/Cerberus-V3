using Magic.ClashOfClans.Extensions.List;

namespace Magic.ClashOfClans.Network.Messages.Server
{
    internal class Name_Change_Response : Message
    {
        internal string Name;

        public Name_Change_Response(Device Device, string Name) : base(Device)
        {
            Identifier = 20300;
            this.Name = Name;
        }

        public override void Encode()
        {
            Data.AddInt(0);
            Data.AddInt(0);
            Data.AddString(Name);
        }
    }
}