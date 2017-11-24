﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Change_Weapon_Heading : Command
    {
        internal override int Type => 554;

        public Change_Weapon_Heading(Device device, Reader reader) : base(device, reader)
        {
        }

        internal int BuildingID;

        internal override void Decode()
        {
            this.BuildingID = this.Reader.ReadInt32();
            this.Reader.ReadByte();
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
            this.Reader.ReadByte();
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
                        if (CombatComponent.AimRotateStep)
                        {
                            CombatComponent.AimAngle = CombatComponent.AimAngle == 360 ? 45 : CombatComponent.AimAngle + 45;
                            CombatComponent.AimAngleDraft = CombatComponent.AimAngleDraft == 360 ? 45 : CombatComponent.AimAngleDraft + 45;
                        }
                    }
                    else
                        Logging.Error(this.GetType(), "Unable to change the weapon heading. The CombatComponent for the game object is null.");
                }
                else if (GameObject is Trap Trap)
                {
                    var CombatComponent = Trap.CombatComponent;

                    if (CombatComponent != null)
                    {
                        if (CombatComponent.AltDirectionMode)
                        {
                            CombatComponent.TrapDirection = CombatComponent.TrapDirection == 3 ? 0 : ++CombatComponent.TrapDirection;
                            CombatComponent.TrapDirectionDraft = CombatComponent.TrapDirectionDraft == 3 ? 0 : ++CombatComponent.TrapDirectionDraft;
                        }
                    }
                    else
                        Logging.Error(this.GetType(), "Unable to change the trap heading. The CombatComponent for the game object is null.");
                }
                else
                    Logging.Error(this.GetType(),
                        "Unable to change the weapon heading. Unable to determine game object");
            }
            else
                Logging.Error(this.GetType(), "Unable to change the weapon heading. The game object is null.");
        }
    }
}
