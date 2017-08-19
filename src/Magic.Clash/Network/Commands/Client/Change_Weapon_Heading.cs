using System;
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
            if (Object?.GetComponent(1, false
                ) == null)
                return;
            var a = (Combat_Component) Object.GetComponent(1, false);
            if (a.AimRotateStep)
            {
                Console.WriteLine(a.AimAngle);
                a.AimAngle = a.AimAngle == 360 ? 45 : a.AimAngle + 45;
                a.AimAngleDraft = a.AimAngleDraft == 360 ? 45 : a.AimAngleDraft + 45;
            }

            if (a.AltDirectionMode)
            {
                a.TrapDirection = a.TrapDirection == 4 ? 0 : a.TrapDirection++;
                a.TrapDirectionDraft = a.TrapDirectionDraft == 4 ? 0 : a.TrapDirectionDraft++;
            }
        }
    }
}
