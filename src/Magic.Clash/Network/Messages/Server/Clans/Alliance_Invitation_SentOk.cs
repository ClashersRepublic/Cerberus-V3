namespace Magic.ClashOfClans.Network.Messages.Server.Clans
{
    internal class Alliance_Invitation_SentOk : Message
    {
        public Alliance_Invitation_SentOk(Device device) : base(device)
        {
            Identifier = 24322;
        }
    }
}
