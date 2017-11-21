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

        internal override void Decode()
        {
            this.BuildingID = this.Reader.ReadInt32();
            this.Reader.ReadByte();
            this.Reader.ReadInt32();
            base.Decode();
        }
        internal int BuildingID;

        internal override void Execute()
        {
            var Level = this.Device.GameMode.Level;

            var GameObject = Level.GameObjectManager.Filter.GetGameObjectById(this.BuildingID);

            if (GameObject != null)
            {
                if (GameObject.Type == 0)
                {
                    var Building = GameObject as Building;
                    var CombatComponent = Building?.CombatComponent;

                    if (CombatComponent != null)
                    {
                        if (CombatComponent.AltAttackMode)
                        {
                            CombatComponent.AttackMode = !CombatComponent.AttackMode;
                            CombatComponent.AttackModeDraft = !CombatComponent.AttackModeDraft;
                        }
                    }
                }
            }
            else
            {
                Logging.Error(this.GetType(),  "Unable to change the weapon mode. GameObject is not valid or not exist.");
            }
        }
    }
}
