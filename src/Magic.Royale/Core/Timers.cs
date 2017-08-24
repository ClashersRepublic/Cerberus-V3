using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic.ClashOfClans.Network;
using Magic.ClashOfClans.Extensions;
using Magic.ClashOfClans.Logic.Enums;
using Timer = System.Timers.Timer;

namespace Magic.ClashOfClans.Core
{
    internal static class Timers
    {

        internal static readonly Dictionary<Logic.Enums.Timer, Timer> LTimers = new Dictionary<Logic.Enums.Timer, Timer>();

        internal static void Initialize()
        {
            Save();
            KeepAlive();
        }

        internal static void KeepAlive()
        {
            Timer Timer = new Timer
            {
                Interval = 60000,
                AutoReset = true
            };

            Timer.Elapsed += (_Sender, _Args) =>
            {
                var numDisc = 0;
#if DEBUG
                Logger.SayInfo("KeepAlive executed at " + DateTime.Now.ToString("T") +  ".");
#endif
                var clients = ResourcesManager.GetConnectedClients();
                foreach (var client in clients)
                {
                    if (DateTime.Now > client.NextKeepAlive)
                    {
                        ResourcesManager.DropClient(client.GetSocketHandle());
                        numDisc++;
                    }
                }

#if DEBUG
                if (numDisc > 0)
                    Logger.SayInfo($"KeepAlive dropped {numDisc} clients due to keep alive timeouts at " + DateTime.Now.ToString("T") +  ".");
#endif
                Logger.SayInfo("#" + DateTime.Now.ToString("d") + " ---- Pools ---- " + DateTime.Now.ToString("T") + " #");
                Logger.SayInfo($"SocketAsyncEventArgs: created -> {Gateway.NumberOfArgsCreated} in-use -> {Gateway.NumberOfArgsInUse} available -> {Gateway.NumberOfArgs}.");
                Logger.SayInfo($"Buffers: created -> {Gateway.NumberOfBuffersCreated} in-use -> {Gateway.NumberOfBuffersInUse} available -> {Gateway.NumberOfBuffers}.");
            };

            LTimers.Add(Logic.Enums.Timer.Keep_Alive, Timer);
        }

        internal static void Save()
        {
            Timer Timer = new Timer
            {
                Interval = TimeSpan.FromMinutes(30).TotalMilliseconds,
                AutoReset = true
            };

            Timer.Elapsed += async (_Sender, _Args) =>
            {
#if DEBUG
                Logger.SayInfo("Save executed at " + DateTime.Now.ToString("T") + ".");
#endif
                try
                {
                    await Task.WhenAll(DatabaseManager.Save(ResourcesManager.GetInMemoryLevels())).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    ExceptionLogger.Log(ex,"[: Failed at " + DateTime.Now.ToString("T") + ']' + Environment.NewLine + ex.StackTrace);
                    return;
                }
#if DEBUG

                Logger.SayInfo("Save finished at " + DateTime.Now.ToString("T") + ".");
#endif
            };

            LTimers.Add(Logic.Enums.Timer.Save, Timer);
        }

        internal static void Run()
        {
            foreach (Timer Timer in LTimers.Values)
            {
                Timer.Start();
            }
        }
    }
}
