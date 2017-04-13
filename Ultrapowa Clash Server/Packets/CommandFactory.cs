using System;
using System.Collections.Generic;
using System.IO;
using UCS.Core;
using UCS.Helpers;
using UCS.PacketProcessing.Commands.Client;
using UCS.PacketProcessing.Commands;
using UCS.Packets.Commands.Client;

namespace UCS.PacketProcessing
{
    internal static class CommandFactory
    {
        static readonly Dictionary<uint, Type> m_vCommands;

        static CommandFactory()
        {
            m_vCommands = new Dictionary<uint, Type>();
            //m_vCommands.Add(14, typeof(PlayerWarStatusCommand));
            m_vCommands.Add(20, typeof(RemainingBuildingsLayoutCommand));
            m_vCommands.Add(500, typeof(BuyBuildingCommand));
            m_vCommands.Add(501, typeof(MoveBuildingCommand));
            m_vCommands.Add(502, typeof(UpgradeBuildingCommand));
            m_vCommands.Add(503, typeof(SellBuildingCommand));
            m_vCommands.Add(504, typeof(SpeedUpConstructionCommand));
            m_vCommands.Add(505, typeof(CancelConstructionCommand));
            m_vCommands.Add(506, typeof(CollectResourcesCommand));
            m_vCommands.Add(507, typeof(ClearObstacleCommand));
            m_vCommands.Add(508, typeof(TrainUnitCommand));
            m_vCommands.Add(509, typeof(CancelUnitProductionCommand));
            m_vCommands.Add(510, typeof(BuyTrapCommand));
            m_vCommands.Add(511, typeof(RequestAllianceUnitsCommand));
            m_vCommands.Add(512, typeof(BuyDecoCommand));
            m_vCommands.Add(513, typeof(SpeedUpTrainingCommand));
            m_vCommands.Add(514, typeof(SpeedUpClearingCommand));
            m_vCommands.Add(515, typeof(CancelUpgradeUnitCommand));
            m_vCommands.Add(516, typeof(UpgradeUnitCommand));
            m_vCommands.Add(517, typeof(SpeedUpUpgradeUnitCommand));
            m_vCommands.Add(518, typeof(BuyResourcesCommand));
            m_vCommands.Add(519, typeof(MissionProgressCommand));
            m_vCommands.Add(520, typeof(UnlockBuildingCommand));
            m_vCommands.Add(521, typeof(FreeWorkerCommand));
            m_vCommands.Add(522, typeof(BuyShieldCommand));
            m_vCommands.Add(523, typeof(ClaimAchievementRewardCommand));
            m_vCommands.Add(524, typeof(ToggleAttackModeCommand));
            m_vCommands.Add(525, typeof(LoadTurretCommand));
            m_vCommands.Add(526, typeof(BoostBuildingCommand));
            m_vCommands.Add(527, typeof(UpgradeHeroCommand));
            m_vCommands.Add(528, typeof(SpeedUpHeroUpgradeCommand));
            m_vCommands.Add(529, typeof(ToggleHeroSleepCommand));
            m_vCommands.Add(530, typeof(SpeedUpHeroHealthCommand));
            m_vCommands.Add(531, typeof(CancelHeroUpgradeCommand));
            m_vCommands.Add(532, typeof(NewShopItemsSeenCommand));
            m_vCommands.Add(533, typeof(MoveMultipleBuildingsCommand));
            m_vCommands.Add(534, typeof(CancelShieldCommand));
            m_vCommands.Add(537, typeof(SendAllianceMailCommand));
            m_vCommands.Add(538, typeof(MyLeagueCommand));
            m_vCommands.Add(539, typeof(NewsSeenCommand));
            m_vCommands.Add(540, typeof(RequestAllianceUnitsCommand));
            //m_vCommands.Add(541, typeof(SpeedUpRequestUnitsCommand));
            m_vCommands.Add(543, typeof(KickAllianceMemberCommand));
            m_vCommands.Add(544, typeof(GetVillageLayoutsCommand));
            m_vCommands.Add(546, typeof(EditVillageLayoutCommand));
            m_vCommands.Add(549, typeof(UpgradeMultipleBuildingsCommand));
            m_vCommands.Add(552, typeof(SaveVillageLayoutCommand));
            m_vCommands.Add(553, typeof(ClientServerTickCommand));
            m_vCommands.Add(554, typeof(RotateDefenseCommand));
            m_vCommands.Add(558, typeof(AddQuicKTrainingTroopCommand));
	        m_vCommands.Add(559, typeof(TrainQuickUnitsCommand));
            m_vCommands.Add(560, typeof(StartClanWarCommand));
            m_vCommands.Add(567, typeof(SetActiveVillageLayoutCommand));
            m_vCommands.Add(568, typeof(CopyVillageLayoutCommand));
            m_vCommands.Add(570, typeof(TogglePlayerWarStateCommand));
            m_vCommands.Add(571, typeof(FilterChatCommand));
            m_vCommands.Add(572, typeof(ToggleHeroAttackModeCommand));
            m_vCommands.Add(574, typeof(ChallangeCommand));
            //m_vCommands.Add(577, typeof(MoveBuildingsCommand));
            m_vCommands.Add(584, typeof(BoostBarracksCommand));
            //m_vCommands.Add(586, typeof(RenameQuickTrainCommand));
            //m_vCommands.Add(590, typeof(EventsSeenCommand));
            m_vCommands.Add(600, typeof(PlaceAttackerCommand));
            m_vCommands.Add(603, typeof(EndOfBattleCommand));
            m_vCommands.Add(604, typeof(CastSpellCommand));
            m_vCommands.Add(605, typeof(PlaceHeroCommand));
            m_vCommands.Add(700, typeof(SearchOpponentCommand));
        }

        public static object Read(PacketReader br, int depth)
        {
            var cm = br.ReadUInt32WithEndian();
            if (m_vCommands.ContainsKey(cm))
            {
                Logger.Write("Command '" + cm + "' is handled");
                var instance = (Command)Activator.CreateInstance(m_vCommands[cm], br);
                instance.Depth = depth;
                return instance;
            }
            else
            {
                Logger.Write("Command '" + cm + "' is unhandled");
                return null;
            } 
        }
    }
}
