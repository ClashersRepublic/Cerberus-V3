using System.Linq;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Clan;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.CoC.Packets.Commands.Server;
using CR.Servers.CoC.Packets.Messages.Server.Alliances;
using CR.Servers.Extensions.Binary;
using CR.Servers.Logic.Enums;

namespace CR.Servers.CoC.Packets.Messages.Client.Alliances
{
    internal class Change_Alliance_Settings : Message
    {
        internal override short Type => 14316;

        public Change_Alliance_Settings(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal string Description;

        internal int AllianceBadge;
        internal int RequiredScore;
        internal int RequiredDuelScore;
        internal int WarFrequency;

        internal bool PublicWarLog;
        internal bool AmicalWar;

        internal Hiring AllianceType;
        internal int Origin;

        internal override void Decode()
        {
            this.Description = this.Reader.ReadString();
            this.Reader.ReadString();

            this.AllianceBadge = this.Reader.ReadInt32();
            this.AllianceType = (Hiring) this.Reader.ReadInt32();
            this.RequiredScore = this.Reader.ReadInt32();
            this.RequiredDuelScore = this.Reader.ReadInt32();
            this.WarFrequency = this.Reader.ReadInt32();

            this.Origin = this.Reader.ReadInt32();

            this.PublicWarLog = this.Reader.ReadBooleanV2();
            this.AmicalWar = this.Reader.ReadBooleanV2();
        }

        internal override void Process()
        {
            if (this.Device.GameMode.Level.Player.InAlliance)
            {
                if (this.Description != null)
                {
                    this.Description = Resources.Regex.Replace(this.Description, " ");

                    if (this.Description.StartsWith(" "))
                    {
                        this.Description = this.Description.Remove(0, 1);
                    }

                    if (this.Description.Length > 128)
                    {
                        this.Description = this.Description.Substring(0, 128);
                    }
                }
                else
                {
                    new Change_Alliance_Settings_Fail(this.Device).Send();
                    return;
                }

                //AllianceBadgeLayerData Background = (AllianceBadgeLayerData)CSV.Tables.Get(Gamefile.AllianceBadgeLayer).GetDataWithInstanceID(this.AllianceBadge % 0x100);
                //AllianceBadgeLayerData Middle = (AllianceBadgeLayerData)CSV.Tables.Get(Gamefile.AllianceBadgeLayer).GetDataWithInstanceID(this.AllianceBadge % 0x1000000 / 0x100);
                //AllianceBadgeLayerData Foreground = (AllianceBadgeLayerData)CSV.Tables.Get(Gamefile.AllianceBadgeLayer).GetDataWithInstanceID(this.AllianceBadge / 0x1000000);

                //if (Background != null)
                {
                    //if (Middle != null)
                    {
                        //if (Foreground != null)
                        {
                            var Level = this.Device.GameMode.Level;
                            var Alliance = Resources.Clans.Get(Level.Player.AllianceHighId, Level.Player.AllianceLowId);
                            Alliance.Description = this.Description;
                            Alliance.Header.Badge = this.AllianceBadge;
                            Alliance.Header.Type = this.AllianceType;
                            Alliance.Header.WarFrequency = this.WarFrequency;
                            Alliance.Header.Origin = this.Origin;
                            Alliance.Header.PublicWarLog = this.PublicWarLog;
                            Alliance.Header.RequiredScore = this.RequiredScore;
                            Alliance.Header.RequiredDuelScore = this.RequiredDuelScore;
                            Alliance.Header.AmicalWar = this.AmicalWar;

                            Alliance.Streams.AddEntry(new EventStreamEntry(Level.Player.AllianceMember,
                                Level.Player.AllianceMember, AllianceEvent.ChangedSettings));

                            foreach (var Player in Alliance.Members.Connected.Values.ToArray())
                            {
                                if (Player.Connected)
                                {
                                    Player.Level.GameMode.CommandManager.AddCommand(
                                        new Changed_Alliance_Settings(Player.Level.GameMode.Device)
                                        {
                                            AllianceId = Alliance.AllianceId,
                                            AllianceBadge = Alliance.Header.Badge
                                        });
                                }
                                else
                                    Alliance.Members.Connected.TryRemove(Player.UserId, out _);
                            }
                        }
                    }
                }
            }
        }
    }
}
