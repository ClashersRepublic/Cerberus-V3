namespace CR.Servers.CoC.Logic.Battle.Manager
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Timers;
    using CR.Servers.CoC.Core;
    using CR.Servers.CoC.Core.Network;
    using CR.Servers.CoC.Logic.Battle.Slots.Items;
    using CR.Servers.CoC.Packets;
    using CR.Servers.CoC.Packets.Messages.Server.Battle;

    internal class BattleManager
    {
        internal IBattle Battle;
        internal BattleCommandManager BattleCommandManager;
        internal int Gamelasttick;

        internal DateTime Gamelastticktime;

        internal Level Player;

        internal ConcurrentDictionary<long, Level> Spectators;

        internal bool Started;
        internal bool Stopped;

        internal Timer Timer;


        public BattleManager(Level Player, IBattle Battle)
        {
            this.Player = Player;
            this.Battle = Battle;

            this.Timer = new Timer();

            this.Spectators = new ConcurrentDictionary<long, Level>();
            this.BattleCommandManager = new BattleCommandManager(this);

            this.Begin();
        }

        internal bool Running => this.Started && !this.Stopped;

        internal void Begin()
        {
            if (this.Started || this.Stopped)
            {
                return;
            }

            this.Started = true;
            this.Timer.Interval = 1500;
            this.Timer.AutoReset = true;
            this.Timer.Elapsed += (Gaybdu, Nagy) =>
            {
                this.BattleCommandManager.Tick();

                List<Command> Buffered = this.BattleCommandManager.Commands.ToList();

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

            List<Command> Buffered = this.BattleCommandManager.Commands.ToList();

            if (this.Spectators.Count > 0)
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

                        new Live_Replay_End(Player.GameMode.Device).Send();
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
                    new Live_Replay_Header(Player.GameMode.Device) {ReplayHeaderJson = this.Battle.Save().ToString(), InitialStreamEndSubTick = this.Battle.EndTick, InitialCommands = this.Battle.Commands}.Send();
                }
                else
                {
                    this.Spectators[Player.Player.UserId] = Player;
                    new Live_Replay_Header(Player.GameMode.Device) {ReplayHeaderJson = this.Battle.Save().ToString(), InitialStreamEndSubTick = this.Battle.EndTick, InitialCommands = this.Battle.Commands}.Send();
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