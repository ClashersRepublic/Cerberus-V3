using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Magic.Royale.Core;

namespace Magic.Royale.Files
{
    internal static class Cards
    {
        internal static string Starting_Card= string.Empty;
        internal static string JsonPath = "Gamefiles/starting_cards.json";

        internal static void Initialize()
        {
            if (!Directory.Exists("Gamefiles/"))
                throw new DirectoryNotFoundException("Directory Gamefiles does not exist!");

            if (!File.Exists(JsonPath))
                throw new Exception($"{JsonPath} does not exist in current directory!");

            Cards.Starting_Card = Regex.Replace(File.ReadAllText(JsonPath, Encoding.UTF8), "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");

            Logger.SayInfo("Deck successfully loaded and stored in memory.");

        }
    }
}