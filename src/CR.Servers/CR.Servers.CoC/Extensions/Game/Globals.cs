﻿namespace CR.Servers.CoC.Extensions.Game
{
    using CR.Servers.CoC.Files;
    using CR.Servers.CoC.Files.CSV_Logic.Logic;
    using CR.Servers.CoC.Logic.Enums;

    internal class Globals
    {
        internal static bool CollectAllResourcesAtOnce;

        internal static int StartingGold;
        internal static int StartingElixir;
        internal static int StartingGold2;
        internal static int StartingElixir2;
        internal static int StartingDiamonds;
        internal static int AllianceCreateCost;

        internal static int SpeedUpDiamondCost1Min;
        internal static int SpeedUpDiamondCost1Hour;
        internal static int SpeedUpDiamondCost24Hours;
        internal static int SpeedUpDiamondCost1Week;
        internal static int Village2SpeedUpDiamondCost1Min;
        internal static int Village2SpeedUpDiamondCost1Hour;
        internal static int Village2SpeedUpDiamondCost24Hours;
        internal static int Village2SpeedUpDiamondCost1Week;

        internal static int ResourceDiamondCost100;
        internal static int ResourceDiamondCost1000;
        internal static int ResourceDiamondCost10000;
        internal static int ResourceDiamondCost100000;
        internal static int ResourceDiamondCost1000000;
        internal static int ResourceDiamondCost10000000;
        internal static int DarkElixirDiamondCost1;
        internal static int DarkElixirDiamondCost10;
        internal static int DarkElixirDiamondCost100;
        internal static int DarkElixirDiamondCost1000;
        internal static int DarkElixirDiamondCost10000;
        internal static int DarkElixirDiamondCost100000;
        internal static int Village2ResourceDiamondCost100;
        internal static int Village2ResourceDiamondCost1000;
        internal static int Village2ResourceDiamondCost10000;
        internal static int Village2ResourceDiamondCost100000;
        internal static int Village2ResourceDiamondCost1000000;
        internal static int Village2ResourceDiamondCost10000000;

        internal static int TroopTrainingSpeedUpTutorialCost;

        internal static int HeroHealthSpeedUpCostMultiplier;
        internal static int SpellSpeedUpCostMultiplier;
        internal static int TroopRequestSpeedUpCostMultiplier;
        internal static int TrainCancelMultiplier;
        internal static int BuildCancelMultiplier;
        internal static int SpellCancelMultiplier;
        internal static int HeroUpgradeCancelMultiplier;

        internal static int WorkerCost2Nd;
        internal static int WorkerCost3Rd;
        internal static int WorkerCost4Th;
        internal static int WorkerCost5Th;

        internal static int AllianceTroopRequestCooldown;
        internal static int ClanMailCooldown;
        internal static int ReplayShareCooldown;
        internal static int ElderKickCooldown;
        internal static int ChallengeCooldown;
        internal static int ArrangeWarCooldown;

        internal static int ObstacleCountMax;
        internal static int ObstacleRespawnSeconds;

        internal static ResourceData AllianceCreateResourceData;

        internal static int[] TroopHousingV2Cost;
        internal static int[] TroopHousingV2BuildTimeSeconds;


        internal static int Village2DoNotAllowClearObstacleTh;
        internal static int TallGrassRespawnSeconds;

        internal static int AttackLength;
        internal static int AttackPrepartionLength;
        internal static int AttackPrepartionLength2;

        internal static void Initialize()
        {
            Globals.AllianceCreateResourceData = (ResourceData) CSV.Tables.Get(Gamefile.Resources).GetData(((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("ALLIANCE_CREATE_RESOURCE")).TextValue);
            Globals.AllianceCreateCost = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("ALLIANCE_CREATE_COST")).NumberValue;
            Globals.StartingDiamonds = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("STARTING_DIAMONDS")).NumberValue;
            Globals.StartingGold = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("STARTING_GOLD")).NumberValue;
            Globals.StartingElixir = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("STARTING_ELIXIR")).NumberValue;
            Globals.StartingGold2 = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("STARTING_GOLD2")).NumberValue;
            Globals.StartingElixir2 = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("STARTING_ELIXIR2")).NumberValue;

            Globals.SpeedUpDiamondCost1Min = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("SPEED_UP_DIAMOND_COST_1_MIN")).NumberValue;
            Globals.SpeedUpDiamondCost1Hour = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("SPEED_UP_DIAMOND_COST_1_HOUR")).NumberValue;
            Globals.SpeedUpDiamondCost24Hours = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("SPEED_UP_DIAMOND_COST_24_HOURS")).NumberValue;
            Globals.SpeedUpDiamondCost1Week = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("SPEED_UP_DIAMOND_COST_1_WEEK")).NumberValue;

            Globals.Village2SpeedUpDiamondCost1Min = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("VILLAGE2_SPEED_UP_DIAMOND_COST_1_MIN")).NumberValue;
            Globals.Village2SpeedUpDiamondCost1Hour = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("VILLAGE2_SPEED_UP_DIAMOND_COST_1_HOUR")).NumberValue;
            Globals.Village2SpeedUpDiamondCost24Hours = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("VILLAGE2_SPEED_UP_DIAMOND_COST_24_HOURS")).NumberValue;
            Globals.Village2SpeedUpDiamondCost1Week = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("VILLAGE2_SPEED_UP_DIAMOND_COST_1_WEEK")).NumberValue;

            Globals.ResourceDiamondCost100 = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("RESOURCE_DIAMOND_COST_100")).NumberValue;
            Globals.ResourceDiamondCost1000 = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("RESOURCE_DIAMOND_COST_1000")).NumberValue;
            Globals.ResourceDiamondCost10000 = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("RESOURCE_DIAMOND_COST_10000")).NumberValue;
            Globals.ResourceDiamondCost100000 = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("RESOURCE_DIAMOND_COST_100000")).NumberValue;
            Globals.ResourceDiamondCost1000000 = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("RESOURCE_DIAMOND_COST_1000000")).NumberValue;
            Globals.ResourceDiamondCost10000000 = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("RESOURCE_DIAMOND_COST_10000000")).NumberValue;

            Globals.DarkElixirDiamondCost1 = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("DARK_ELIXIR_DIAMOND_COST_1")).NumberValue;
            Globals.DarkElixirDiamondCost10 = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("DARK_ELIXIR_DIAMOND_COST_10")).NumberValue;
            Globals.DarkElixirDiamondCost100 = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("DARK_ELIXIR_DIAMOND_COST_100")).NumberValue;
            Globals.DarkElixirDiamondCost1000 = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("DARK_ELIXIR_DIAMOND_COST_1000")).NumberValue;
            Globals.DarkElixirDiamondCost10000 = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("DARK_ELIXIR_DIAMOND_COST_10000")).NumberValue;
            Globals.DarkElixirDiamondCost100000 = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("DARK_ELIXIR_DIAMOND_COST_100000")).NumberValue;

            Globals.Village2ResourceDiamondCost100 = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("VILLAGE2_RESOURCE_DIAMOND_COST_100")).NumberValue;
            Globals.Village2ResourceDiamondCost1000 = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("VILLAGE2_RESOURCE_DIAMOND_COST_1000")).NumberValue;
            Globals.Village2ResourceDiamondCost10000 = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("VILLAGE2_RESOURCE_DIAMOND_COST_10000")).NumberValue;
            Globals.Village2ResourceDiamondCost100000 = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("VILLAGE2_RESOURCE_DIAMOND_COST_100000")).NumberValue;
            Globals.Village2ResourceDiamondCost1000000 = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("VILLAGE2_RESOURCE_DIAMOND_COST_1000000")).NumberValue;
            Globals.Village2ResourceDiamondCost10000000 = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("VILLAGE2_RESOURCE_DIAMOND_COST_10000000")).NumberValue;

            Globals.WorkerCost2Nd = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("WORKER_COST_2ND")).NumberValue;
            Globals.WorkerCost3Rd = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("WORKER_COST_3RD")).NumberValue;
            Globals.WorkerCost4Th = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("WORKER_COST_4TH")).NumberValue;
            Globals.WorkerCost5Th = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("WORKER_COST_5TH")).NumberValue;

            Globals.TroopTrainingSpeedUpTutorialCost = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("TROOP_TRAINING_SPEED_UP_COST_TUTORIAL")).NumberValue;
            Globals.HeroHealthSpeedUpCostMultiplier = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("HERO_HEALTH_SPEED_UP_COST_MULTIPLIER")).NumberValue;
            Globals.SpellSpeedUpCostMultiplier = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("SPELL_SPEED_UP_COST_MULTIPLIER")).NumberValue;
            Globals.TroopRequestSpeedUpCostMultiplier = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("TROOP_REQUEST_SPEED_UP_COST_MULTIPLIER")).NumberValue;
            Globals.TrainCancelMultiplier = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("TRAIN_CANCEL_MULTIPLIER")).NumberValue;
            Globals.BuildCancelMultiplier = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("BUILD_CANCEL_MULTIPLIER")).NumberValue;
            Globals.SpellCancelMultiplier = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("SPELL_CANCEL_MULTIPLIER")).NumberValue;
            Globals.HeroUpgradeCancelMultiplier = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("HERO_UPGRADE_CANCEL_MULTIPLIER")).NumberValue;

            Globals.AllianceTroopRequestCooldown = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("ALLIANCE_TROOP_REQUEST_COOLDOWN")).NumberValue;
            Globals.ClanMailCooldown = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("CLAN_MAIL_COOLDOWN")).NumberValue;
            Globals.ReplayShareCooldown = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("REPLAY_SHARE_COOLDOWN")).NumberValue;
            Globals.ElderKickCooldown = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("ELDER_KICK_COOLDOWN")).NumberValue;
            Globals.ChallengeCooldown = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("CHALLENGE_COOLDOWN")).NumberValue;
            Globals.ArrangeWarCooldown = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("ARRANGE_WAR_COOLDOWN")).NumberValue;

            Globals.ObstacleCountMax = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("OBSTACLE_COUNT_MAX")).NumberValue;
            Globals.ObstacleRespawnSeconds = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("OBSTACLE_RESPAWN_SECONDS")).NumberValue;

            Globals.CollectAllResourcesAtOnce = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("COLLECT_ALL_RESOURCES_AT_ONCE")).BooleanValue;

            Globals.TroopHousingV2Cost = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("TROOP_HOUSING_V2_COST")).NumberArray;
            Globals.TroopHousingV2BuildTimeSeconds = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("TROOP_HOUSING_V2_BUILD_TIME_SECONDS")).NumberArray;


            Globals.Village2DoNotAllowClearObstacleTh = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("VILLAGE2_DO_NOT_ALLOW_CLEAR_OBSTACLE_TH")).NumberValue;
            Globals.TallGrassRespawnSeconds = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("TALLGRASS_RESPAWN_SECONDS")).NumberValue;

            Globals.AttackLength = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("ATTACK_LENGTH_SEC")).NumberValue;
            Globals.AttackPrepartionLength = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("ATTACK_PREPARATION_LENGTH_SEC")).NumberValue;
            Globals.AttackPrepartionLength2 = ((GlobalData) CSV.Tables.Get(Gamefile.Globals).GetData("ATTACK_PREPARATION_LENGTH_VILLAGE2_SEC")).NumberValue;
        }
    }
}