using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Files;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.CoC.Packets.Messages.Server.Home;
using CR.Servers.Logic.Enums;

namespace CR.Servers.CoC.Packets.Debugs.Elite
{
    internal class Add_Unit : Debug
    {
        internal override Rank RequiredRank => Rank.Elite;

        public Add_Unit(Device Device, params string[] Parameters) : base(Device, Parameters)
        {

        }

        internal StringBuilder Help;

        internal override void Process()
        {
            if (Parameters.Length >= 1)
            {

                foreach (CharacterData Data in CSV.Tables.Get(Gamefile.Characters).Datas)
                {
                    if (!Data.DisableProduction)
                    {
                        if (Data.UnitOfType == 1)
                        {
                            this.Device.GameMode.Level.Player.Units.Add(Data, 500);
                        }
                    }
                }

                if (this.Device.Connected)
                    new Own_Home_Data(this.Device).Send();
            }
            else
            {
                this.Help = new StringBuilder();
                this.Help.AppendLine("Time exceed the limit!");
                this.SendChatMessage(Help.ToString());
            }
            //Time bust be at least 1
        }
    }
}
