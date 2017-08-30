using System;
using System.IO;
using Magic.ClashOfClans.Core;
using Newtonsoft.Json.Linq;

namespace Magic.Files
{
    internal static class Fingerprint
    {
        internal static string Json;
        internal static string Sha;
        internal static string[] Version;
        internal static bool Custom;

        internal static void Initialize()
        {
            try
            {
                if (!Patches())
                {
                    if (File.Exists(@"Gamefiles\fingerprint.json"))
                    {
                        Json = File.ReadAllText(Directory.GetCurrentDirectory() + @"\Gamefiles\fingerprint.json");
                        JObject _Json = JObject.Parse(Json);
                        Sha = _Json["sha"].ToObject<string>();
                        Version = _Json["version"].ToObject<string>().Split('.');
                        Logger.SayInfo("Default patch detected, with version " + string.Join(".", Version) + "." + Environment.NewLine);
                    }
                    else
                    {
                        Logger.SayInfo("The Fingerprint cannot be loaded, the file does not exist." + Environment.NewLine);
                    }
                }
                else
                {
                    Logger.SayInfo("Custom patch detected, with version " + string.Join(".", Version) + "." + Environment.NewLine);
                }
            }
            catch (Exception)
            {
                Logger.SayInfo("An error occured while parsing the fingerprint." + Environment.NewLine);
            }
        }

        internal static bool Patches()
        {
            bool _Result = false;

            if (File.Exists(Directory.GetCurrentDirectory() + "\\Patchs\\VERSION"))
            {
                string[] _Lines = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\Patchs\\VERSION");

                if (!string.IsNullOrEmpty(_Lines[0]))
                {
                    Version = _Lines[0].Split('.');

                    if (_Lines.Length > 1 && !string.IsNullOrEmpty(_Lines[1]))
                    {
                        Sha = _Lines[1];

                        if (File.Exists(Directory.GetCurrentDirectory() + "\\Patchs\\" + Sha + "\\fingerprint.json"))
                        {
                            Json = File.ReadAllText(Directory.GetCurrentDirectory() + "\\Patchs\\" + Sha + "\\fingerprint.json");
                            Json.Trim('\n', '\r');

                            _Result = true;
                            Custom = true;
                        }
                    }
                }
            }
            return _Result;
        }
    }
}