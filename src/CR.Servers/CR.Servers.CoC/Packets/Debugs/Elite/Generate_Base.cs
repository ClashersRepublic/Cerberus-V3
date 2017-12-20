using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Files;
using CR.Servers.CoC.Files.CSV_Helpers;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.CoC.Logic.Map;
using CR.Servers.CoC.Packets.Messages.Server.Home;
using CR.Servers.Logic.Enums;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Packets.Debugs.Elite
{
    class Generate_Base : Debug
    {
        internal override Rank RequiredRank => Rank.Elite;

        public Generate_Base(Device Device, params string[] Parameters) : base(Device, Parameters)
        {

        }

        internal StringBuilder Help;

        internal override void Process()
        {
            if (Parameters.Length >= 1)
            {
                //Proof of concept only
                var A = new GameObjectManager(this.Device.GameMode.Level);
                var B = new TileMap(50, 50);
                var C = CSV.Tables.GetWithGlobalId(1000013) as BuildingData;
                C.VillageType = 0;


                //NPC 40
                for (int X = 49; X != 0;)
                {
                    for (int Y = 49; Y != 0;)
                    {
                        if (this.Device.GameMode.Level.IsValidPlaceForBuilding(C, X - 1, Y - 1, C.Width, C.Height, B))
                        {
                            Building Building = (Building)GameObjectFactory.CreateGameObject(C, this.Device.GameMode.Level);
                            Building.SetPositionXY(X - 1, Y - 1);
                            Building.Id = GlobalId.Create(500 + Building.Type, A.GameObjects[Building.Type][0].Count);
                            Building.SetUpgradeLevel(C.MaxLevel);
                            if (Building.CombatComponent != null)
                            {
                                Building.CombatComponent.GearUp = 1;
                                Building.CombatComponent.AttackMode = true;
                                Building.CombatComponent.AttackModeDraft = true;
                            }

                            A.GameObjects[Building.Type][0].Add(Building);

                            B.AddGameObject(Building);

                            Logging.Info(this.GetType(), "Village:" + C.VillageType + "   X:" + X + "   Y:" + Y);

                            break;
                        }

                        if (Y - C.Width >= C.Width)
                            Y -= C.Width;
                        else
                        {
                            Y = 0;
                            if (X - C.Height >= C.Height)
                                X -= C.Height;
                            else
                            {
                                X = 0;
                                break;
                            }
                        }
                        JObject Json = new JObject { { "exp_ver", 1 } };
                        A.Save(Json);
                        this.Device.GameMode.Level.GameObjectManager.GameObjects[0][0].Clear();
                        this.Device.GameMode.Level.Load(Json);
                    }
                }

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
