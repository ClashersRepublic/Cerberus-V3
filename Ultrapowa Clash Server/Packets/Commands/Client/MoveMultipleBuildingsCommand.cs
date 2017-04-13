using System.Collections.Generic;
using System.IO;
using UCS.Helpers;
using UCS.Logic;
using UCS.Packets.Commands.Client;

namespace UCS.PacketProcessing.Commands.Client
{
    internal class MoveMultipleBuildingsCommand : Command
    {
        internal List<BuildingToMove> m_vBuildingsToMove;

        public MoveMultipleBuildingsCommand(PacketReader br)
        {
            m_vBuildingsToMove = new List<BuildingToMove>();
            int num = br.ReadInt32();
            for (int index = 0; index < num; ++index)
                this.m_vBuildingsToMove.Add(new BuildingToMove()
                {
                    X = br.ReadInt32(),
                    Y = br.ReadInt32(),
                    GameObjectId = br.ReadInt32()
                });
            br.ReadInt32();
        }

        public override void Execute(Level level)
        {
            foreach (BuildingToMove buildingToMove in m_vBuildingsToMove)
              level.GameObjectManager.GetGameObjectByID(buildingToMove.GameObjectId).SetPositionXY(buildingToMove.X, buildingToMove.Y, level.GetPlayerAvatar().GetActiveLayout());
        }
    }
}