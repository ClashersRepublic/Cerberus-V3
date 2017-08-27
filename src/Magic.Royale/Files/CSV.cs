using System.Collections.Generic;
using System.IO;
using Magic.Royale.Core;
using Magic.Royale.Files.CSV_Reader;
using Magic.Royale.Logic.Enums;

namespace Magic.Royale.Files
{
    internal static class CSV
    {
        internal static readonly Dictionary<int, string> Gamefiles = new Dictionary<int, string>();

        internal static Gamefiles Tables;

        internal static void Initialize()
        {
            Gamefiles.Add((int) Gamefile.Treasure_Chests, @"Gamefiles/csv_logic/treasure_chests.csv");
            Gamefiles.Add((int) Gamefile.Rarities, @"Gamefiles/csv_logic/rarities.csv");
            Gamefiles.Add((int) Gamefile.Spells_Characters, @"Gamefiles/csv_logic/spells_characters.csv");
            Gamefiles.Add((int) Gamefile.Spells_Buildings, @"Gamefiles/csv_logic/spells_buildings.csv");
            Gamefiles.Add((int) Gamefile.Spells_Other, @"Gamefiles/csv_logic/spells_other.csv");
            Gamefiles.Add((int) Gamefile.Arenas, @"Gamefiles/csv_logic/Arenas.csv");
            Tables = new Gamefiles();

            //Parallel.ForEach(CSV.Gamefiles, File => //Parallel is slower in this case (When we have load csv it will help)
            foreach (var File in Gamefiles)
                if (new FileInfo(File.Value).Exists)
                    Tables.Initialize(new Table(File.Value), File.Key);
                else
                    throw new FileNotFoundException($"{File.Value} does not exist!");

            Logger.SayInfo(Gamefiles.Count + " CSV Files, loaded and stored in memory.\n");
        }
    }
}