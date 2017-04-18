using System.IO;
using Magic.Core;
using Magic.Files.Logic;
using Magic.Helpers;
using Magic.Logic;

namespace Magic.PacketProcessing.Commands.Client
{
    internal class CancelConstructionCommand : Command
    {
        public int BuildingId;
        public uint Unknown1;

        public CancelConstructionCommand(PacketReader br)
        {
            BuildingId = br.ReadInt32(); 
            Unknown1 = br.ReadUInt32();
        }

        public override void Execute(Level level)
        {
            GameObject gameObjectById = level.GameObjectManager.GetGameObjectByID(BuildingId);
            if (gameObjectById == null)
              return;
                if (gameObjectById.ClassId == 0 || gameObjectById.ClassId == 4)
                {
                    ConstructionItem constructionItem = (ConstructionItem) gameObjectById;
                    if (!constructionItem.IsConstructing())
                      return;
                    ClientAvatar avatar = level.Avatar;
                    string name = level.GameObjectManager.GetGameObjectByID(BuildingId).Data.Name;
                        Logger.Write("Canceling Building Upgrade: " + name + " (" + BuildingId + ')');
                        if (string.Equals(name, "Alliance Castle"))
                        {
                            avatar.DeIncrementAllianceCastleLevel();
                            BuildingData buildingData = ((Building)gameObjectById).GetBuildingData();
                            avatar.SetAllianceCastleTotalCapacity(buildingData.GetUnitStorageCapacity(avatar.GetAllianceCastleLevel() - 1));
                        }
                        else if (string.Equals(name, "Town Hall"))
                          avatar.DeIncrementTownHallLevel();
                        constructionItem.CancelConstruction();
                }
                else
                {
                  int classId = gameObjectById.ClassId;
                }
        }
    }
}
