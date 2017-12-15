using CR.Servers.CoC.Core;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Extensions.Game;
using CR.Servers.CoC.Extensions.Helper;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Clan;
using CR.Servers.CoC.Logic.Clan.Items;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.CoC.Packets.Commands.Server;
using CR.Servers.CoC.Packets.Enums;
using CR.Servers.CoC.Packets.Messages.Server.Alliances;
using CR.Servers.Extensions.Binary;
using CR.Servers.Logic.Enums;

namespace CR.Servers.CoC.Packets.Messages.Client.Alliances
{
    internal class Create_Alliance : Message
    {
        internal override short Type => 14301;

        public Create_Alliance(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal string Name;
        internal string Description;

        internal int AllianceBadge;
        internal int RequiredScore;
        internal int RequiredDuelScore;
        internal int WarFrequency;

        internal Hiring AllianceType;
        internal int Origin;

        internal bool PublicWarLog;
        internal bool AmicalWar;

        internal override void Decode()
        {
            this.Name = this.Reader.ReadString();
            this.Description = this.Reader.ReadString();

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
            var Level = this.Device.GameMode.Level;
            if (Level.Player.Resources.GetCountByData(Globals.AllianceCreateResourceData) >= Globals.AllianceCreateCost)
            {
                if (!this.Device.GameMode.Level.Player.InAlliance)
                {
                    if (!string.IsNullOrWhiteSpace(this.Name))
                    {
                        if (Resources.Name.IsMatch(this.Name))
                        {
                            if (this.Name.Length <= 16)
                            {
                                this.Name = Resources.Regex.Replace(this.Name, " ");

                                if (this.Name.StartsWith(" "))
                                {
                                    this.Name = this.Name.Remove(0, 1);
                                }

                                if (this.Name.Length >= 2)
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
                                        new Alliance_Create_Fail(this.Device)
                                        {
                                            Error = AllianceErrorReason.InvalidDescription
                                        }.Send();
                                        return;
                                    }

                                    //var Background =  (AllianceBadgeLayerData) CSV.Tables.Get(Gamefile.AllianceBadgeLayer).GetDataWithInstanceID(this.AllianceBadge % 0x100);
                                    //var Middle =  (AllianceBadgeLayerData) CSV.Tables.Get(Gamefile.AllianceBadgeLayer).GetDataWithInstanceID(this.AllianceBadge % 0x1000000 / 0x100);
                                    //var Foreground = (AllianceBadgeLayerData) CSV.Tables.Get(Gamefile.AllianceBadgeLayer).GetDataWithInstanceID(this.AllianceBadge / 0x1000000);

                                    //if (Background != null)
                                    {
                                        //if (Middle != null)
                                        {
                                            //Z`if (Foreground != null)
                                            {
                                                var Alliance = new Alliance
                                                {
                                                    Header =
                                                    {
                                                        Name = this.Name,
                                                        Locale = this.Device.Info.Locale,
                                                        Badge = this.AllianceBadge,
                                                        Type = this.AllianceType,
                                                        WarFrequency = this.WarFrequency,
                                                        PublicWarLog = this.PublicWarLog,
                                                        RequiredScore = this.RequiredScore,
                                                        RequiredDuelScore = this.RequiredDuelScore,
                                                        AmicalWar = this.AmicalWar,
                                                        Origin = this.Origin,
                                                    },
                                                    Description = this.Description
                                                };

                                                if (Alliance.Members.Join(Level.Player, out Member Member))
                                                {
                                                    Member.Role = Role.Leader;
                                                    Resources.Clans.New(Alliance);

                                                    Level.Player.SetAlliance(Alliance, Member);
                                                    Level.Player.AllianceHighId = Alliance.HighId;
                                                    Level.Player.AllianceLowId = Alliance.LowId;

                                                    Level.Player.Resources.Remove(Globals.AllianceCreateResourceData,
                                                        Globals.AllianceCreateCost);

                                                    new Alliance_Full_Entry(this.Device) {Alliance = Alliance}.Send();
                                                    this.Device.GameMode.CommandManager.AddCommand(
                                                        new Joined_Alliance(this.Device)
                                                        {
                                                            AllianceId = Alliance.AllianceId,
                                                            AllianceName = Alliance.Header.Name,
                                                            AllianceBadge = Alliance.Header.Badge,
                                                            AllianceLevel = Alliance.Header.ExpLevel,
                                                            CreateAlliance = true
                                                        }
                                                    );
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                    new Alliance_Create_Fail(this.Device) {Error = AllianceErrorReason.NameTooShort}
                                        .Send();
                            }
                            else
                                new Alliance_Create_Fail(this.Device) { Error = AllianceErrorReason.InvalidName }.Send();
                        }
                        else
                            new Alliance_Create_Fail(this.Device) { Error = AllianceErrorReason.InvalidName}.Send();
                    }
                }
            }
        }
    }
}

