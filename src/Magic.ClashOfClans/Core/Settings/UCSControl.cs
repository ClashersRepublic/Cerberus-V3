using System;
using System.Diagnostics;
using System.Threading;
using static System.Console;
using static Magic.Core.Logger;

namespace Magic.Core.Settings
{
    internal class MagicControl
    {
        public static void UpdateTitle(bool Status)
        {
            if (Status == false)
            {
                Console.Title = Constants.DefaultTitle + "OFFLINE";
            }
            else if (Status == true)
            {
                Constants.DefaultTitle = Constants.DefaultTitle + "ONLINE | Players > ";
                Console.Title = Constants.DefaultTitle;
            }
        }

        public static void WelcomeMessage()
        {
            UpdateTitle(true);
            ForegroundColor = ConsoleColor.Green;
            if (Constants.IsRc4)
                WriteLine("[UCS]    > Magic.ClashOfClans is running under RC4 mode. Please make sure CSV is modded to allow RC4 client to connect");
            else
                WriteLine("[UCS]    > Magic.ClashOfClans is running under Pepper mode. Please make sure client key is modded to allow Pepper client to connect");
            Console.Write("[UCS]    ");

            ResetColor();
            WriteLine("\n[UCS]    Preparing Server...\n");
        }
    }
}