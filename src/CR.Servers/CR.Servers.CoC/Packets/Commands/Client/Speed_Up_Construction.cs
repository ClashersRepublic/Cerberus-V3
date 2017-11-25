using System;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Speed_Up_Construction : Command
    {
        internal override int Type => 504;

        public Speed_Up_Construction(Device Client, Reader Reader) : base(Client, Reader)
        {
        }

        internal int BuildingId;
        internal int Village;

        internal override void Decode()
        {
            this.BuildingId = Reader.ReadInt32();
            this.Village = this.Reader.ReadInt32();
            base.Decode();
        }
        
        internal override void Execute()
        {
            GameObject GameObject = this.Device.GameMode.Level.GameObjectManager.Filter.GetGameObjectById(this.BuildingId);

            if (GameObject != null)
            {
                if (GameObject is Building Building)
                {
                    Building.SpeedUpConstruction();
                }
                else if (GameObject is Trap Trap)
                {
                    Trap.SpeedUpConstruction();
                }
                else if (GameObject is VillageObject VillageObject)
                {
                    VillageObject.SpeedUpConstruction();
                }
                else
                {
                    Logging.Error(this.GetType(), "Unable to speed up the construction. GameObject Type : " + GameObject.Type + ".");
                }
            }
            else
                Logging.Error(this.GetType(), "Unable to speed up the construction. The game object doesn't exist.");
        }
    }
}