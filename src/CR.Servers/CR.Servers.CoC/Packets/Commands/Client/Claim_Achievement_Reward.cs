using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Messages.Server.Home;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Claim_Achievement_Reward : Command
    {
        internal override int Type => 523;

        public Claim_Achievement_Reward(Device Device, Reader reader) : base(Device, reader)
        {
            
        }

        internal int AchievementId;

        internal override void Decode()
        {
            this.AchievementId = this.Reader.ReadInt32();
            base.Decode();
        }

        internal override void Execute()
        {
            var Level = this.Device.GameMode.Level;
            if (!Level.Player.AchievementProgress.Claim(this.AchievementId))
            {
                new Out_Of_Sync(this.Device).Send();
            }
        }
    }
}
