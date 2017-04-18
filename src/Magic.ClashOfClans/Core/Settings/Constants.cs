using System;
using System.Configuration;
using System.Reflection;
using Magic.Helpers;

namespace Magic.Core.Settings
{
    internal class Constants
    {
        public static string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static string DefaultTitle = "Magic.ClashOfClans v" + Version + " - © 2017 | ";
        public static bool IsRc4 = Utils.ParseConfigBoolean("use_rc4");  // false = Pepper Crypto
        public static int BufferSize = 4096;
    }
}
