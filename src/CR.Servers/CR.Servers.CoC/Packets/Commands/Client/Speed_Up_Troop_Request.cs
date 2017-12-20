using CR.Servers.CoC.Core;
using CR.Servers.CoC.Extensions.Game;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Speed_Up_Troop_Request : Command
    {
        internal override int Type => 541;

        public Speed_Up_Troop_Request(Device Device, Reader Reader) : base(Device, Reader)
        {
            
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
                        if (!BunkerComponent.CanSendUnitRequest)
                        {
                            var RemainingSeconds = BunkerComponent.UnitRequestTimer.GetRemainingSeconds(Player.LastTick);

                            if (RemainingSeconds > 0)
                            {
                                int Cost = GamePlayUtil.GetSpeedUpCost(RemainingSeconds, 0, Globals.TroopRequestSpeedUpCostMultiplier);

                                if (Cost > 0)
                                {
                                    if (Player.HasEnoughDiamonds(Cost))
                                    {
                                        Player.UseDiamonds(Cost);
                                        BunkerComponent.UnitRequestTimer = null;
                                    }
                                }
                            }
                        }
                        else
                            Logging.Error(this.GetType(), "Unable to speed up the troop request. The player BunkerComponent.CanSendUnitRequest returned true!");
                    }
                    else
                        Logging.Error(this.GetType(), "Unable to speed up the troop request. The player BunkerComponent is null!");
                }
                else
                    Logging.Error(this.GetType(), "Unable to speed up the troop request. The player doesn't have a bunker!");
            }
            else
                Logging.Error(this.GetType(), "Unable to speed up the troop request. The player is not in a clan!");
        }
    }
}
