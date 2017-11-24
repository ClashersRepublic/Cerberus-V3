using CR.Servers.CoC.Core;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Change_Weapon_Mode : Command
    {
        internal override int Type => 524;

        public Change_Weapon_Mode(Device device, Reader reader) : base(device, reader)
        {
        }

        internal int BuildingID;

        internal override void Decode()
        {
            this.BuildingID = this.Reader.ReadInt32();
            this.Reader.ReadByte();
            this.Reader.ReadInt32();
            base.Decode();
        }

        internal override void Execute()
        {
            var Level = this.Device.GameMode.Level;

            var GameObject = Level.GameObjectManager.Filter.GetGameObjectById(this.BuildingID);

            if (GameObject != null)
            {
                if (GameObject is Building Building)
                {
                    var CombatComponent = Building.CombatComponent;

                    if (CombatComponent != null)
                    {
                        if (CombatComponent.AltAttackMode)
                        {
                            CombatComponent.AttackMode = !CombatComponent.AttackMode;
                            CombatComponent.AttackModeDraft = !CombatComponent.AttackModeDraft;
                        }
                    }
                    else
                        Logging.Error(this.GetType(),
                            "Unable to change the weapon mode. The CombatComponent for the game object is null.");
                }

                else if (GameObject is Trap Trap)
                {
                    var CombatComponent = Trap.CombatComponent;

                    if (CombatComponent != null)
                    {
                        if (CombatComponent.AltTrapAttackMode)
                        {
                            CombatComponent.AirMode = !CombatComponent.AirMode;
                            CombatComponent.AirModeDraft = !CombatComponent.AirModeDraft;
                        }
                    }
                    else
                        Logging.Error(this.GetType(), "Unable to change the trap mode. The CombatComponent for the game object is null.");

                }
                else
                    Logging.Error(this.GetType(), "Unable to change the weapon mode. Unable to determine game object");
            }
            else
                Logging.Error(this.GetType(), "Unable to change the weapon mode. The game object is null.");
        }
    }
}
