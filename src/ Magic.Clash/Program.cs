using System;
using System.Reflection;
using System.Threading;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Core.Settings;
using Magic.ClashOfClans.Extensions;
using global::Discord;

namespace Magic.ClashOfClans
{
    internal class Program
    {
        public static int OP;
//old Code MySQL coc 9.105

        public static void Main(string[] args)
        {
            UpdateTitle(true);
            Logger.Say();
            Logger.SayAscii(@"_________ .__                .__                          __________                   ___.   .__  .__        ");
            Logger.SayAscii(@"\_   ___ \|  | _____    _____|  |__   ___________  ______ \______   \ ____ ______  __ _\_ |__ |  | |__| ____  ");
            Logger.SayAscii(@"/    \  \/|  | \__  \  /  ___/  |  \_/ __ \_  __ \/  ___/  |       _// __ \\____ \|  |  \ __ \|  | |  |/ ___\ ");
            Logger.SayAscii(@"\     \___|  |__/ __ \_\___ \|   Y  \  ___/|  | \/\___ \   |    |   \  ___/|  |_> >  |  / \_\ \  |_|  \  \___ ");
            Logger.SayAscii(@" \______  /____(____  /____  >___|  /\___  >__|  /____  >  |____|_  /\___  >   __/|____/|___  /____/__|\___  >");
            Logger.SayAscii(@"        \/          \/     \/     \/     \/           \/          \/     \/|__|             \/             \/ ");
            Logger.SayAscii(@"                                                                                             Clash Edition  ");

            Logger.SayInfo(Constants.IsRc4 ? "Crypto: RC4" : "Crypto: None");
            Logger.Say();

            Logger.Say(Assembly.GetExecutingAssembly().GetName().Name + @" is now starting..." + Environment.NewLine);

            NativeCalls.DisableMenus();
            Logger.Say("Menu has been disabled. Exit the emulator by pressing the F4 Key.");

            Classes.Initialize();

            Logger.Say(@"-------------------------------------" + Environment.NewLine);
            Thread.Sleep(Timeout.Infinite);
        }

        public static void UpdateTitle(bool Status)
        {
            if (Status == false)
            {
                Console.Title = Constants.DefaultTitle + "OFFLINE";
            }
            else
            {
                Constants.DefaultTitle = Constants.DefaultTitle + "ONLINE | Active Connections > ";
                Console.Title = Constants.DefaultTitle + OP;
            }
        }

        public static void TitleAd()
        {
            Console.Title = Constants.DefaultTitle + Interlocked.Increment(ref OP);
        }

        public static void TitleDe()
        {
            Console.Title = Constants.DefaultTitle + Interlocked.Decrement(ref OP);
        }
    }
}
