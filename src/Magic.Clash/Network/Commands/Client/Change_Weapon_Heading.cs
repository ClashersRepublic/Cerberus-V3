using System;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Logic.Components;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class Change_Weapon_Heading : Command
    {
        internal int BuildingID;
        internal int Tick;

        public Change_Weapon_Heading(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            BuildingID = Reader.ReadInt32();
            Reader.ReadByte();
            Reader.ReadInt32();
            Reader.ReadString();
            Reader.ReadByte();
            Tick = Reader.ReadInt32();
        }

        public override void Process()
        {
            var Object =
                Device.Player.GameObjectManager.GetGameObjectByID(BuildingID,
                    Device.Player.Avatar.Variables.IsBuilderVillage);

            if (Object != null)
            {
                var a = Object.GetComponent(1, false);
                if (a == null)
                    return;

                var component = (Combat_Component)a;
                if (component.AimRotateStep)
                {
                    component.AimAngle = component.AimAngle == 360 ? 45 : component.AimAngle + 45;
                    component.AimAngleDraft = component.AimAngleDraft == 360 ? 45 : component.AimAngleDraft + 45;
                }

                if (component.AltDirectionMode)
                {
                    component.TrapDirection = component.TrapDirection == 4 ? 0 : component.TrapDirection++;
                    component.TrapDirectionDraft = component.TrapDirectionDraft == 4
                        ? 0
                        : component.TrapDirectionDraft++;
                }
            }
            else
            {
                ExceptionLogger.Log(new NullReferenceException(),
                    $"Object with id {BuildingID} from user {Device.Player.Avatar.UserId} is null at Change Weapon Heading");
            }
        }
    }
}
