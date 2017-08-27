using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Magic.Royale.Core;

namespace Magic.Royale.Files
{
    internal static class Deck
    {
        internal static string Starting_Deck = string.Empty;
        internal static string JsonPath = "Gamefiles/starting_deck.json";

        internal static void Initialize()
        {
            if (!Directory.Exists("Gamefiles/"))
                throw new DirectoryNotFoundException("Directory Gamefiles does not exist!");

            if (!File.Exists(JsonPath))
                throw new Exception($"{JsonPath} does not exist in current directory!");

            Deck.Starting_Deck = Regex.Replace(File.ReadAllText(JsonPath, Encoding.UTF8), "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");

            Logger.SayInfo("Deck successfully loaded and stored in memory.");

        }
    }
}