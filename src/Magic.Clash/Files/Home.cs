using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Magic.ClashOfClans.Core;

namespace Magic.ClashOfClans.Files
{
    internal static class Home
    {
        internal static string Starting_Home = string.Empty;
        internal static string JsonPath = "Gamefiles/level/starting_home.json";

        internal static void Initialize()
        {
            if (!Directory.Exists("Gamefiles/level/"))
                throw new DirectoryNotFoundException("Directory Gamefiles/level does not exist!");

            if (!File.Exists(JsonPath))
                throw new Exception($"{JsonPath} does not exist in current directory!");

            Starting_Home = Regex.Replace(File.ReadAllText(JsonPath, Encoding.UTF8), "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");

            Logger.SayInfo("Home successfully loaded and stored in memory.");

        }
    }
}