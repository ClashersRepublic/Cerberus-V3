using System;
using System.Collections.Generic;
using System.IO;
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
            Gamefiles.Add((int)Gamefile.Obstacles, @"Gamefiles/logic/obstacles.csv");
            Gamefiles.Add((int)Gamefile.Traps, @"Gamefiles/logic/traps.csv");
            Gamefiles.Add((int)Gamefile.Building_Classes, @"Gamefiles/logic/building_classes.csv");
            Gamefiles.Add((int)Gamefile.Globals, @"Gamefiles/logic/globals.csv");
            Gamefiles.Add((int)Gamefile.Experience_Levels, @"Gamefiles/logic/experience_levels.csv");
            Gamefiles.Add((int)Gamefile.Townhall_Levels, @"Gamefiles/logic/townhall_levels.csv");
            Gamefiles.Add((int)Gamefile.Npcs, @"Gamefiles/logic/npcs.csv");
            Gamefiles.Add((int)Gamefile.Missions, @"Gamefiles/logic/missions.csv");
            Gamefiles.Add((int)Gamefile.Spells, @"Gamefiles/logic/spells.csv");
            Gamefiles.Add((int)Gamefile.Heroes, @"Gamefiles/logic/heroes.csv");
            Gamefiles.Add((int)Gamefile.Variables, @"Gamefiles/logic/variables.csv");
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

            Console.WriteLine(Gamefiles.Count + " CSV Files loaded and stored in memory.");
        }
    }
}
