using CR.Servers.CoC.Extensions;

namespace CR.Servers.CoC.Core
{
    internal class Settings
    {
        public static string PatchServer;
        public static string UpdateServer;
        public static string EventServer;
        public static string ClientMajorVersion;
        public static string ClientMinorVersion;

        public static void Initialize()
        {
            PatchServer = Extension.ParseConfigString("Game:PatchUrl");
            UpdateServer = Extension.ParseConfigString("Game:UpdateUrl");
            EventServer = Extension.ParseConfigString("Game:EventUrl");
            ClientMajorVersion = Extension.ParseConfigString("Game:ClientMajorVersion");
            ClientMinorVersion = Extension.ParseConfigString("Game:ClientMinorVersion");

        }
    }
}
