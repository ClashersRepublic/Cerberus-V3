using System;
using System.Linq;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Extensions.Game;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Clan;
using CR.Servers.CoC.Logic.Enums;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Troop_Request : Command
    {
        internal override int Type => 511;

        public Troop_Request(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal bool HaveMessage;
        internal string Message;

        internal override void Decode()
        {
            base.Decode();

            this.HaveMessage = this.Reader.ReadBoolean();

            if (this.HaveMessage)
            {
                this.Message = this.Reader.ReadString();
            }
        }

        internal override void Execute()
        {
            var Player = this.Device.GameMode.Level.Player;

            if (Player.InAlliance)
            {
                var Bunker = this.Device.GameMode.Level.GameObjectManager.Bunker;
                if (Bunker != null)
                {
                    var BunkerComponent = Bunker.BunkerComponent;
                    if (BunkerComponent != null)
                    {
                        if (BunkerComponent.CanSendUnitRequest)
                        {
                            var Alliance = Player.Alliance;

                            foreach (var entry in Alliance.Streams.Slots.Values.Where(T => T.SenderHighId == Player.HighID && T.SenderLowId == Player.LowID && T.Type == AllianceStream.Donate).ToArray())
                            {
                                Alliance.Streams.Remove(entry);
                            }

                            Alliance.Streams.AddEntry(new DonateStreamEntry(Player.AllianceMember)
                            {
                                MaxTroop = Player.CastleTotalCapacity,
                                MaxSpell = Player.CastleTotalSpellCapacity,
                                UsedTroopSend = Player.CastleUsedCapacity,
                                UsedTroop = Player.CastleUsedCapacity,
                                UsedSpell = Player.CastleUsedSpellCapacity,
                                Units = Player.AllianceUnits.Copy(),
                                HaveMessage = this.HaveMessage,
                                Message = this.Message
                            });

                            BunkerComponent.UnitRequestTimer = new Timer();
                            BunkerComponent.UnitRequestTimer.StartTimer(Player.LastTick, Globals.AllianceTroopRequestCooldown);
                        }
                        else
                            Logging.Error(this.GetType(), "Unable to request troop. The player BunkerComponent.CanSendUnitRequest returned false!");
                    }
                    else
                        Logging.Error(this.GetType(), "Unable to request troop. The player BunkerComponent is null!");
                }
                else
                    Logging.Error(this.GetType(), "Unable to request troop. The player doesn't have a bunker!");
            }
            else
                Logging.Error(this.GetType(), "Unable to request troop. The player is not in a clan!");
        }
    }
}
