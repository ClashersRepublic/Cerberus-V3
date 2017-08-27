using System;
using System.Reflection;
using System.Threading;
using Magic.Royale.Core;
using Magic.Royale.Core.Settings;
using Magic.Royale.Logic.Structure.Game;
using Magic.Royale.Logic.Structure.Slots;
using Magic.Royale.Logic.Structure.Slots.Items;
using Newtonsoft.Json;

namespace Magic.Royale
{
    internal class Program
    {
        public static int OP;

        public static void Main(string[] args)
        {
            UpdateTitle(true);
            Logger.Say();
            Logger.SayAscii(
                @"_________ .__                .__                          __________                   ___.   .__  .__        ");
            Logger.SayAscii(
                @"\_   ___ \|  | _____    _____|  |__   ___________  ______ \______   \ ____ ______  __ _\_ |__ |  | |__| ____  ");
            Logger.SayAscii(
                @"/    \  \/|  | \__  \  /  ___/  |  \_/ __ \_  __ \/  ___/  |       _// __ \\____ \|  |  \ __ \|  | |  |/ ___\ ");
            Logger.SayAscii(
                @"\     \___|  |__/ __ \_\___ \|   Y  \  ___/|  | \/\___ \   |    |   \  ___/|  |_> >  |  / \_\ \  |_|  \  \___ ");
            Logger.SayAscii(
                @" \______  /____(____  /____  >___|  /\___  >__|  /____  >  |____|_  /\___  >   __/|____/|___  /____/__|\___  >");
            Logger.SayAscii(
                @"        \/          \/     \/     \/     \/           \/          \/     \/|__|             \/             \/ ");
            Logger.SayAscii(
                @"                                                                                             Royale Edition  ");

            Logger.SayInfo(Constants.IsRc4 ? "Crypto: RC4" : "Crypto: Pepper");
            Logger.SayInfo(
                @"Clashers Republic's programs are protected by our policies, available only to our partner.");
            Logger.SayInfo(@"Clashers Republic's programs are under the 'Proprietary' license.");
            Logger.SayInfo(@"Clashers Republic is NOT affiliated to 'Supercell Oy'.");
            Logger.SayInfo(@"Clashers Republic does NOT own 'Clash of Clans', 'Boom Beach', 'Clash Royale'." +
                           Environment.NewLine);

            Logger.Say(Assembly.GetExecutingAssembly().GetName().Name + @" is now starting..." + Environment.NewLine);

            Classes.Initialize();
            
            Logger.Say(@"-------------------------------------" + Environment.NewLine);
            //Test.Unpack();
            //Console.WriteLine(Chests.Get(22).Name);

            //Console.WriteLine(Chests.Get("Star") == null);
            /*var a = new ChestGenerator().GenerateChest(new Avatar(), Chests.Get("Free"), new Random());

            Logger.Say("Generating card : ");

            a?.End();
            Console.WriteLine(a.SelectedCard.Count);
            foreach (var cardStacks in a.SelectedCard)
            {
                    Console.WriteLine($"{cardStacks.Card.Name} with value of {cardStacks.Count}");
                
            }*/
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
                Constants.DefaultTitle = Constants.DefaultTitle + "ONLINE | Players > ";
                Console.Title = Constants.DefaultTitle;
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
