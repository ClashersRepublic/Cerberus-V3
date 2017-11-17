using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Logic;

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

            Logging.Info(typeof(EventHandler), "Server is shutting down, warning players about the maintenance...");

            /*Task Message = Task.Run(() =>
            {
                foreach (Player Player in Resources.Accounts.Players.Values.ToArray())
                {
                    if (Player.Connected)
                    {
                       // new Maintenance_Inbound_Message(Player.Level.GameMode.Device).Send();
                    }
                }
            });*/

            Task.WaitAll(Resources.Accounts.Saves());
            //Task AllSave = Task.Run(() => Resources.Clans.Save());

            //AllSave.Wait();
//Message.Wait();
        }
    }
}