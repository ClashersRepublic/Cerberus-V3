namespace CR.Servers.CoC.Packets.Messages.Server.Home
{
    using CR.Servers.CoC.Logic;
    using CR.Servers.Extensions.List;
    using CR.Servers.Logic.Enums;

    internal class WaitingToGoHomeMessage : Message
    {
        internal int EstimatedTime;

        internal WaitingToGoHomeMessage(Device device, int estimatedTime) : base(device)
        {
            this.EstimatedTime = estimatedTime;
        }

        internal override short Type => 24112;

        internal override void Encode()
        {
            this.Data.AddInt(this.EstimatedTime);
        }
    }
}