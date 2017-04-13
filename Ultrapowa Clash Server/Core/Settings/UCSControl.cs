using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using UCS.Core.Threading;
using UCS.Core.Web;
using static System.Console;
using static UCS.Core.Logger;

namespace UCS.Core.Settings
{
    internal class UCSControl
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

        public static void UpdateGuiStatus()
        {
            if (Console.Title.Contains("ONLINE"))
            {
                UCSUI Gui = (UCSUI)Application.OpenForms["UCSUI"];
                Gui.labelOnlineStatus.Text = "ONLINE";
            }
        }

        public static void WelcomeMessage()
        {
            UpdateTitle(false);
            WriteLine("[UCS]    > This program was made by the Ultrapowa Development Team.\n[UCS]    > Ultrapowa is not affiliated to \"Supercell, Oy\".\n[UCS]    > UCS is proudly licensed under MIT.\n[UCS]    > Visit www.ultrapowa.com daily for News & Updates!\n[UCS]    > To have proper saving, make sure the CSV files inside the app AND the emulator csv files are the same!");
            ForegroundColor = ConsoleColor.Green;
            if (Constants.IsRc4)
                WriteLine("[UCS]    > UCS is running under RC4 mode. Please make sure CSV is modded to allow RC4 client to connect");
            else
                WriteLine("[UCS]    > UCS is running under Pepper mode. Please make sure client key is modded to allow Pepper client to connect");
            Console.Write("[UCS]    ");
            ForegroundColor = ConsoleColor.DarkGreen;
            WriteLine("> UCS is up-to-date: " + Constants.Version);
            ResetColor();
            WriteLine("\n[UCS]    Preparing Server...\n");
        }
    }
}