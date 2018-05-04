using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Files;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.CoC.Packets.Commands.Server;
using CR.Servers.CoC.Packets.Messages.Server.Home;
using CR.Servers.Logic.Enums;
using NLog.Targets;

namespace CR.Servers.CoC.Packets.Debugs
{
    internal class Add_Castle_Unit : Debug
    {
        public Add_Castle_Unit(Device Device, params string[] Parameters) : base(Device, Parameters)
        {
            // Add_Spells
        }

        internal override Rank RequiredRank
        {
            get
            {
                return Rank.Player;
            }
        }

        internal override void Process()
        {
            Level level = this.Device.GameMode.Level;
            level.Player.AllianceUnits.Clear();
            foreach (CharacterData Data in CSV.Tables.Get(Gamefile.Characters).Datas)
            {
                if (!Data.DisableProduction)
                {
                    if (Data.VillageType == 0)
                    {
                        if (!Data.IsSecondaryTroop)
                        {
                            if (!Data.EnabledByCalendar)
                            {
                                int UnitLevel = level.Player.GetUnitUpgradeLevel(Data);
                                this.Device.GameMode.CommandManager.AddCommand(new Alliance_Unit_Received(this.Device)
                                {
                                    Donator = "❤DebugCommand❤",
                                    UnitType = 0,
                                    UnitId = Data.GlobalId,
                                    Level = UnitLevel
                                });

                                level.Player.AllianceUnits.Add(Data.GlobalId, 999, UnitLevel);
                            }
                        }
                    }
                }
            }
        }
    }
}