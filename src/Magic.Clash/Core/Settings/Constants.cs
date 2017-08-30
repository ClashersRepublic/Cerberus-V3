using System.Reflection;
using Magic.ClashOfClans.Extensions;

namespace Magic.ClashOfClans.Core.Settings
{
    internal class Constants
    {
        public static string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static string DefaultTitle = "Magic.ClashOfClans v" + Version + " - © 2017 | ";
        public static bool IsRc4 = Utils.ParseConfigBoolean("UseRC4");

        public const int BufferSize = 2048;
        public static readonly int Verbosity = Utils.ParseConfigInt("Verbosity");
        public const int MaxCommand = 0;

        public static readonly bool UseDiscord = Utils.ParseConfigBoolean("UseDiscord");
        public static readonly string DiscordPrefix = Utils.ParseConfigString("DiscordPrefix");
        public static readonly string DiscordToken = Utils.ParseConfigString("DiscordToken");

        public static readonly string PatchServer = Utils.ParseConfigString("PatchUrl");
        public static readonly string UpdateServer = Utils.ParseConfigString("UpdateUrl");
        internal static readonly string[] ClientVersion = Utils.ParseConfigString("ClientVersion").Split('.');
    }
}
