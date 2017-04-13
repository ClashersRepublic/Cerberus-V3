using System;
using System.Collections.Generic;
using System.IO;
using UCS.Core;
using UCS.Core.Network;
using UCS.Files.Logic;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Commands.Client
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
            SearchOpponentCommand searchOpponentCommand = this;
            try
            {
                var l = ObjectManager.GetRandomOnlinePlayer();
                if (l != null)
                {
                    //l.Tick();
                    level.GetPlayerAvatar().State = ClientAvatar.UserState.Searching;

                    var trophyDiff = Math.Abs(level.GetPlayerAvatar().GetScore() - l.GetPlayerAvatar().GetScore());
                    var reward = (int)Math.Round(Math.Pow((5 * trophyDiff), 0.25) + 5d);
                    var lost = (int)Math.Round(Math.Pow((2 * trophyDiff), 0.35) + 5d);

                    var info = new ClientAvatar.AttackInfo
                    {
                        Attacker = level,
                        Defender = l,

                        Lost = lost,
                        Reward = reward,
                        UsedTroop = new List<DataSlot>()

                    };

                    // Just fucking clear it since its per user and a user can attack only once at a time.
                    level.GetPlayerAvatar().AttackingInfo.Clear();

                    level.GetPlayerAvatar().AttackingInfo.Add(l.GetPlayerAvatar().GetId(), info); //Use UserID For a while..Working on AttackID soon

                    l.Tick();
                    new EnemyHomeDataMessage(level.GetClient(), l, level).Send();
                }
                else
                {
                    //new EnemyHomeDataMessage(level.GetClient(), l, level).Send();
                    Logger.Error("Could not find opponent in memory, returning home.");
                    new OwnHomeDataMessage(level.GetClient(), level);
                }
            }
            catch (Exception ex)
            {
                // Ultimate fail safe incase unexpected shit happens.

                Logger.Error("Could not find opponent in memory, returning home. " + ex);
                new OwnHomeDataMessage(level.GetClient(), level);
            }
        }
    }
}
