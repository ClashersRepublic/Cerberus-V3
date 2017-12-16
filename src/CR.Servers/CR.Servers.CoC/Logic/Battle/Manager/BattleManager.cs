using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Core.Network;
using CR.Servers.CoC.Logic.Battle.Slots.Items;
using CR.Servers.CoC.Logic.Mode;
using CR.Servers.CoC.Packets;
using CR.Servers.CoC.Packets.Messages.Server.Battle;

using Timers = System.Timers.Timer;

namespace CR.Servers.CoC.Logic.Battle.Manager
{
    internal class BattleManager
    {

        internal Level Player;
        internal IBattle Battle;

        internal ConcurrentDictionary<long, Level> Spectators;
        internal BattleCommandManager BattleCommandManager;

        internal bool Started;
        internal bool Stopped;

        internal Timers Timer;

        internal DateTime Gamelastticktime;
        internal int Gamelasttick;

        internal bool Running => this.Started && !this.Stopped;


        public BattleManager(Level Player, IBattle Battle)
        {
            this.Player = Player;
            this.Battle = Battle;

            this.Timer = new Timers();

            this.Spectators = new ConcurrentDictionary<long, Level>();
            this.BattleCommandManager = new BattleCommandManager(this);

            this.Begin();
        }

        internal void Begin()
        {
            if (this.Started || this.Stopped)
            {
                return;
            }

            this.Started = true;
            this.Timer.Interval = 2000;
            this.Timer.AutoReset = true;
            this.Timer.Elapsed += (Gaybdu, Nagy) =>
            {
                //int CurrentTick = Interlocked.Increment(ref this.Tick) - 1; // -1 because it returns the incremented tick.

                this.BattleCommandManager.Tick();

                var Buffered = this.BattleCommandManager.Commands.ToList();

                if (!this.Stopped && this.Spectators.Count > 0)
                {
                    foreach (Level Player in this.Spectators.Values.ToArray())
                    {
                        if (Player.GameMode.Connected)
                        {
                            new Live_Replay_Data(Player.GameMode.Device)
                            {
                                EndSubTick = this.Battle.EndTick,
                                Spectator = this.Spectators.Count,
                                Commands = Buffered
                            }.Send();
                        }
                    }
                }
            };

            this.Timer.Start();
        }

        internal void Stop()
        {
            this.Timer.Stop();
            this.Timer.Close();

            this.Stopped = true;
            this.Started = false;

            this.BattleCommandManager.Tick();

            var Buffered = this.BattleCommandManager.Commands.ToList();

            if (!this.Stopped && this.Spectators.Count > 0)
            {
                foreach (Level Player in this.Spectators.Values.ToArray())
                {
                    if (Player.GameMode.Connected)
                    {
                        new Live_Replay_Data(Player.GameMode.Device)
                        {
                            EndSubTick = this.Battle.EndTick,
                            Spectator = this.Spectators.Count,
                            Commands = Buffered
                        }.Send();
                    }
                }
            }


            Logging.Info(this.GetType(), "Warning, a battle is ending.");
        }


        internal void AddSpectator(Level Player)
        {
            if (Player.GameMode.Connected)
            {
                if (this.Spectators.TryAdd(Player.Player.UserId, Player))
                {
                    new Live_Replay_Header(Player.GameMode.Device){ReplayHeaderJson = this.Battle.Save().ToString(), InitialStreamEndSubTick = this.Battle.EndTick, InitialCommands = this.Battle.Commands}.Send();
                }
                else
                {
                    this.Spectators[Player.Player.UserId] = Player;
                    new Live_Replay_Header(Player.GameMode.Device) { ReplayHeaderJson = this.Battle.Save().ToString(), InitialStreamEndSubTick = this.Battle.EndTick, InitialCommands = this.Battle.Commands }.Send();
                }
            }
        }

        internal void RemoveSpectator(Level Player)
        {
            if (this.Spectators.TryRemove(Player.Player.UserId, out _))
            {
                // Send.
            }
        }

        internal void Tick()
        {          
            this.Battle.BattleTick = this.Player.Time.SubTick;
            /*this.BattleCommandManager.Tick();

            var Buffered = this.BattleCommandManager.Commands.ToList();

            if (!this.Stopped && this.Spectators.Count > 0)
            {
                foreach (Level Player in this.Spectators.Values.ToArray())
                {
                    if (Player.GameMode.Connected)
                    {
                        new Live_Replay_Data(Player.GameMode.Device)
                        {
                            EndSubTick = this.Battle.EndTick,
                            Spectator = this.Spectators.Count,
                            Commands = Buffered
                        }.Send();
                    }
                }
            }


            this.Battle.BattleTick = this.Player.Time.SubTick;
            this.FakeTick = this.Player.Time.SubTick;*/
        }
    }
}
