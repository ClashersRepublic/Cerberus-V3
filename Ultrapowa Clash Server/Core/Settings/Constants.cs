using System;
using System.Configuration;
using System.Reflection;
using UCS.Helpers;

namespace UCS.Core.Settings
{
    internal class Constants
    {
        public static string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static string DefaultTitle = "Ultrapowa Clash Server v" + Version + " - © 2016 | ";
        public static bool IsRc4 = Utils.ParseConfigBoolean("UseRc4");  // false = Pepper Crypto
        public static int BufferSize = 4096;
    }
}
