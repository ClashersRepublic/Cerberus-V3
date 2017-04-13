﻿using System.IO;
using UCS.Files.Logic;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Commands.Client
{
    internal class SellBuildingCommand : Command
    {
        internal int m_vBuildingId;

        public SellBuildingCommand(PacketReader br)
        {
            m_vBuildingId = br.ReadInt32();
            br.ReadUInt32();
        }

        public override void Execute(Level level)
        {
            ClientAvatar avatar = level.GetPlayerAvatar();
            GameObject gameObjectById = level.GameObjectManager.GetGameObjectByID(m_vBuildingId);
            if (gameObjectById == null)
              return;
            if (gameObjectById.ClassId == 4)
            {
              Trap trap = (Trap) gameObjectById;
              int upgradeLevel = trap.GetUpgradeLevel();
              ResourceData buildResource = trap.GetTrapData().GetBuildResource(upgradeLevel);
              int sellPrice = trap.GetTrapData().GetSellPrice(upgradeLevel);
              avatar.CommodityCountChangeHelper(0, (Data)buildResource, sellPrice);
              level.GameObjectManager.RemoveGameObject((GameObject) trap);
            }
            else
            {
                if (gameObjectById.ClassId != 6)
                    return;
                Deco deco = (Deco)gameObjectById;
                ResourceData buildResource = deco.GetDecoData().GetBuildResource();
                int sellPrice = deco.GetDecoData().GetSellPrice();
                if (buildResource.PremiumCurrency)
                    avatar.SetDiamonds(avatar.GetDiamonds() + sellPrice);
                else
                    avatar.CommodityCountChangeHelper(0, (Data)buildResource, sellPrice);
                level.GameObjectManager.RemoveGameObject((GameObject) deco);
            }
        }
    }
}