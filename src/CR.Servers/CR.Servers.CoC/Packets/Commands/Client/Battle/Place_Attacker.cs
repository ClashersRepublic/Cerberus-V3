using System.Collections.Generic;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Extensions.Helper;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Battle.Slots;
using CR.Servers.Extensions.Binary;
using CR.Servers.Extensions.List;
using CR.Servers.Logic.Enums;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Packets.Commands.Client.Battle
{
    internal class Place_Attacker : Command
    {
        internal override int Type => 700;

        public Place_Attacker(Device Device, Reader Reader) : base(Device, Reader)
        {

        }

        internal int X;
        internal int Y;
        internal CharacterData Character;

        internal override void Decode()
        {
            this.X = this.Reader.ReadInt32();
            this.Y = this.Reader.ReadInt32();
            this.Character = this.Reader.ReadData<CharacterData>();
            base.Decode();
        }

        internal override void Encode(List<byte> Data)
        {
            Data.AddInt(this.X);
            Data.AddInt(this.Y);
            Data.AddInt(this.Character.GlobalId);
            base.Encode(Data);
        }

        internal override void Execute()
        {
            if (this.Character != null)
            {
                var Level = this.Device.GameMode.Level;
                if (Level.GameObjectManager.Map == 0)
                {
                    Item Unit = Level.Player.Units.GetByData(this.Character);

                    if (Unit != null)
                    {
                        if (Unit.Count > 0)
                        {
                            //Do some logging shit for replay and etc

                            Unit.Count--;
                        }
                    }
                }
                else
                {
                    if (this.Device.State == State.IN_1VS1_BATTLE)
                    {
                        var Battle = Resources.BattlesV2.GetPlayer(Level.Player.BattleIdV2,  Level.Player.UserId);

                        int Index = Battle.ReplayInfo.Units.FindIndex(T => T[0] == this.Character.GlobalId);
                        if (Index > -1)
                            Battle.ReplayInfo.Units[Index][1]++;
                        else
                            Battle.ReplayInfo.Units.Add(new[] { this.Character.GlobalId, 1 });

                        Battle.Add(this);
                        Level.BattleManager.BattleCommandManager.StoreCommands(this);
                    }
                }
            }
        }

        internal override JObject Save()
        {
            JObject Json = new JObject
            {
                {"base", this.SaveBase()},
                {"x", this.X},
                {"y", this.Y},
                {"d", this.Character.GlobalId}
            };

            return Json;
        }
    }
}