using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Extensions;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.CoC.Packets.Messages.Server.Home;
using Microsoft.VisualBasic.CompilerServices;
using NLog;
using Timer = System.Timers.Timer;

namespace CR.Servers.CoC.Core
{
    internal class Timers : ConcurrentDictionary<ServerTimer, Timer>
    {

        internal bool Initialized;

        internal Timers()
        {
            if (this.Initialized)
            {
                return;
            }

            this.Save();
            this.Restart();
            this.Initialized = true;

            this.Run();
        }

        internal void Stop()
        {
            foreach (var Timer in this.Values)
            {
                Timer.Stop();
            }

            this.Initialized = false;
        }

        internal void Run()
        {
            if (this.Initialized)
            {
                foreach (var Timer in this.Values)
                    Timer.Start();
            }
        }


        internal void Save()
        {
            var Timer = new Timer
            {
                Interval = TimeSpan.FromHours(1).TotalMilliseconds,
                AutoReset = true
            };

            Timer.Elapsed += async (_Sender, _Args) =>
            {
                await Task.WhenAll(Resources.Accounts.Saves());
                await Task.WhenAll(Resources.Clans.Saves());

                Logging.Info(typeof(Timers), "Sucessfuly saved players and clans at " + DateTime.Now.ToString("T"));
            };

            if (!this.TryAdd(ServerTimer.DataSaving, Timer))
                Logging.Error(this.GetType(), $"Failed to add DataSaving timer to ServerTimer dictionary");

        }

        internal void KeepAlive()
        {
            var Timer = new Timer
            {
                Interval = 60000,
                AutoReset = true
            };

            Timer.Elapsed += (_Sender, _Args) =>
            {

            };

            if (!this.TryAdd(ServerTimer.KeepAliveCheck, Timer))
                Logging.Error(this.GetType(), $"Failed to add KeepAlive timer to ServerTimer dictionary");

        }

        internal void Restart()
        {
            if (!TimeSpan.TryParse(Extension.ParseConfigString("Server:RestartInterval"), out TimeSpan interval))
            {
                interval = TimeSpan.FromMinutes(30);
            }

            if (System.Math.Abs(interval.TotalSeconds) > 60)
            {
              Console.WriteLine($"Server Restarter has been loaded successfully. Restart Interval : {interval.TotalSeconds} seconds");

                var Timer = new Timer
                {
                    Interval = interval.TotalMilliseconds,
                    AutoReset = true
                };

                Timer.Elapsed += async (_Sender, _Args) =>
                {
                    try
                    {
                        Resources.Closing = true;
                        await Task.WhenAll(Task.Run(() =>
                        {
                            foreach (Player Player in Resources.Accounts.Players.Values.ToArray())
                            {
                                if (Player.Connected)
                                {
                                    new Disconnected(Player.Level.GameMode.Device).Send();
                                }
                            }
                        }));

                        await Task.WhenAll(Resources.Accounts.Saves());
                        await Task.WhenAll(Resources.Clans.Saves());
                    }
                    catch (Exception Exception)
                    {
                        Logging.Error(Exception.GetType(), "[: Failed at " + DateTime.Now.ToString("T") + ']' + Environment.NewLine + Exception.StackTrace);
                        return;
                    }

                    Environment.Exit(0);
                };

                if (!this.TryAdd(ServerTimer.ServerRestart, Timer))
                    Logging.Error(this.GetType(), $"Failed to add ServerRestart timer to ServerTimer dictionary");
            }
            else
            {
                Console.WriteLine("Server Restarter has been disabled");
            }
        }
    }
}
