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

        internal int Id;

        internal override void Decode()
        {
            this.Id = Reader.ReadInt32();
            ExecuteSubTick = Reader.ReadInt32();
        }
        
        internal override void Execute()
        {
            GameObject GameObject = this.Device.GameMode.Level.GameObjectManager.Filter.GetGameObjectById(this.Id);

            if (GameObject != null)
            {
                switch (GameObject.Type)
                {
                    case 0:
                    {
                        ((Building)GameObject).SpeedUpConstruction();
                        break;
                    }
                    case 8:
                    {
                        ((VillageObject)GameObject).SpeedUpConstruction();
                        break;
                    }
                    default:
                    {
                        Logging.Error(this.GetType(), "Unable to speed up the construction. GameObject Type : " + GameObject.Type + ".");
                        break;
                    }
                }
            }
            else
                Logging.Error(this.GetType(), "Unable to speed up the construction. The game object doesn't exist.");
        }
    }
}