using Magic.ClashOfClans.Extensions.List;

namespace Magic.ClashOfClans.Network.Commands.Server
{
    internal class Name_Change_Callback : Command
    {
        public string Name = string.Empty;
        public string Previous = string.Empty;

        public Name_Change_Callback(Device _Client) : base(_Client)
        {
            Identifier = 3;
        }

        public override void Decode()
        {
            Previous = Name;
            Name = Reader.ReadString();
        }

        public override void Encode()
        {
            Data.AddString(Device.Player.Avatar.Name);
            Data.AddInt(Device.Player.Avatar.NameState);
            Data.AddInt(4);
            Data.AddInt(-1);
        }
    }
}
