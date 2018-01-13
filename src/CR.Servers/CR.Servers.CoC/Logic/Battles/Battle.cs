using CR.Servers.CoC.Packets.Messages.Server.Account;

namespace CR.Servers.CoC.Logic.Battles
{
    using System;
    using System.Collections.Generic;
    using CR.Servers.CoC.Core;
    using CR.Servers.CoC.Core.Network;
    using CR.Servers.CoC.Packets;
    using CR.Servers.CoC.Packets.Commands.Client.Battle;
    using CR.Servers.CoC.Packets.Messages.Server.Battle;
    using CR.Servers.CoC.Packets.Messages.Server.Home;
    using Timer = System.Timers.Timer;

    internal class Battle
    {
        internal Level Attacker;
        internal List<Command> Commands;
        internal Level Defender;
        internal Device Device;
        internal bool Started;
        internal bool Ended;

        internal int EndSubTick;
        internal DateTime LastClientTurn;
        internal BattleLog BattleLog;
        internal BattleRecorder Recorder;

        internal List<Device> Viewers;

        internal Timer Timer;


        /// <summary>
        ///     Initializes a new instance of the <see cref="Battle" /> class.
        /// </summary>
        internal Battle(Device device, Level attacker, Level defender)
        {
            this.Device = device;
            this.Attacker = attacker;
            this.Defender = defender;

            this.BattleLog = new BattleLog(this);
            this.Recorder = new BattleRecorder(this);
            this.Commands = new List<Command>(64);
            this.Viewers = new List<Device>(32);
            this.EndClientTurn();
        }

        internal void AddViewer(Device device)
        {
            if (!this.Viewers.Contains(device) && !this.Ended)
            {
                this.Viewers.Add(device);

                new LiveReplayMessage(device, this.Recorder.Save().ToString(), this.Commands, this.EndSubTick).Send();
            }
        }

        internal void EndBattle()
        {
            if (this.Ended)
            {
                return;
            }

            this.Timer.Dispose();

            if (this.Started)
            {
                var replay = Resources.Battles.Save(this);

                this.Defender.Player.Inbox.Add(new BattleReportStreamEntry(this.Attacker.Player, this, (long) replay.HighId << 32 | (uint) replay.LowId, 2));
                this.Attacker.Player.Inbox.Add(new BattleReportStreamEntry(this.Defender.Player, this, (long) replay.HighId << 32 | (uint) replay.LowId, 7));

                if (this.Commands.Count > 0)
                {
                    Resources.Accounts.SavePlayer(this.Defender.Player);
                    Resources.Accounts.SaveHome(this.Defender.Home);
                }
            }

            for (int i = 0; i < this.Viewers.Count; i++)
            {
                if (this.Viewers[i].Connected)
                {
                    if (this.Viewers[i].GameMode.Level.Player.UserId == this.Defender.Player.UserId)
                    {
                        new OwnHomeDataMessage(this.Viewers[i]).Send();
                    }
                }
            }

            this.Ended = true;
            this.Device.Account.Battle = null;
        }

        internal bool RemoveViewer(Device device)
        {
            return this.Viewers.Remove(device);
        }

        internal void HandleCommands(int subTick, List<Command> commands)
        {
            this.EndSubTick = subTick;
            this.Recorder.EndTick = subTick;
            this.LastClientTurn = DateTime.UtcNow;

            if (commands != null)
            {
                for (int i = 0; i < commands.Count; i++)
                {
                    if (commands[i].Type != 800)
                    {
                        if (!this.Started)
                        {
                            this.Started = true;
                        }

                        this.Recorder.Commands.Add(commands[i].Save());
                        this.Commands.Add(commands[i]);
                    }
                }
            }

            for (int i = 0; i < this.Viewers.Count; i++)
            {
                if (this.Viewers[i].Connected)
                {
                    new LiveReplayDataMessage(this.Viewers[i])
                    {
                        EndSubTick = subTick,
                        Commands = commands,
                        //SpectatorTeam1 = this.Viewers.Count
                    }.Send();
                }
                else
                {
                    if (this.RemoveViewer(this.Viewers[i]))
                    {
                        --i;
                    }
                }
            }

            if (commands != null)
            {
                for (int i = 0; i < commands.Count; i++)
                {
                    switch (commands[i].Type)
                    {
                        case 700:
                        {
                            Place_Attacker placeAttacker = (Place_Attacker) commands[i];
                            this.BattleLog.IncrementUnit(placeAttacker.Character);
                            break;
                        }

                        case 701:
                        {
                            this.BattleLog.AlliancePortalDeployed();
                            break;
                        }

                        case 703:
                        {
                            this.EndBattle();
                            break;
                        }

                        case 704:
                        {
                            Place_Spell placeSpell = (Place_Spell) commands[i];
                            this.BattleLog.IncrementSpell(placeSpell.Spell);
                            break;
                        }

                        case 705:
                        {
                            Place_Hero placeHero = (Place_Hero) commands[i];
                            this.BattleLog.HeroDeployed(placeHero.Hero);
                            break;
                        }
                    }
                }
            }
        }

        internal void EndClientTurn()
        {
            if (this.Ended)
            {
                return;
            }

            this.Timer = new Timer();
            this.Timer.Interval = 2000;
            this.Timer.AutoReset = true;
            this.Timer.Elapsed += (Aidid, Mike) =>
            {
                int LastClientTurnSeconds = (int) DateTime.UtcNow.Subtract(this.LastClientTurn).TotalSeconds;
                if (LastClientTurnSeconds > 5)
                {
                    this.EndBattle();

                    if (this.Device.TimeSinceLastKeepAlive > 5)
                    {
                        Resources.Gateway.Disconnect(this.Device.Token.Args);
                    }

                    this.Timer.Stop();
                }
            };

            this.Timer.Start();
        }
    }
}