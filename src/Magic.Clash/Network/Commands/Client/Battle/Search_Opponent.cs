using System;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Network.Messages.Server.Battle;

namespace Magic.ClashOfClans.Network.Commands.Client.Battle
{
    internal class Search_Opponent : Command
    {
        public Search_Opponent(Reader _Reader, Device _Client, int _ID) : base(_Reader, _Client, _ID)
        {
        }

        public override void Decode()
        {
            Reader.ReadInt32();
            Reader.ReadInt32();
            Reader.ReadInt32();
        }

        public override void Process()
        {
            if (Device.State == State.LOGGED)
                Device.Player.Tick();

            try
            {
                if (!Device.Player.Avatar.Modes.IsAttackingOwnBase)
                {
                    Device.State = State.SEARCH_BATTLE;
                    var defender = ObjectManager.GetRandomOnlinePlayer();
                    if (defender != null)
                    {
                        var allianceId = defender.Avatar.ClanId;
                        if (allianceId > 0)
                        {
                            //var defenderAlliance = ObjectManager.GetAlliance(allianceId);
                            //if (defenderAlliance == null)
                            //continue;
                        }


                        defender.Tick();
                        new Pc_Battle_Data(Device) {BattleMode = Battle_Mode.PVP, Enemy = defender}.Send();
                    }
                    else
                    {
                        Logger.Error("Could not find opponent in memory, sending fail message.");
                        new Battle_Failed(Device).Send();
                    }
                }
                else
                {
                    new Pc_Battle_Data(Device) {BattleMode = Battle_Mode.NEXT_BUTTON_DISABLE, Enemy = Device.Player}
                        .Send();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex, "Could not find opponent in memory, sending fail message.");
                new Battle_Failed(Device).Send();
            }
        }
    }
}
