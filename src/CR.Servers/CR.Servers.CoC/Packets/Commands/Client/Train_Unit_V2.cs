using CR.Servers.CoC.Extensions.Helper;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Train_Unit_V2 : Command
    {
        internal override int Type => 592;

        public Train_Unit_V2(Device Device, Reader Reader) : base(Device, Reader)
        {
            
        }

        internal int BuildingId;
        internal int UnitOfType;
        internal CharacterData Unit;

        internal override void Decode()
        {
            this.BuildingId = this.Reader.ReadInt32();
            this.UnitOfType = this.Reader.ReadInt32();
            this.Unit = this.Reader.ReadData<CharacterData>();
            base.Decode();
        }

        internal override void Execute()
        {
            var Level = this.Device.GameMode.Level;
            var GameObject = Level.GameObjectManager.Filter.GetGameObjectById(this.BuildingId);
            if (GameObject != null)
            {
                if (GameObject is Building Building)
                {
                    var UnitStorageV2Component = Building.UnitStorageV2Component;

                    if (UnitStorageV2Component != null)
                    {
                        UnitStorageV2Component.AddUnit(this.Unit);
                    }
                }
            }
        }
    }
}
