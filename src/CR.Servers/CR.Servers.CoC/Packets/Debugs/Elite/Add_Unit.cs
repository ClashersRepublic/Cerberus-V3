namespace CR.Servers.CoC.Packets.Debugs.Elite
{
    using System.Text;
    using CR.Servers.CoC.Core.Network;
    using CR.Servers.CoC.Files;
    using CR.Servers.CoC.Files.CSV_Logic.Logic;
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Logic.Enums;
    using CR.Servers.CoC.Packets.Messages.Server.Home;
    using CR.Servers.Logic.Enums;

    internal class Add_Unit : Debug
    {
        internal StringBuilder Help;

        public Add_Unit(Device Device, params string[] Parameters) : base(Device, Parameters)
        {
        }

        internal override Rank RequiredRank
        {
            get
            {
                return Rank.Elite;
            }
        }

        internal override void Process()
        {
            foreach (CharacterData Data in CSV.Tables.Get(Gamefile.Characters).Datas)
            {
                if (!Data.DisableProduction)
                {
                    if (Data.VillageType == 0)
                    {
                        this.Device.GameMode.Level.Player.Units.Add(Data, 500);
                    }
                }
            }

            if (this.Device.Connected)
            {
                new OwnHomeDataMessage(this.Device).Send();
            }
        }
    }
}