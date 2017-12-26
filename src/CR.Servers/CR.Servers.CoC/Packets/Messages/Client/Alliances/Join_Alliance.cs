using System;
using System.Linq;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Clan;
using CR.Servers.CoC.Logic.Clan.Items;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.CoC.Packets.Commands.Server;
using CR.Servers.CoC.Packets.Messages.Server.Alliances;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Messages.Client.Alliances
{
    internal class Join_Alliance : Message
    {
        internal override short Type => 14305;

        public Join_Alliance(Device device, Reader reader) : base(device, reader)
        {
        }

        internal int ClanHighId;
        internal int ClanLowId;

        internal override void Decode()
        {
            this.ClanHighId = this.Reader.ReadInt32();
            this.ClanLowId = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            var Alliance = Resources.Clans.Get(this.ClanHighId, this.ClanLowId);

            if (Alliance != null)
            {
                var Avatar = this.Device.GameMode.Level.Player;
                
                foreach (var entry in Avatar.Inbox.Entries.Values.Where(T => T.Type == AvatarStream.Invitation).ToArray())
                {
                    Avatar.Inbox.Remove(entry);
                }

                if (Alliance.Members.Join(Avatar, out Member Member))
                {
                    Avatar.SetAlliance(Alliance, Member);
                    Avatar.AllianceHighId = Alliance.HighId;
                    Avatar.AllianceLowId = Alliance.LowId;

                    new Alliance_Full_Entry(this.Device) {Alliance = Alliance}.Send();

                    try
                    {
                        new Alliance_Stream(this.Device) { Alliance = Alliance }.Send();
                    }
                    catch (Exception Exception)
                    {
                        Logging.Error(Exception.GetType(), $"Exception happend when trying to send Alliance Stream. {Exception.Message} : [{(this.Device.GameMode?.Level?.Player != null ? this.Device.GameMode.Level.Player.HighID + ":" + this.Device.GameMode.Level.Player.LowID : "-:-")}]" + Environment.NewLine + Exception.StackTrace);
                    }

                    Alliance.IncrementTotalConnected();
                    Alliance.Streams.AddEntry(new EventStreamEntry(Member, Member, AllianceEvent.Joined));

                    this.Device.GameMode.CommandManager.AddCommand(
                        new Joined_Alliance(this.Device)
                        {
                            AllianceId = Alliance.AllianceId,
                            AllianceName = Alliance.Header.Name,
                            AllianceBadge = Alliance.Header.Badge,
                            AllianceLevel = Alliance.Header.ExpLevel,
                            CreateAlliance = false
                        }
                    );
                }
                else
                    Logging.Error(this.GetType(), "Unable to join the clan, Join() function returned false");
            }
            else
                Logging.Error(this.GetType(), "Unable to join the clan. Get() returned a null clan");
        }
    }
}
