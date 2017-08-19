using Magic.ClashOfClans.Extensions.Binary;
using Magic.ClashOfClans.Logic.Components;

namespace Magic.ClashOfClans.Network.Commands.Client
{
    internal class Change_Weapon_Mode : Command
    {
        internal int BuildingID;
        internal int Tick;

        public Change_Weapon_Mode(Reader reader, Device client, int id) : base(reader, client, id)
        {
        }

        public override void Decode()
        {
            BuildingID = Reader.ReadInt32();
            Reader.ReadByte();
            Reader.ReadInt32();
            Tick = Reader.ReadInt32();
        }

        public override void Process()
        {
            var Object =
                Device.Player.GameObjectManager.GetGameObjectByID(BuildingID,
                    Device.Player.Avatar.Variables.IsBuilderVillage);
            if (Object?.GetComponent(1, false) == null)
                return;
            var a = (Combat_Component) Object.GetComponent(1, false);
            if (a.AltAttackMode)
            {
                a.AttackMode = !a.AttackMode;
                a.AttackModeDraft = !a.AttackModeDraft;
            }

            if (a.AltTrapAttackMode)
            {
                a.AirMode = !a.AirMode;
                a.AirModeDraft = !a.AirModeDraft;
            }
        }
    }
}
