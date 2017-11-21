using CR.Servers.CoC.Core;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Messages.Server.Avatar;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Messages.Client.Avatar
{
    internal class Ask_For_Avatar_Profile : Message
    {
        internal override short Type => 14325;

        public Ask_For_Avatar_Profile(Device Device, Reader Reader) : base(Device, Reader)
        {

        }

        internal int AvatarHighId;
        internal int AvatarLowId;

        internal override void Decode()
        {
            this.AvatarHighId = this.Reader.ReadInt32();
            this.AvatarLowId = this.Reader.ReadInt32();

            if (this.Reader.ReadBoolean())
            {
                this.Reader.ReadInt32(); //HomeHighId
                this.Reader.ReadInt32(); //HomeLowId
            }

            this.Reader.ReadBoolean();
        }

        internal override async void Process()
        {
            if (this.AvatarHighId >= 0 && this.AvatarLowId > 0)
            {
                var Player = await Resources.Accounts.LoadPlayerAsync(this.AvatarHighId, this.AvatarLowId);
                var Home = await Resources.Accounts.LoadHomeAsync(this.AvatarHighId, this.AvatarLowId);

                if (Player != null)
                {
                    new Avatar_Profile_Data(this.Device, Player, Home).Send();
                }
            }
        }
    }
}
