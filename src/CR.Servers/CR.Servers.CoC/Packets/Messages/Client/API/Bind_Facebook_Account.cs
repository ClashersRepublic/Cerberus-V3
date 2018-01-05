namespace CR.Servers.CoC.Packets.Messages.Client.API
{
    using CR.Servers.CoC.Core;
    using CR.Servers.CoC.Core.Network;
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Packets.Messages.Server.API;
    using CR.Servers.Extensions.Binary;

    internal class Bind_Facebook_Account : Message
    {
        internal string Identifier;
        internal string Token;

        internal bool Unknown;

        public Bind_Facebook_Account(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override short Type => 14201;

        internal override void Decode()
        {
            this.Unknown = this.Reader.ReadBoolean();
            this.Identifier = this.Reader.ReadString();
            this.Token = this.Reader.ReadString();
        }

        internal override void Process()
        {
            Level Level = this.Device.GameMode.Level;
            if (!string.IsNullOrEmpty(this.Identifier))
            {
                Player Player = Resources.Accounts.LoadAccountViaFacebook(this.Identifier)?.Player;

                if (Player != null)
                {
                    if (Player.UserId != Level.Player.UserId)
                    {
                        Player.Facebook.Identifier = string.Empty;
                        Player.Facebook.Token = string.Empty;
                    }
                }

                Level.Player.Facebook.Identifier = this.Identifier;
                Level.Player.Facebook.Token = this.Token;
                new Bind_Facebook_Account_Ok(this.Device).Send();
            }
        }
    }
}