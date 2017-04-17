using System;
using System.Collections.Generic;
using System.IO;
using Magic.Core;
using Magic.Core.Network;
using Magic.Files.Logic;
using Magic.Helpers;
using Magic.Logic;
using Magic.PacketProcessing.Messages.Server;

namespace Magic.PacketProcessing.Commands.Client
{
    internal class SearchOpponentCommand : Command
    {
        public SearchOpponentCommand(PacketReader br)
        {
            br.ReadInt32();
            br.ReadInt32();
            br.ReadInt32();
        }

        public override void Execute(Level level)
        {
            try
            {
                while (true)
                {
                    var defender = ObjectManager.GetRandomOnlinePlayer();
                    if (defender != null)
                    {
                        var allianceId = defender.GetPlayerAvatar().GetAllianceId();
                        if (allianceId > 0)
                        {
                            var defenderAlliance = ObjectManager.GetAlliance(allianceId);
                            if (defenderAlliance == null)
                                continue;
                        }

                        level.GetPlayerAvatar().State = ClientAvatar.UserState.Searching;

                        var trophyDiff = Math.Abs(level.GetPlayerAvatar().GetScore() - defender.GetPlayerAvatar().GetScore());
                        var reward = (int)Math.Round(Math.Pow((5 * trophyDiff), 0.25) + 5d);
                        var lost = (int)Math.Round(Math.Pow((2 * trophyDiff), 0.35) + 5d);

                        var info = new ClientAvatar.AttackInfo
                        {
                            Attacker = level,
                            Defender = defender,

                            Lost = lost,
                            Reward = reward,
                            UsedTroop = new List<DataSlot>()

                        };

                        // Just fucking clear it since its per user and a user can attack only once at a time.
                        level.GetPlayerAvatar().AttackingInfo.Clear();
                        level.GetPlayerAvatar().AttackingInfo.Add(level.GetPlayerAvatar().GetId(), info); //Use UserID For a while..Working on AttackID soon

                        defender.Tick();
                        new EnemyHomeDataMessage(level.GetClient(), defender, level).Send();
                    }
                    else
                    {
                        Logger.Error("Could not find opponent in memory, returning home.");
                        new OwnHomeDataMessage(level.GetClient(), level).Send();
                    }

                    break;
                }
            }
            catch (Exception ex)
            {
                // Ultimate fail safe in case unexpected shit happens.

                ExceptionLogger.Log(ex, "Could not find opponent in memory, returning home.");
                new OwnHomeDataMessage(level.GetClient(), level).Send();
            }
        }
    }
}
