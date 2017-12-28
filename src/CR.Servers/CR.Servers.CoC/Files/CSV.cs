using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Files.CSV_Reader;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.Files.CSV_Reader;

namespace CR.Servers.CoC.Files
{
    internal static class CSV
    {
        internal static readonly Dictionary<int, string> Gamefiles = new Dictionary<int, string>();

        internal static Gamefiles Tables;

        internal static void Initialize()
        {

            Tables = new Gamefiles();

            Gamefiles.Add((int)Gamefile.Buildings, @"Gamefiles/logic/buildings.csv");
            Gamefiles.Add((int)Gamefile.Locales, @"Gamefiles/logic/locales.csv");
            Gamefiles.Add((int)Gamefile.Resources, @"Gamefiles/logic/resources.csv");
            Gamefiles.Add((int)Gamefile.Characters, @"Gamefiles/logic/characters.csv");
            Gamefiles.Add((int)Gamefile.Building_Classes, @"Gamefiles/logic/building_classes.csv");
            Gamefiles.Add((int)Gamefile.Obstacles, @"Gamefiles/logic/obstacles.csv");
            Gamefiles.Add((int)Gamefile.Traps, @"Gamefiles/logic/traps.csv");
            Gamefiles.Add((int)Gamefile.Globals, @"Gamefiles/logic/globals.csv");
            Gamefiles.Add((int)Gamefile.Experience_Levels, @"Gamefiles/logic/experience_levels.csv");
            Gamefiles.Add((int)Gamefile.Townhall_Levels, @"Gamefiles/logic/townhall_levels.csv");
            Gamefiles.Add((int)Gamefile.Npcs, @"Gamefiles/logic/npcs.csv");
            Gamefiles.Add((int)Gamefile.Decos, @"Gamefiles/logic/decos.csv");
            Gamefiles.Add((int)Gamefile.Shields, @"Gamefiles/logic/shields.csv");
            Gamefiles.Add((int)Gamefile.Missions, @"Gamefiles/logic/missions.csv");
            Gamefiles.Add((int)Gamefile.Achievements, @"Gamefiles/logic/achievements.csv");
            Gamefiles.Add((int)Gamefile.Spells, @"Gamefiles/logic/spells.csv");
            Gamefiles.Add((int)Gamefile.Heroes, @"Gamefiles/logic/heroes.csv");
            Gamefiles.Add((int)Gamefile.Variables, @"Gamefiles/logic/variables.csv");
            Gamefiles.Add((int)Gamefile.Leagues, @"Gamefiles/logic/leagues.csv");
            Gamefiles.Add((int)Gamefile.Regions, @"Gamefiles/logic/regions.csv");
            Gamefiles.Add((int)Gamefile.AllianceBadgeLayer, @"Gamefiles/logic/alliance_badge_layers.csv");
            Gamefiles.Add((int)Gamefile.Village_Objects, @"Gamefiles/logic/village_objects.csv");

            foreach (var File in Gamefiles)
            {
                if (new FileInfo(File.Value).Exists)
                {
                    Tables.Initialize(new Table(File.Value), File.Key);
                }
                else
                {
                    throw new FileNotFoundException($"{File.Value} does not exist!");
                }
            }

            foreach (var table in Tables.DataTables.Values)
            {
                foreach (var data in table.Datas)
                {
                    data.Process();
                }
            }

            var Buildings = CSV.Tables.Get(Gamefile.Buildings).Datas;
            var Help = new StringBuilder();
            Help.AppendLine("Clashers Republic - AI Base Generator");
            Help.AppendLine("Available Building:");

            foreach (var Building in Buildings)
            {
                if (Building is BuildingData BuildingData)
                {
                    Help.AppendLine($" ID {BuildingData.InstanceId}\n Name: {BuildingData.Name}\n Alt Mode: {!string.IsNullOrEmpty(BuildingData.GearUpBuilding) || BuildingData.AltAttackMode}\n");
                }
            }


            Help.AppendLine("Command:\n/AIBase {Id-Here} {Attack-Mode-Here-If Available (Optional)}");

            Help.AppendLine("Example:");
            Help.AppendLine(" /AIBase 1 (Will generate normal town hall)\n/AIBase 1 true (Will not generate anything due to Alt Mode is not avaiable)\n/AIBase 9      (Will generate normal archer tower)\n/AIBase 9 true (Will generate fast attack archer tower)");
            Constants.AIBaseHelp = Help;

            Console.WriteLine(Gamefiles.Count + " CSV Files loaded and stored in memory.");
        }
    }
}
