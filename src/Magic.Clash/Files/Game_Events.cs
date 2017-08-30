﻿using Magic.ClashOfClans.Logic.Structure.Slots;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Magic.ClashOfClans.Core;

namespace Magic.ClashOfClans.Files
{
    internal static class Game_Events
    {
        internal static string Events_Json = string.Empty;
        internal static Calendar Events_Calendar = new Calendar();
        internal static string JsonPath = "Gamefiles/events.json";

        internal static void Initialize()
        {
            if (!Directory.Exists("Gamefiles/level/"))
                throw new DirectoryNotFoundException("Directory Gamefiles/level does not exist!");

            if (!File.Exists(JsonPath))
                throw new Exception($"{JsonPath} does not exist in current directory!");

            Events_Json = Regex.Replace(File.ReadAllText(JsonPath, Encoding.UTF8), "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");
            JsonConvert.PopulateObject(Events_Json, Events_Calendar);
            Logger.SayInfo("Game Events successfully loaded and stored in memory.");
        }
    }
}