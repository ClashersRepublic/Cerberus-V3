using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CR.Servers.CoC.Logic;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Files
{
    public class LevelFile
    {
        internal static JObject StartingHome;
        internal static Dictionary<string, Home> Files;

        internal static void Initialize()
        {
            LevelFile.Files = new Dictionary<string, Home>(200);
            LevelFile.StartingHome = JObject.Parse(File.ReadAllText("Gamefiles/level/starting_home.json", Encoding.UTF8));

            /*foreach (NpcData Data in CSV.Tables.Get(Gamefile.Npcs).Datas)
            {
                if (Data.LevelFile != null)
                {
                    LevelFile.Files.Add(Data.LevelFile, new Home
                    {
                        LastSave = File.ReadAllText("Gamefiles/" + Data.LevelFile)
                    });
                }
            }*/
        }
    }
}