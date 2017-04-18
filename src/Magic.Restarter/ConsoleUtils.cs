using System;
using System.Reflection;

namespace Magic.Restarter
{
    public static class ConsoleUtils
    {
        public static void Welcome()
        {
            Console.WriteLine();
            WriteLineCenterYellow(" ____ ___.__   __                                                  ");
            WriteLineCenterDarkYellow("|    |   \\  |_/  |_____________  ______   ______  _  _______       ");
            WriteLineCenterYellow("|    |   /  |\\   __\\_  __ \\__  \\ \\____ \\ /  _ \\ \\/ \\/ /\\__  \\      ");
            WriteLineCenterDarkYellow("|    |  /|  |_|  |  |  | \\// __ \\|  |_> >  <_> )     /  / __ \\_    ");
            WriteLineCenterYellow("|______/ |____/__|  |__|  (____  /   __/ \\____/ \\/\\_/  (____  /    ");
            WriteLineCenterDarkYellow("                               \\/|__|    RESTARTER V" + Assembly.GetExecutingAssembly().GetName().Version + "\\/     ");
            WriteLineCenterYellow("            ");

            WriteLineCenterGreen("Server Restarter has been loaded successfully.\n");
            WriteLineCenterCyan("Source code: https://www.github.com/PatrikCoC/ucs-restarter");
            WriteLineCenterCyan("This program was made by @FICTURE7 and @PatrikCoC.");
            WriteLineCenterYellow("Make sure to edit 'restarter.config' file for your needs!");
        }

        public static void WriteLineCenter(string value)
        {
            Console.SetCursorPosition((Console.WindowWidth - value.Length) / 2, Console.CursorTop);
            Console.WriteLine(value);
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
        }

        public static void WriteLineCenterDarkYellow(string value)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition((Console.WindowWidth - value.Length) / 2, Console.CursorTop);
            Console.WriteLine(value);
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
            Console.ResetColor();
        }

        public static void WriteLineCenterGreen(string value)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition((Console.WindowWidth - value.Length) / 2, Console.CursorTop);
            Console.WriteLine(value);
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
            Console.ResetColor();
        }

        public static void WriteLineCenterRed(string value)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition((Console.WindowWidth - value.Length) / 2, Console.CursorTop);
            Console.WriteLine(value);
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
            Console.ResetColor();
        }

        public static void WriteLineCenterCyan(string value)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition((Console.WindowWidth - value.Length) / 2, Console.CursorTop);
            Console.WriteLine(value);
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
            Console.ResetColor();
        }

        public static void WriteLineCenterYellow(string value)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition((Console.WindowWidth - value.Length) / 2, Console.CursorTop);
            Console.WriteLine(value);
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
            Console.ResetColor();
        }
    }
}
