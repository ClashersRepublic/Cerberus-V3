namespace CR.Servers.CoC.Logic.Battles
{
    using System;
    using System.Collections.Generic;
    using CR.Servers.CoC.Core;
    using CR.Servers.CoC.Core.Network;
    using CR.Servers.CoC.Packets;
    using CR.Servers.CoC.Packets.Messages.Server.Battle;
    using CR.Servers.CoC.Packets.Messages.Server.Home;
    using Newtonsoft.Json;

    internal class Battle
    {
        internal DateTime LastClientTurn;

        internal Level Attacker;
        internal Level Defender;
        internal BattleRecorder Recorder;

        internal int EndSubTick;
        internal bool Ended;

        internal List<Device> Viewers;
        internal List<Command> Commands;


        /// <summary>
        ///     Initializes a new instance of the <see cref="Battle"/> class.
        /// </summary>
        internal Battle(Level attacker, Level defender)
        {
            this.Attacker = attacker;
            this.Defender = defender;

            this.Recorder = new BattleRecorder(this);
            this.Commands = new List<Command>(64);
            this.Viewers = new List<Device>(32);
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

            if (this.Commands.Count > 0)
            {
                Resources.Accounts.SavePlayer(this.Defender.Player);
                Resources.Accounts.SaveHome(this.Defender.Home);
            }

            for (int i = 0; i < this.Viewers.Count; i++)
            {
                if (this.Viewers[i].Connected)
                {
                    if (this.Viewers[i].GameMode.Level == this.Defender)
                    {
                        new OwnHomeDataMessage(this.Viewers[i]).Send();
                    }
                }
            }

            this.Ended = true;
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
                    this.Recorder.Commands.Add(commands[i].Save());
                    this.Commands.Add(commands[i]);
                }
            }

            for (int i = 0; i < this.Viewers.Count; i++)
            {
                if (this.Viewers[i].Connected)
                {
                    new LiveReplayDataMessage(this.Viewers[i])
                    {
                        EndSubTick = subTick,
                        Commands = commands
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
                    if (commands[i].Type == 703)
                    {
                        this.EndBattle();
                        break;
                    }
                }
            }
        }
    }
}