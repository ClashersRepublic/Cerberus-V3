using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Gear_Up : Command
    {
        internal override int Type => 600;


        public Gear_Up(Device device, Reader reader) : base(device, reader)
        {
        }

        internal override void Decode()
        {
            this.BuildingID = this.Reader.ReadInt32();
            base.Decode();
        }
        internal int BuildingID;

        internal override void Execute()
        {
            var Level = this.Device.GameMode.Level;

            Building Building = (Building)Level.GameObjectManager.Filter.GetGameObjectById(this.BuildingID);

            CombatComponent CombatComponent = Building?.CombatComponent;

            if (CombatComponent != null)
            {
                CombatComponent.GearUp = 1;
                if (CombatComponent.AltAttackMode)
                {
                    CombatComponent.AttackMode = true;
                    CombatComponent.AttackModeDraft = true;
                }
            }
        }
    }
}
