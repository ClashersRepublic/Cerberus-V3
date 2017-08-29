using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Network;

namespace Magic.ClashOfClans.Core
{
    internal static class Timers
    {
        internal static readonly Dictionary<Timer, System.Timers.Timer> LTimers =
            new Dictionary<Timer, System.Timers.Timer>();

        internal static void Initialize()
        {
            Save();
            KeepAlive();
            Random();
            Restart();
        }

        internal static void Restart()
        {
            if (!TimeSpan.TryParse(ConfigurationManager.AppSettings["RestartInterval"], out TimeSpan interval))
            {
                interval = TimeSpan.FromMinutes(30);
            }
            if (Math.Abs(interval.TotalSeconds) > 60)
            {

                Logger.SayInfo($"Server Restarter has been loaded successfully");
                var Timer = new System.Timers.Timer
                {
                    Interval = interval.TotalMilliseconds,
                    AutoReset = true
                };

                Timer.Elapsed += async (_Sender, _Args) =>
                {
                    try
                    {
                        await Task.WhenAll(DatabaseManager.Save(ResourcesManager.GetInMemoryLevels()), DatabaseManager.Save(ResourcesManager.GetInMemoryAlliances())).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        ExceptionLogger.Log(ex,
                            "[: Failed at " + DateTime.Now.ToString("T") + ']' + Environment.NewLine + ex.StackTrace);
                        return;
                    }

                    Environment.Exit(0);
                };
                LTimers.Add(Logic.Enums.Timer.Restart, Timer);
            }
            else
            {
                Logger.SayInfo("Server Restarter has been disabled");
            }
        }

        internal static void Random()
        {
            var Timer = new System.Timers.Timer
            {
                Interval = TimeSpan.FromHours(1).TotalMilliseconds,
                AutoReset = true
            };
            Timer.Elapsed += (_Sender, _Args) =>
            {
                ObjectManager.Random = new Random(DateTime.Now.ToString("T").GetHashCode());
            };
            LTimers.Add(Logic.Enums.Timer.Random, Timer);
        }


        internal static void KeepAlive()
        {
            var Timer = new System.Timers.Timer
            {
                Interval = 60000,
                AutoReset = true
            };

            Timer.Elapsed += (_Sender, _Args) =>
            {
                var numDisc = 0;
#if DEBUG
                Logger.SayInfo("KeepAlive executed at " + DateTime.Now.ToString("T") + ".");
#endif
                var clients = ResourcesManager.GetConnectedClients();
                foreach (var client in clients)
                    if (DateTime.Now > client.NextKeepAlive)
                    {
                        ResourcesManager.DropClient(client.GetSocketHandle());
                        numDisc++;
                    }

#if DEBUG
                if (numDisc > 0)
                    Logger.SayInfo($"KeepAlive dropped {numDisc} clients due to keep alive timeouts at " +
                                   DateTime.Now.ToString("T") + ".");
#endif
                Logger.SayInfo("#" + DateTime.Now.ToString("d") + " ---- Pools ---- " + DateTime.Now.ToString("T") +
                               " #");
                Logger.SayInfo(
                    $"SocketAsyncEventArgs: created -> {Gateway.NumberOfArgsCreated} in-use -> {Gateway.NumberOfArgsInUse} available -> {Gateway.NumberOfArgs}.");
                Logger.SayInfo(
                    $"Buffers: created -> {Gateway.NumberOfBuffersCreated} in-use -> {Gateway.NumberOfBuffersInUse} available -> {Gateway.NumberOfBuffers}.");
            };

            LTimers.Add(Logic.Enums.Timer.Keep_Alive, Timer);
        }

        internal static void Save()
        {
            var Timer = new System.Timers.Timer
            {
                Interval = TimeSpan.FromMinutes(25).TotalMilliseconds,
                AutoReset = true
            };

            Timer.Elapsed += async (_Sender, _Args) =>
            {
#if DEBUG
                Logger.SayInfo("Save executed at " + DateTime.Now.ToString("T") + ".");
#endif
                try
                {
                    await Task.WhenAll(DatabaseManager.Save(ResourcesManager.GetInMemoryLevels()), DatabaseManager.Save(ResourcesManager.GetInMemoryAlliances())).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    ExceptionLogger.Log(ex,
                        "[: Failed at " + DateTime.Now.ToString("T") + ']' + Environment.NewLine + ex.StackTrace);
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
            foreach (var Timer in LTimers.Values)
                Timer.Start();
        }
    }
}
