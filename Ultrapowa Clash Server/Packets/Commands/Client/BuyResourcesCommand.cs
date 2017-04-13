using System;
using System.IO;
using UCS.Core;
using UCS.Files.Logic;
using UCS.Helpers;
using UCS.Logic;

namespace UCS.PacketProcessing.Commands.Client
{
    internal class BuyResourcesCommand : Command
    {
        internal object m_vCommand;
        internal bool m_vIsCommandEmbedded;
        internal int m_vResourceCount;
        internal int m_vResourceId;
        internal int Unknown1;

        public BuyResourcesCommand(PacketReader br)
        {
            m_vResourceId = br.ReadInt32();
            m_vResourceCount = br.ReadInt32();
            m_vIsCommandEmbedded = br.ReadBoolean();
            if (m_vIsCommandEmbedded)
            {
                Depth = Depth + 1;

                if (Depth >= 10)
                {
                    Console.WriteLine("Detected Exploit.");
                    return;
                }
                else
                {
                    m_vCommand = CommandFactory.Read(br, Depth);
                }
            }
            Unknown1 = br.ReadInt32();
        }

        public override void Execute(Level level)
        {
            if (Depth >= 10)
            {
                //TODO: Block Exploitor's IP.
                ResourcesManager.DropClient(level.GetClient().GetSocketHandle());
            }
            else
            {
                ResourceData dataById = (ResourceData)CSVManager.DataTables.GetDataById(m_vResourceId);
                if (dataById == null || m_vResourceCount < 1 || dataById.PremiumCurrency)
                    return;

                ClientAvatar avatar = level.GetPlayerAvatar();
                int resourceDiamondCost = GamePlayUtil.GetResourceDiamondCost(m_vResourceCount, dataById);
                if (m_vResourceCount > avatar.GetUnusedResourceCap(dataById) || !avatar.HasEnoughDiamonds(resourceDiamondCost))
                    return;

                avatar.UseDiamonds(resourceDiamondCost);
                avatar.CommodityCountChangeHelper(0, (Data)dataById, m_vResourceCount);
                if (!m_vIsCommandEmbedded)
                    return;

                if (m_vIsCommandEmbedded && m_vCommand != null)
                {
                    var cmd = (Command)m_vCommand;
                    cmd.Execute(level);
                }
            }
        }
    }
}
