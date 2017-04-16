﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UCS.Files.CSV;
using UCS.Files.Logic;
using static UCS.Core.Logger;

namespace UCS.Core
{
    internal static class CSVManager
    {
        private static readonly List<Tuple<string, string, int>> _gameFiles = new List<Tuple<string, string, int>>();
        private static readonly DataTables _dataTables = new DataTables();

        public static void Initialize()
        {
            _gameFiles.Add(new Tuple<string, string, int>("Buildings", @"contents/logic/buildings.csv", 0));
            _gameFiles.Add(new Tuple<string, string, int>("Resources", @"contents/logic/resources.csv", 2));
            _gameFiles.Add(new Tuple<string, string, int>("Characters", @"contents/logic/characters.csv", 3));
            _gameFiles.Add(new Tuple<string, string, int>("Obstacles", @"contents/logic/obstacles.csv", 7));
            _gameFiles.Add(new Tuple<string, string, int>("Experience Levels", @"contents/logic/experience_levels.csv", 10));
            _gameFiles.Add(new Tuple<string, string, int>("Traps", @"contents/logic/traps.csv", 11));
            _gameFiles.Add(new Tuple<string, string, int>("Leagues", @"contents/logic/leagues.csv", 12));
            _gameFiles.Add(new Tuple<string, string, int>("Globals", @"contents/logic/globals.csv", 13));
            _gameFiles.Add(new Tuple<string, string, int>("Townhall Levels", @"contents/logic/townhall_levels.csv", 14));
            _gameFiles.Add(new Tuple<string, string, int>("NPCs", @"contents/logic/npcs.csv", 16));
            _gameFiles.Add(new Tuple<string, string, int>("Decos", @"contents/logic/decos.csv", 17));
            _gameFiles.Add(new Tuple<string, string, int>("Shields", @"contents/logic/shields.csv", 19));
            _gameFiles.Add(new Tuple<string, string, int>("Achievements", @"contents/logic/achievements.csv", 22));
            _gameFiles.Add(new Tuple<string, string, int>("Spells", @"contents/logic/spells.csv", 25));
            _gameFiles.Add(new Tuple<string, string, int>("Heroes", @"contents/logic/heroes.csv", 27));
            /*
            gameFiles.Add(new Tuple<string, string, int>("Alliance Badge Layers", @"Gamefiles/logic/alliance_badge_layers.csv", 30));
            gameFiles.Add(new Tuple<string, string, int>("Alliance Badges", @"Gamefiles/logic/alliance_badges.csv", 31));
            gameFiles.Add(new Tuple<string, string, int>("Alliance Levels", @"Gamefiles/logic/alliance_levels.csv", 32));
            gameFiles.Add(new Tuple<string, string, int>("Alliance Portal", @"Gamefiles/logic/alliance_portal.csv", 33));
            gameFiles.Add(new Tuple<string, string, int>("Buildings Classes", @"Gamefiles/logic/building_classes.csv", 34));
            gameFiles.Add(new Tuple<string, string, int>("Effects", @"Gamefiles/logic/effects.csv", 35));
            gameFiles.Add(new Tuple<string, string, int>("Locales", @"Gamefiles/logic/locales.csv", 36));
            gameFiles.Add(new Tuple<string, string, int>("Missions", @"Gamefiles/logic/missions.csv", 37));
            gameFiles.Add(new Tuple<string, string, int>("Projectiles", @"Gamefiles/logic/projectiles.csv", 38));
            gameFiles.Add(new Tuple<string, string, int>("Regions", @"Gamefiles/logic/regions.csv", 39));
            gameFiles.Add(new Tuple<string, string, int>("Variables", @"Gamefiles/logic/variables.csv", 40)); 
            gameFiles.Add(new Tuple<string, string, int>("War", @"Gamefiles/logic/war.csv", 28));
            */
            try
            {
                Parallel.ForEach(_gameFiles, file =>
                {
                    // Just to figure out where CSV fails at.
                    Say($"Loading CSV table {file.Item1} at {file.Item2}...");

                    _dataTables.InitDataTable(new CSVTable(file.Item2), file.Item3);
                });
                Say("CSV Tables  have been successfully loaded.");
            }
            catch (Exception Ex)
            {
                Say();
                Error("Error loading game files. Looks like you have :");
                Error("     -> Edited the Files Wrong");
                Error("     -> Made mistakes by deleting values");
                Error("     -> Entered too High or Low value");
                Error("     -> Please check to these errors");
                Error(Ex.ToString());
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static List<Tuple<string, string, int>> Gamefiles => _gameFiles;
        public static DataTables DataTables => _dataTables;
    }
}
