namespace CR.Servers.CoC.Core
{
    using CR.Servers.CoC.Extensions;

    internal class Settings
    {
        public static string PatchServer;
        public static string UpdateServer;
        public static string EventServer;
        public static string GameAssetsServer;
        public static string ClientMajorVersion;
        public static string ClientMinorVersion;

        public static void Initialize()
        {
            PatchServer = Extension.ParseConfigString("Game:PatchURL");
            UpdateServer = Extension.ParseConfigString("Game:UpdateURL");
            EventServer = Extension.ParseConfigString("Game:EventURL");
            GameAssetsServer = Extension.ParseConfigString("Game:GameAssetsURL");
            ClientMajorVersion = Extension.ParseConfigString("Game:ClientMajorVersion");
            ClientMinorVersion = Extension.ParseConfigString("Game:ClientMinorVersion");
        }
    }
}