using CR.Servers.CoC.Core;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Clan;
using CR.Servers.CoC.Packets.Commands.Server;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Messages.Client.Alliances
{

    internal class Claim_Alliance_Gift : Message
    {
        internal override short Type => 14334;

        public Claim_Alliance_Gift(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal long StreamId;

        internal override void Decode()
        {
            this.StreamId = this.Reader.ReadInt64();
        }

        internal override void Process()
        {
            var level = this.Device.GameMode.Level;

            if (level.Player.InAlliance)
            {
                var stream = level.Player.Alliance.Streams.Get(this.StreamId);
                if (stream != null)
                {
                    if (stream is GiftStreamEntry StreamEntry)
                    {
                        if (!StreamEntry.Claimers.Contains(level.Player.UserId))
                        {
                            StreamEntry.Claimers.Add(level.Player.UserId);
                            --StreamEntry.ClaimLeft;

                            level.Player.Alliance.Streams.Update(stream);

                            this.Device.GameMode.CommandManager.AddCommand(
                                new Diamonds_Added(this.Device)
                                {
                                    Count = StreamEntry.DiamondCount,
                                    AlliangeGift = true
                                });
                        }
                        else
                            Logging.Error(this.GetType(), "Unable to claim the alliance gift. The player already claimed the gift");
                    }
                    else
                        Logging.Error(this.GetType(), "Unable to claim the alliance gift. The stream is not a giftstream.");
                }
                else
                    Logging.Error(this.GetType(), "Unable to claim the alliance gift. The stream is null.");
            }
            else
                Logging.Error(this.GetType(), "Unable to claim the alliance gift. The player is not in an alliance.");
        }
    }
}
