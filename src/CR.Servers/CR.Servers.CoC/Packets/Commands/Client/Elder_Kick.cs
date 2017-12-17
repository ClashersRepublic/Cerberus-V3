using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Extensions;
using CR.Servers.CoC.Extensions.Game;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Clan;
using CR.Servers.CoC.Logic.Clan.Items;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.CoC.Packets.Commands.Server;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Elder_Kick : Command
    {
        internal override int Type => 543;

        public Elder_Kick(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal int HighId;
        internal int LowId;
        internal bool HaveMessage;
        internal string Message;

        internal override void Decode()
        {
            this.HighId = this.Reader.ReadInt32();
            this.LowId = this.Reader.ReadInt32();
            this.HaveMessage = this.Reader.ReadBoolean();

            if (this.HaveMessage)
            {
                this.Message = this.Reader.ReadString();
            }

            base.Decode();
        }

        internal override void Execute()
        {
            var Level = this.Device.GameMode.Level;
            var Target = Resources.Accounts.LoadAccount(this.HighId, this.LowId)?.Player;

            if (Level.Player.InAlliance)
            {
                var Alliance = Level.Player.Alliance;
                if (Target != null)
                {
                    if (Target.InAlliance)
                    {
                        if (Target.AllianceId == Level.Player.AllianceId)
                        {
                            var ExecutorRole = Level.Player.AllianceMember.Role;
                            if (!string.IsNullOrWhiteSpace(this.Message))
                            {
                                if (this.Message.Length <= 256)
                                {
                                    this.Message = Resources.Regex.Replace(this.Message, " ");

                                    if (this.Message.StartsWith(" "))
                                    {
                                        this.Message = this.Message.Remove(0, 1);
                                    }

                                    if (ExecutorRole == Role.Member)
                                    {
                                        Logging.Error(this.GetType(), "Unable to kick player from the alliance. The executer have a member role!");
                                    }
                                    else if (ExecutorRole == Role.Leader || ExecutorRole == Role.CoLeader)
                                    {
                                        if (Target.AllianceMember.Role.Superior(ExecutorRole))
                                        {
                                            if (Alliance.Members.Quit(Target, out Member Member))
                                            {
                                                if (Target.Connected)
                                                {
                                                    Target.Level.GameMode.CommandManager.AddCommand(
                                                        new Leaved_Alliance(Target.Level.GameMode.Device)
                                                        {
                                                            AllianceId = Alliance.AllianceId
                                                        });
                                                }
                                                else
                                                {
                                                    Target.AllianceHighId = 0;
                                                    Target.AllianceLowId = 0;
                                                    Target.AllianceMember = null;
                                                    Target.Alliance = null;
                                                }

                                                Target.Inbox.Add(
                                                    new AllianceKickOutEntry(Level.Player, Alliance)
                                                    {
                                                        Message = this.Message
                                                    });

                                                Alliance.Streams.AddEntry(new EventStreamEntry(Member, Level.Player.AllianceMember, AllianceEvent.Kicked));
                                            }
                                            else
                                                Logging.Error(this.GetType(), "Unable to kick player from the alliance. The Quit() returned false!");
                                        }
                                        else
                                            Logging.Error(this.GetType(), "Unable to kick player from the alliance. The executor have a lower role than the target!");
                                    }
                                    else if (ExecutorRole == Role.Elder)
                                    {
                                        var Bunker = Level.GameObjectManager.Bunker;
                                        if (Bunker != null)
                                        {
                                            var BunkerComponent = Bunker.BunkerComponent;
                                            if (BunkerComponent != null)
                                            {
                                                if (BunkerComponent.CanElderKick)
                                                {
                                                    if (Target.AllianceMember.Role.Superior(ExecutorRole))
                                                    {
                                                        if (Alliance.Members.Quit(Target, out Member Member))
                                                        {
                                                            if (Target.Connected)
                                                            {
                                                                Target.Level.GameMode.CommandManager.AddCommand(
                                                                    new Leaved_Alliance(Target.Level.GameMode.Device)
                                                                    {
                                                                        AllianceId = Alliance.AllianceId
                                                                    });
                                                            }
                                                            else
                                                            {
                                                                Target.AllianceHighId = 0;
                                                                Target.AllianceLowId = 0;
                                                                Target.AllianceMember = null;
                                                                Target.Alliance = null;
                                                            }

                                                            Target.Inbox.Add(
                                                                new AllianceKickOutEntry(Level.Player, Alliance)
                                                                {
                                                                    Message = this.Message
                                                                });

                                                            Alliance.Streams.AddEntry(new EventStreamEntry(Member, Level.Player.AllianceMember, AllianceEvent.Kicked));

                                                            BunkerComponent.ElderKickTimer = new Timer();
                                                            BunkerComponent.ElderKickTimer.StartTimer(Level.Player.LastTick, Globals.ElderKickCooldown);

                                                        }
                                                        else
                                                            Logging.Error(this.GetType(), "Unable to kick player from the alliance. The Quit() returned false!");
                                                    }
                                                else
                                                    Logging.Error(this.GetType(), "Unable to kick player from the alliance. The executor have a lower role than the target!");
                                                }
                                                else
                                                    Logging.Error(this.GetType(), "Unable to kick player from the alliance. The executor is still in cooldown mode!");
                                            }
                                            else
                                                Logging.Error(this.GetType(), "Unable to kick player from the alliance. The executor BunkerComponent is null!");
                                        }
                                        else
                                            Logging.Error(this.GetType(), "Unable to kick player from the alliance. The executor doesn't have bunker!");
                                    }
                                    else
                                        Logging.Error(this.GetType(), "Unable to kick player from the alliance. The executor doesn't have any recognisable role!");
                                }
                                else
                                    Logging.Error(this.GetType(), "Unable to kick player from the alliance. The executor send a message that beyond the size limit!");
                            }
                            else
                                Logging.Error(this.GetType(), "Unable to kick player from the alliance. The executor send a message that is null or whitespace!");
                        }
                        else
                            Logging.Error(this.GetType(), "Unable to kick player from the alliance. The player and the executer is not in the same clan!");
                    }
                    else
                        Logging.Error(this.GetType(), "Unable to kick player from the alliance. The player is not in a clan!");
                }
                else
                    Logging.Error(this.GetType(), "Unable to kick player from the alliance. The player is null!");
            }
            else
                Logging.Error(this.GetType(), "Unable to kick player from the alliance. The executor clan is null!");
        }
    }
}
