using System.Reflection;
using Magic.ClashOfClans.Extensions;

namespace Magic.ClashOfClans.Core.Settings
{
    internal class Constants
    {
        public static string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static string DefaultTitle = "Magic.ClashOfClans v" + Version + " - © 2017 | ";
        public static bool IsRc4 = Utils.ParseConfigBoolean("use_rc4"); // false = Pepper Crypto

        public const int BufferSize = 2048;
        public const int Verbosity = 3;
        public const int MaxCommand = 0;


        public static readonly string PatchServer = Utils.ParseConfigString("PatchUrl");
        public static readonly string UpdateServer = Utils.ParseConfigString("UpdateUrl");
    }
}
