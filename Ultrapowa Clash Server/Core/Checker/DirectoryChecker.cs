using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCS.Core.Checker
{
    internal class DirectoryChecker
    {
        public static void CheckDirectories()
        {
            if(!Directory.Exists("Logs"))
                Directory.CreateDirectory("Logs");

            if (!Directory.Exists("Patch"))
                Directory.CreateDirectory("Patch");

            if (!Directory.Exists("Tools"))
                Directory.CreateDirectory("Tools");

            if (!Directory.Exists("Library"))
                Directory.CreateDirectory("Library");

            if (!Directory.Exists("Gamefiles"))
                Directory.CreateDirectory("Gamefiles");
        }

        public static void CheckFiles()
        {
			if (!File.Exists("filter.ucs"))
                File.WriteAllText("filter.ucs", "./savegame");
        }
    }
}
