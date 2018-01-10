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
            Settings.PatchServer = Extension.ParseConfigString("Game:PatchURL");
            Settings.UpdateServer = Extension.ParseConfigString("Game:UpdateURL");
            Settings.EventServer = Extension.ParseConfigString("Game:EventURL");
            Settings.GameAssetsServer = Extension.ParseConfigString("Game:GameAssetsURL");
            Settings.ClientMajorVersion = Extension.ParseConfigString("Game:ClientMajorVersion");
            Settings.ClientMinorVersion = Extension.ParseConfigString("Game:ClientMinorVersion");
        }
    }
}