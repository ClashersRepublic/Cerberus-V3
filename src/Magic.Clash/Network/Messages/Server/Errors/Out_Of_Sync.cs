namespace Magic.ClashOfClans.Network.Messages.Server.Errors
{
    internal class Out_Of_Sync : Message
    {
        public Out_Of_Sync(Device device) : base(device)
        {
            Identifier = 24104;
        }
    }
}