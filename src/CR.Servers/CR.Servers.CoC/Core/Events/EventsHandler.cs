using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Packets.Messages.Server.Home;

namespace CR.Servers.CoC.Core.Events
{
    internal class EventsHandler
    {
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler Handler, bool Enabled);

        private static EventHandler EHandler;
        private delegate void EventHandler();

        internal static void Initialize()
        {
            EventsHandler.EHandler += EventsHandler.Process;
            EventsHandler.SetConsoleCtrlHandler(EventsHandler.EHandler, true);
        }
       
        internal static void Process()
        {
            Console.WriteLine("- SHUTDOWN -");

            Logging.Info(typeof(EventHandler), "Server is shutting down, disconnecting player...");
            Resources.Closing = true;
            Task.Run(() =>
            {
                foreach (Player Player in Resources.Accounts.Players.Values.ToArray())
                {
                    if (Player.Connected)
                    {
                       new Disconnected(Player.Level.GameMode.Device).Send();
                    }
                }
            });


            Task.WaitAll(Resources.Accounts.Saves());
            Task.WaitAll(Resources.Clans.Saves());
            //Task AllSave = Task.Run(() => Resources.Clans.Save());

            //AllSave.Wait();
            //Message.Wait();
        }
    }
}