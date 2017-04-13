using System;
using System.Configuration;
using System.Reflection;
using UCS.Helpers;

namespace UCS.Core.Settings
{
    internal class Constants
    {
        public static string Version                 = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static string DefaultTitle            = "Ultrapowa Clash Server v" + Version + " - © 2016 | ";

        public const int MemoryInterval              = 900000; 
        
        public const bool IsPremiumServer            = true;  // false = max100 Online Players; true = unlimited Online Players
        public static bool IsRc4                     = Utils.ParseConfigBoolean("UseRc4");  // false = Pepper Crypto

        public const string RedisAddr                = "192.168.0.27";
        public const int RedisPort                   = 6379;

        public static int MaxOnlinePlayers           = 50000;

    }
}
