using System;
using System.Threading;
using System.Threading.Tasks;

namespace Magic.ClashOfClans.Core.Events
{
    internal static class Parser
    {
        internal static Thread Thread;

        internal static void Initialize()
        {
            Thread = new Thread(Parse)
            {
                Priority = ThreadPriority.Lowest,
                Name = "Parser"
            };
            Thread.Start();
        }

        internal static async void Parse()
        {
            while (true)
            {
                var Command = Console.ReadKey(false);

                switch (Command.Key)
                {
                    default:
                    {
                        Console.WriteLine();
                        Console.WriteLine("Press H for help");
                        break;
                    }

                    case ConsoleKey.F4:
                    {
                        Logger.SayInfo("Save executed before exiting " + DateTime.Now.ToString("T") + ".");
                        await Task.WhenAll(DatabaseManager.Save(ResourcesManager.GetInMemoryLevels()),
                            DatabaseManager.Save(ResourcesManager.GetInMemoryAlliances())).ConfigureAwait(false);
                        Logger.SayInfo("Save finished at " + DateTime.Now.ToString("T") + ".");
                        Environment.Exit(0);
                        break;
                    }
                    case ConsoleKey.S:
                    {
                        Logger.SayInfo("Save executed at "  + DateTime.Now.ToString("T") + ".");
                        await Task.WhenAll(DatabaseManager.Save(ResourcesManager.GetInMemoryLevels()),
                            DatabaseManager.Save(ResourcesManager.GetInMemoryAlliances())).ConfigureAwait(false);
                        Logger.SayInfo("Save finished at " + DateTime.Now.ToString("T") + ".");
                        break;
                    }


                    case ConsoleKey.C:
                    {
                        Console.Clear();
                        break;
                    }

                    case ConsoleKey.F10:
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("# " + DateTime.Now.ToString("d") + " ---- Entered Bleeding Edge Mode ---- " +
                                          DateTime.Now.ToString("T") + " #");
                        break;
                }
            }
        }
    }
}