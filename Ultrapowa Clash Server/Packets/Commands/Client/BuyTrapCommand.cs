using System.IO;
using UCS.Core;
using UCS.Files.Logic;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Commands.Client
{
    internal class BuyTrapCommand : Command
    {
        public int TrapId;
        public uint Unknown1;
        public int X;
        public int Y;

        public BuyTrapCommand(PacketReader br)
        {
            X = br.ReadInt32();
            Y = br.ReadInt32();
            TrapId = br.ReadInt32();
            Unknown1 = br.ReadUInt32();
        }

        public override void Execute(Level level)
        {
            ClientAvatar avatar = level.GetPlayerAvatar();
            TrapData dataById = (TrapData) CSVManager.DataTables.GetDataById(TrapId);
            Trap trap = new Trap((Data) dataById, level);

            if (!avatar.HasEnoughResources(dataById.GetBuildResource(0), dataById.GetBuildCost(0)) || level.HasFreeWorkers())
              return;
            ResourceData buildResource = dataById.GetBuildResource(0);
            avatar.CommodityCountChangeHelper(0, (Data) buildResource, -dataById.GetBuildCost(0));
            trap.StartConstructing(X, Y);
            level.GameObjectManager.AddGameObject((GameObject) trap);
        }
    }
}