using System;
using Magic.ClashOfClans.Extensions.List;
using Magic.ClashOfClans.Logic;
using Magic.ClashOfClans.Logic.Enums;

namespace Magic.ClashOfClans.Network.Messages.Server.Battle
{
    internal class Pc_Battle_Data : Message
    {
        internal Level Enemy = null;
        internal Battle_Mode BattleMode = Battle_Mode.PVP;

        public Pc_Battle_Data(Device device) : base(device)
        {
            Identifier = 24107;
        }

        public override void Encode()
        {
            Enemy.Tick();
            var Home = new Objects(Enemy, Enemy.Json);
            {
                Data.AddInt((int) (Home.Timestamp - DateTime.UtcNow).TotalSeconds);
                Data.AddInt(-1);

                Data.AddRange(Home.ToBytes);
                Data.AddRange(Enemy.Avatar.ToBytes);
                Data.AddRange(Device.Player.Avatar.ToBytes);

                Data.AddInt((int) BattleMode);
                Data.AddInt(0);
                Data.Add(0);
            }
        }

        public override void Process()
        {
            if (BattleMode == Battle_Mode.PVP)
            {
                Device.Player.Avatar.Last_Attack_Enemy_ID.Add((int) Enemy.Avatar.UserId);

                if (Device.State == State.SEARCH_BATTLE)
                    if (Device.Player.Avatar.Last_Attack_Enemy_ID.Count > 20)
                        Device.Player.Avatar.Last_Attack_Enemy_ID.RemoveAt(0);

                Device.State = State.IN_PC_BATTLE;

                /*if (this.Device.Player.Avatar.Battle_ID == 0)
                {
                    Core.Resources.Battles.New(this.Device.Player, Core.Players.Get(this.Device.Player.Avatar.Last_Attack_Enemy_ID[this.Device.Player.Avatar.Last_Attack_Enemy_ID.Count - 1]));
                }
                else
                {
                    Core.Resources.Battles.Save(Core.Resources.Battles.Get(this.Device.Player.Avatar.Battle_ID));
                    Core.Resources.Battles.TryRemove(this.Device.Player.Avatar.Battle_ID);

                    Core.Resources.Battles.New(this.Device.Player, Core.Players.Get(this.Device.Player.Avatar.Last_Attack_Enemy_ID[this.Device.Player.Avatar.Last_Attack_Enemy_ID.Count - 1]));
                }*/
            }
            else if (BattleMode == Battle_Mode.AMICAL)
            {
                Device.State = State.IN_AMICAL_BATTLE;
            }
            else if (BattleMode == Battle_Mode.NEXT_BUTTON_DISABLE)
            {
                Device.State = State.IN_PC_BATTLE;
            }
        }
    }
}
