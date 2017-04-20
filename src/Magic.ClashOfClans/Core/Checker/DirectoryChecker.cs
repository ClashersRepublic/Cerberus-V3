using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.ClashOfClans.Core.Checker
{
    internal class DirectoryChecker
    {
        public static void CheckDirectories()
        {
            if(!Directory.Exists("logs"))
                Directory.CreateDirectory("Logs");

            if (!Directory.Exists("patch"))
                Directory.CreateDirectory("patch");

            if (!Directory.Exists("tools"))
                Directory.CreateDirectory("tools");

            if (!Directory.Exists("libs"))
                Directory.CreateDirectory("libs");

            if (!Directory.Exists("contents"))
                Directory.CreateDirectory("contents");
        }

        public static void CheckFiles()
        {
			if (!File.Exists("filter.ucs"))
                File.WriteAllText("filter.ucs", "./savegame");
        }
    }
}
