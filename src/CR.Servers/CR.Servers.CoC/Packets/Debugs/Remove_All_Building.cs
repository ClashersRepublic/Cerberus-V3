namespace CR.Servers.CoC.Packets.Debugs
{
    using System;
    using CR.Servers.CoC.Core;
    using CR.Servers.CoC.Core.Network;
    using CR.Servers.CoC.Files;
    using CR.Servers.CoC.Files.CSV_Helpers;
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Logic.Enums;
    using CR.Servers.CoC.Packets.Messages.Server.Home;
    using CR.Servers.Logic.Enums;

    internal class Remove_All_Building : Debug
    {
        public Remove_All_Building(Device Device, params string[] Parameters) : base(Device, Parameters)
        {
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
            Player Player = this.Device.GameMode.Level.Player;

            GameObjectManager GameObjectManager = this.Device.GameMode.Level.GameObjectManager;

            try
            {
                GameObjectManager.GameObjects[0][0].Clear();
                GameObjectManager.GameObjects[3][0].Clear();
                GameObjectManager.GameObjects[4][0].Clear();
                GameObjectManager.GameObjects[6][0].Clear();

                GameObjectManager.ObstaclesIndex[0] = 0;
                GameObjectManager.DecoIndex[0] = 0;

                Player.TownHallLevel = 0;
                Player.CastleLevel = -1;
                Player.CastleTotalCapacity = 0;
                Player.CastleTotalSpellCapacity = 0;
                Player.CastleUsedCapacity = 0;
                Player.CastleUsedSpellCapacity = 0;

                if (Player.AllianceUnits != null)
                {
                    Player.AllianceUnits.Clear();
                }

                if (Player.AllianceSpells != null)
                {
                    Player.AllianceSpells.Clear();
                }

                Data TownHall = CSV.Tables.Get(Gamefile.Buildings).GetDataWithID(1000001);
                Data Castle = CSV.Tables.Get(Gamefile.Buildings).GetDataWithID(1000014);
                Data Builder = CSV.Tables.Get(Gamefile.Buildings).GetDataWithID(1000015);

                GameObjectManager.AddGameObject(new Building(TownHall, this.Device.GameMode.Level)
                {
                    Position =
                    {
                        X = 24 << 9,
                        Y = 23 << 9
                    }
                });

                GameObjectManager.AddGameObject(new Building(Castle, this.Device.GameMode.Level)
                {
                    Position =
                    {
                        X = 28 << 9,
                        Y = 35 << 9
                    },
                    Locked = true
                });

                GameObjectManager.AddGameObject(new Building(Builder, this.Device.GameMode.Level)
                {
                    Position =
                    {
                        X = 21 << 9,
                        Y = 23 << 9
                    }
                });

                this.SendChatMessage("Successfully flatten your main village, Enjoy!");
                new OwnHomeDataMessage(this.Device).Send();
            }
            catch (Exception Exception)
            {
                Logging.Error(Exception.GetType(), "Unable to flatten the village for " + this.Device.GameMode.Level.Player.UserId + Environment.NewLine + Exception.StackTrace);
                this.SendChatMessage($"Failed to flatten the village, Error code {Exception.GetType()}");
            }
        }
    }
}