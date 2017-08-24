namespace Magic.Royale.Network.Messages.Server.Battle
{
    internal class Battle_Failed : Message
    {
        public Battle_Failed(Device _Device) : base(_Device)
        {
            Identifier = 24103;
        }
    }
}
