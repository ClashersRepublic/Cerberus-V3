﻿using System.IO;
using UCS.Core;
using UCS.Files.Logic;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Commands.Client
{
    internal class BuyDecoCommand : Command
    {
        public int DecoId;
        public uint Unknown1;
        public int X;
        public int Y;

        public BuyDecoCommand(PacketReader br)
        {
            X = br.ReadInt32();
            Y = br.ReadInt32();
            DecoId = br.ReadInt32();
            Unknown1 = br.ReadUInt32();
        }

        public override void Execute(Level level)
        {
            ClientAvatar avatar = level.GetPlayerAvatar();

            DecoData dataById = (DecoData) CSVManager.DataTables.GetDataById(DecoId);

            if (!avatar.HasEnoughResources(dataById.GetBuildResource(), dataById.GetBuildCost()))
              return;
            ResourceData buildResource = dataById.GetBuildResource();
            avatar.CommodityCountChangeHelper(0, (Data) buildResource, -dataById.GetBuildCost());

            Deco deco = new Deco((Data) dataById, level);
            deco.SetPositionXY(X, Y, level.GetPlayerAvatar().GetActiveLayout());
            level.GameObjectManager.AddGameObject((GameObject) deco);
        }
    }
}